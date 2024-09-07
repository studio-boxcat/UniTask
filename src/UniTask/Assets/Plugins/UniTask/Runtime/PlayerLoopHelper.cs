#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Linq;
using UnityEngine;
using Cysharp.Threading.Tasks.Internal;
using System.Threading;
using UnityEngine.Assertions;
using UnityEngine.LowLevel;
using PlayerLoopType = UnityEngine.PlayerLoop;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Cysharp.Threading.Tasks
{
    public interface IPlayerLoopItem
    {
        bool MoveNext();
    }

    public static class PlayerLoopHelper
    {
        public static int MainThreadId => mainThreadId;
        internal static string ApplicationDataPath => applicationDataPath;

        public static bool IsMainThread => Thread.CurrentThread.ManagedThreadId == mainThreadId;

        static int mainThreadId;
        static string applicationDataPath;
        static ContinuationQueue updateYielder;
        static PlayerLoopRunner updateRunner;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        static void Init()
        {
            L.I("[UniTask] PlayerLoopHelper.Init()");

            // capture default(unity) sync-context.
            mainThreadId = Thread.CurrentThread.ManagedThreadId;
            try
            {
                applicationDataPath = Application.dataPath;
            }
            catch { }

#if UNITY_EDITOR
            // When domain reload is disabled, re-initialization is required when entering play mode; 
            // otherwise, pending tasks will leak between play mode sessions.
            var domainReloadDisabled = EditorSettings.enterPlayModeOptionsEnabled &&
                                       EditorSettings.enterPlayModeOptions.HasFlag(EnterPlayModeOptions.DisableDomainReload);
            if (!domainReloadDisabled && updateRunner != null) return;
#else
            if (updateRunner != null) return; // already initialized
#endif

            var playerLoop = PlayerLoop.GetCurrentPlayerLoop();
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
        }
#endif

        static void Initialize(ref PlayerLoopSystem playerLoop)
        {
            // Set yielders and runners.
            var cq = new ContinuationQueue();
            var runner = new PlayerLoopRunner();
            updateYielder = cq;
            updateRunner = runner;

            // Create loops.
            var yieldLoop = new PlayerLoopSystem { type = typeof(ContinuationQueue), updateDelegate = cq.Run };
            var runnerLoop = new PlayerLoopSystem { type = typeof(PlayerLoopRunner), updateDelegate = runner.Run };

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
                Assert.AreNotEqual(-1, orgRunnerIndex, "PlayerLoopRunner not found.");
                orgLoops[orgYielderIndex] = yieldLoop;
                orgLoops[orgRunnerIndex] = runnerLoop;
            }
            else // When not initialized, insert it.
            {
                Assert.IsFalse(FindIndex(orgLoops, typeof(ContinuationQueue)) != -1, "ContinuationQueue already exists.");
                Assert.IsFalse(FindIndex(orgLoops, typeof(PlayerLoopRunner)) != -1, "PlayerLoopRunner already exists.");

                var orgCount = orgLoops.Length;
                var newLoops = new PlayerLoopSystem[orgCount + 2];
                newLoops[0] = new PlayerLoopSystem { type = typeof(ContinuationQueue), updateDelegate = cq.Run };
                newLoops[1] = new PlayerLoopSystem { type = typeof(PlayerLoopRunner), updateDelegate = runner.Run };
                Array.Copy(orgLoops, 0, newLoops, 2, orgCount);

                // Update subSystem.
                updateSystem.subSystemList = newLoops;
            }

            // Set new subSystem.
            Assert.IsTrue(playerLoop.subSystemList[updateSystemIndex].subSystemList
                .Any(x => x.type == typeof(ContinuationQueue)), "ContinuationQueue not found.");
            Assert.IsTrue(playerLoop.subSystemList[updateSystemIndex].subSystemList
                .Any(x => x.type == typeof(PlayerLoopRunner)), "PlayerLoopRunner not found.");
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

        public static void AddAction(IPlayerLoopItem action)
        {
            Assert.IsNotNull(updateRunner, "UniTask.PlayerLoopHelper is not initialized.");
            updateRunner.AddAction(action);
        }

        public static void AddContinuation(Action continuation)
        {
            Assert.IsNotNull(updateYielder, "UniTask.PlayerLoopHelper is not initialized.");
            updateYielder.Enqueue(continuation);
        }
    }
}