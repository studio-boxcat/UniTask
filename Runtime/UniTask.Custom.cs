#nullable enable

using System;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

namespace Cysharp.Threading.Tasks
{
    public partial struct UniTask
    {
        public static UniTask DelaySec(float delay, bool ignoreTimeScale = false, CancellationToken cancellationToken = default(CancellationToken))
        {
            var delayType = ignoreTimeScale ? DelayType.UnscaledDeltaTime : DelayType.DeltaTime;
            return Delay(TimeSpan.FromSeconds(delay), delayType, cancellationToken);
        }

#if UNITY_EDITOR
        public static void EditorLoop(Action<float> action, GameObject scope)
        {
            var sw = Stopwatch.StartNew();
            WaitWhile(() =>
            {
                var dt = sw.Elapsed;
                action(dt.TotalSeconds.F32());
                sw.Restart();
                return scope;
            }).Forget();
        }
#endif
    }
}