#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using UnityEngine;
using Cysharp.Threading.Tasks.Internal;
using System.Threading;

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
    public static class UniTaskLoopRunners
    {
        public struct UniTaskLoopRunnerUpdate { };

        // Last
        public struct UniTaskLoopRunnerLastPostLateUpdate { };

        // Yield
        public struct UniTaskLoopRunnerYieldUpdate { };

        // Yield Last
        public struct UniTaskLoopRunnerLastYieldPostLateUpdate { };
    }

    public enum PlayerLoopTiming
    {
        Update = 0,
        LastPostLateUpdate = 1,
    }

    [Flags]
    public enum InjectPlayerLoopTimings
    {
        /// <summary>
        /// Preset: All loops(default).
        /// </summary>
        All =
            Update |
            LastPostLateUpdate
            ,

        Update = 0,
        LastPostLateUpdate = 1
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
        static ContinuationQueue[] yielders;
        static PlayerLoopRunner[] runners;
        internal static bool IsEditorApplicationQuitting { get; private set; }
        static void InsertRunner(PlayerLoopBuilder loopSystem,
            bool injectOnFirst,
            Type loopRunnerYieldType, ContinuationQueue cq,
            Type loopRunnerType, PlayerLoopRunner runner)
        {

#if UNITY_EDITOR
            EditorApplication.playModeStateChanged += (state) =>
            {
                if (state == PlayModeStateChange.EnteredEditMode || state == PlayModeStateChange.ExitingEditMode)
                {
                    IsEditorApplicationQuitting = true;
                    // run rest action before clear.
                    if (runner != null)
                    {
                        runner.Run();
                        runner.Clear();
                    }
                    if (cq != null)
                    {
                        cq.Run();
                        cq.Clear();
                    }
                    IsEditorApplicationQuitting = false;
                }
            };
#endif

            var yieldLoop = new PlayerLoopSystem
            {
                type = loopRunnerYieldType,
                updateDelegate = cq.Run
            };

            var runnerLoop = new PlayerLoopSystem
            {
                type = loopRunnerType,
                updateDelegate = runner.Run
            };

            // Remove items from previous initializations.
            loopSystem.Remove(loopRunnerYieldType, loopRunnerType);

            if (injectOnFirst)
            {
                loopSystem.InsertFront(yieldLoop, runnerLoop);
            }
            else
            {
                loopSystem.Add(yieldLoop, runnerLoop);
            }
        }

        static void RemoveRunner(PlayerLoopBuilder loopSystem, Type loopRunnerYieldType, Type loopRunnerType)
        {
            loopSystem.Remove(loopRunnerYieldType, loopRunnerType);
        }

        static void InsertUniTaskSynchronizationContext(PlayerLoopBuilder loopSystem)
        {
            var loop = new PlayerLoopSystem
            {
                type = typeof(UniTaskSynchronizationContext),
                updateDelegate = UniTaskSynchronizationContext.Run
            };

            // Remove items from previous initializations.
            loopSystem.Remove(typeof(UniTaskSynchronizationContext));

            var index = loopSystem.IndexOf(typeof(PlayerLoopType.Update.ScriptRunDelayedTasks));
            if (index == -1)
            {
                index = loopSystem.IndexOf(typeof(UniTaskLoopRunners.UniTaskLoopRunnerUpdate));
            }

            loopSystem.Insert(index + 1, loop);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
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
            if (!domainReloadDisabled && runners != null) return;
#else
            if (runners != null) return; // already initialized
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

            if (yielders != null)
            {
                foreach (var item in yielders)
                {
                    if (item != null) item.Run();
                }
            }

            if (runners != null)
            {
                foreach (var item in runners)
                {
                    if (item != null) item.Run();
                }
            }

            UniTaskSynchronizationContext.Run();
        }

#endif

        private static int FindLoopSystemIndex(PlayerLoopBuilder[] playerLoopList, Type systemType)
        {
            for (int i = 0; i < playerLoopList.Length; i++)
            {
                if (playerLoopList[i].type == systemType)
                {
                    return i;
                }
            }

            throw new Exception("Target PlayerLoopSystem does not found. Type:" + systemType.FullName);
        }

        static void InsertLoop(PlayerLoopBuilder[] copyList, InjectPlayerLoopTimings injectTimings, Type loopType, InjectPlayerLoopTimings targetTimings,
            int index, bool injectOnFirst, Type loopRunnerYieldType, Type loopRunnerType, PlayerLoopTiming playerLoopTiming)
        {
            var i = FindLoopSystemIndex(copyList, loopType);
            if ((injectTimings & targetTimings) == targetTimings)
            {
                InsertRunner(copyList[i], injectOnFirst,
                    loopRunnerYieldType, yielders[index] = new ContinuationQueue(playerLoopTiming),
                    loopRunnerType, runners[index] = new PlayerLoopRunner(playerLoopTiming));
            }
            else
            {
                RemoveRunner(copyList[i], loopRunnerYieldType, loopRunnerType);
            }
        }

        public static void Initialize(ref PlayerLoopSystem playerLoop, InjectPlayerLoopTimings injectTimings = InjectPlayerLoopTimings.All)
        {
            yielders = new ContinuationQueue[2];
            runners = new PlayerLoopRunner[2];

            var copyList = PlayerLoopBuilder.CreateSubSystemArray(playerLoop);

            // Update
            InsertLoop(copyList, injectTimings, typeof(PlayerLoopType.Update),
                InjectPlayerLoopTimings.Update, 0, true,
                typeof(UniTaskLoopRunners.UniTaskLoopRunnerYieldUpdate), typeof(UniTaskLoopRunners.UniTaskLoopRunnerUpdate), PlayerLoopTiming.Update);

            InsertLoop(copyList, injectTimings, typeof(PlayerLoopType.PostLateUpdate),
                InjectPlayerLoopTimings.LastPostLateUpdate, 1, false,
                typeof(UniTaskLoopRunners.UniTaskLoopRunnerLastYieldPostLateUpdate), typeof(UniTaskLoopRunners.UniTaskLoopRunnerLastPostLateUpdate), PlayerLoopTiming.LastPostLateUpdate);

            // Insert UniTaskSynchronizationContext to Update loop
            var i = FindLoopSystemIndex(copyList, typeof(PlayerLoopType.Update));
            InsertUniTaskSynchronizationContext(copyList[i]);

            playerLoop.subSystemList = PlayerLoopBuilder.BuildSubSystemArray(copyList);
            PlayerLoop.SetPlayerLoop(playerLoop);
        }

        public static void AddAction(PlayerLoopTiming timing, IPlayerLoopItem action)
        {
            var runner = runners[(int)timing];
            if (runner == null)
            {
                ThrowInvalidLoopTiming(timing);
            }
            runner.AddAction(action);
        }

        static void ThrowInvalidLoopTiming(PlayerLoopTiming playerLoopTiming)
        {
            throw new InvalidOperationException("Target playerLoopTiming is not injected. Please check PlayerLoopHelper.Initialize. PlayerLoopTiming:" + playerLoopTiming);
        }

        public static void AddContinuation(PlayerLoopTiming timing, Action continuation)
        {
            var q = yielders[(int)timing];
            if (q == null)
            {
                ThrowInvalidLoopTiming(timing);
            }
            q.Enqueue(continuation);
        }

    }
}

