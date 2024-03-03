#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Linq;
using UnityEngine;
using Cysharp.Threading.Tasks.Internal;
using System.Threading;
using UnityEngine.Assertions;

#if UNITY_2019_3_OR_NEWER
using UnityEngine.LowLevel;
using PlayerLoopType = UnityEngine.PlayerLoop;
#else
using UnityEngine.Experimental.LowLevel;
using PlayerLoopType = UnityEngine.Experimental.PlayerLoop;
#endif

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Cysharp.Threading.Tasks
{
    public enum PlayerLoopTiming
    {
        Update = 0,
    }

    public interface IPlayerLoopItem
    {
        bool MoveNext();
    }

    public static class PlayerLoopHelper
    {
        public static SynchronizationContext UnitySynchronizationContext => unitySynchronizationContext;
        public static int MainThreadId => mainThreadId;
        internal static string ApplicationDataPath => applicationDataPath;

        public static bool IsMainThread => Thread.CurrentThread.ManagedThreadId == mainThreadId;

        static int mainThreadId;
        static string applicationDataPath;
        static SynchronizationContext unitySynchronizationContext;
        static ContinuationQueue updateYielder;
        static PlayerLoopRunner updateRunner;
        internal static bool IsEditorApplicationQuitting { get; private set; }

#if UNITY_2020_1_OR_NEWER
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
#else
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
#endif
        static void Init()
        {
            // capture default(unity) sync-context.
            unitySynchronizationContext = SynchronizationContext.Current;
            mainThreadId = Thread.CurrentThread.ManagedThreadId;
            try
            {
                applicationDataPath = Application.dataPath;
            }
            catch { }

#if UNITY_EDITOR && UNITY_2019_3_OR_NEWER
            // When domain reload is disabled, re-initialization is required when entering play mode; 
            // otherwise, pending tasks will leak between play mode sessions.
            var domainReloadDisabled = UnityEditor.EditorSettings.enterPlayModeOptionsEnabled &&
                UnityEditor.EditorSettings.enterPlayModeOptions.HasFlag(UnityEditor.EnterPlayModeOptions.DisableDomainReload);
            if (!domainReloadDisabled && updateRunner != null) return;
#else
            if (updateRunner != null) return; // already initialized
#endif

            var playerLoop =
#if UNITY_2019_3_OR_NEWER
                PlayerLoop.GetCurrentPlayerLoop();
#else
                PlayerLoop.GetDefaultPlayerLoop();
#endif

            Initialize(ref playerLoop);
        }


#if UNITY_EDITOR

        [InitializeOnLoadMethod]
        static void InitOnEditor()
        {
            // Execute the play mode init method
            Init();

            // register an Editor update delegate, used to forcing playerLoop update
            EditorApplication.update += ForceEditorPlayerLoopUpdate;
        }

        private static void ForceEditorPlayerLoopUpdate()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isCompiling || EditorApplication.isUpdating)
            {
                // Not in Edit mode, don't interfere
                return;
            }

            // EditorApplication.QueuePlayerLoopUpdate causes performance issue, don't call directly.
            // EditorApplication.QueuePlayerLoopUpdate();

            updateYielder?.Run();
            updateRunner?.Run();

            UniTaskSynchronizationContext.Run();
        }

#endif

        static void Initialize(ref PlayerLoopSystem playerLoop)
        {
            // Set yielders and runners.
            var cq = new ContinuationQueue(PlayerLoopTiming.Update);
            var runner = new PlayerLoopRunner(PlayerLoopTiming.Update);
            updateYielder = cq;
            updateRunner = runner;

            // Create loops.
            var yieldLoop = new PlayerLoopSystem { type = typeof(ContinuationQueue), updateDelegate = cq.Run };
            var runnerLoop = new PlayerLoopSystem { type = typeof(PlayerLoopRunner), updateDelegate = runner.Run };
            var syncContextLoop = new PlayerLoopSystem { type = typeof(UniTaskSynchronizationContext), updateDelegate = UniTaskSynchronizationContext.Run };

            // Find update loop.
            var subSystemList = playerLoop.subSystemList;
            var updateSystemIndex = FindIndex(subSystemList, typeof(PlayerLoopType.Update));
            Assert.AreNotEqual(-1, updateSystemIndex, "Update Loop not found.");
            ref var updateSystem = ref subSystemList[updateSystemIndex];

            // Insert loop into subSystem.
            var orgLoops = updateSystem.subSystemList;
            var orgYielderIndex = FindIndex(orgLoops, typeof(ContinuationQueue));
            if (orgYielderIndex != -1) // When already initialized, replace it.
            {
                var orgRunnerIndex = FindIndex(orgLoops, typeof(PlayerLoopRunner));
                var orgSyncContextIndex = FindIndex(orgLoops, typeof(UniTaskSynchronizationContext));
                Assert.AreNotEqual(-1, orgRunnerIndex, "PlayerLoopRunner not found.");
                Assert.AreNotEqual(-1, orgSyncContextIndex, "UniTaskSynchronizationContext not found.");
                orgLoops[orgYielderIndex] = yieldLoop;
                orgLoops[orgRunnerIndex] = runnerLoop;
                orgLoops[orgSyncContextIndex] = syncContextLoop;
            }
            else // When not initialized, insert it.
            {
                Assert.IsFalse(FindIndex(orgLoops, typeof(ContinuationQueue)) != -1, "ContinuationQueue already exists.");
                Assert.IsFalse(FindIndex(orgLoops, typeof(PlayerLoopRunner)) != -1, "PlayerLoopRunner already exists.");
                Assert.IsFalse(FindIndex(orgLoops, typeof(UniTaskSynchronizationContext)) != -1, "UniTaskSynchronizationContext already exists.");

                var orgCount = orgLoops.Length;
                var newLoops = new PlayerLoopSystem[orgCount + 3];
                newLoops[0] = new PlayerLoopSystem { type = typeof(ContinuationQueue), updateDelegate = cq.Run };
                newLoops[1] = new PlayerLoopSystem { type = typeof(PlayerLoopRunner), updateDelegate = runner.Run };

                // Insert UniTaskSynchronizationContext to Update loop
                var i = 0;
                var j = 2;
                for (; i < orgCount; i++)
                {
                    if (orgLoops[i].type == typeof(PlayerLoopType.Update.ScriptRunDelayedTasks))
                        newLoops[j++] = new PlayerLoopSystem { type = typeof(UniTaskSynchronizationContext), updateDelegate = UniTaskSynchronizationContext.Run };
                    newLoops[j++] = orgLoops[i];
                }
                Assert.AreEqual(orgCount + 3, j);

                // Update subSystem.
                updateSystem.subSystemList = newLoops;
            }

            // Set new subSystem.
            Assert.IsTrue(playerLoop.subSystemList[updateSystemIndex].subSystemList
                .Any(x => x.type == typeof(ContinuationQueue)), "ContinuationQueue not found.");
            Assert.IsTrue(playerLoop.subSystemList[updateSystemIndex].subSystemList
                .Any(x => x.type == typeof(PlayerLoopRunner)), "PlayerLoopRunner not found.");
            Assert.IsTrue(playerLoop.subSystemList[updateSystemIndex].subSystemList
                .Any(x => x.type == typeof(UniTaskSynchronizationContext)), "UniTaskSynchronizationContext not found.");
            PlayerLoop.SetPlayerLoop(playerLoop);
            return;

            static int FindIndex(PlayerLoopSystem[] systems, Type type)
            {
                for (var i = 0; i < systems.Length; i++)
                {
                    if (systems[i].type == type)
                        return i;
                }
                return -1;
            }
        }

        public static void AddAction(PlayerLoopTiming timing, IPlayerLoopItem action)
        {
            Assert.AreEqual(PlayerLoopTiming.Update, timing, "Only Update timing is supported.");
            Assert.IsNotNull(updateRunner, "UniTask.PlayerLoopHelper is not initialized.");
            updateRunner.AddAction(action);
        }

        public static void AddContinuation(PlayerLoopTiming timing, Action continuation)
        {
            Assert.AreEqual(PlayerLoopTiming.Update, timing, "Only Update timing is supported.");
            Assert.IsNotNull(updateYielder, "UniTask.PlayerLoopHelper is not initialized.");
            updateYielder.Enqueue(continuation);
        }
    }
}

