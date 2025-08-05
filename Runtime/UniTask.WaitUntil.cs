#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Threading;
using UnityEngine.Assertions;

namespace Cysharp.Threading.Tasks
{
    public partial struct UniTask
    {
        public static UniTask WaitUntil(Func<bool> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (cancellationToken.IsCancellationRequested) return FromCanceled(cancellationToken);
            if (predicate()) return CompletedTask; // don't wait if the predicate is already true.
            return new UniTask(WaitUntilPromise.Create(predicate, cancellationToken, out var token), token);
        }

        public static UniTask WaitWhile(Func<bool> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (cancellationToken.IsCancellationRequested) return FromCanceled(cancellationToken);
            if (predicate() is false) return CompletedTask; // don't wait if the predicate is already false.
            return new UniTask(WaitWhilePromise.Create(predicate, cancellationToken, out var token), token);
        }

        sealed class WaitUntilPromise : IUniTaskSource, IPlayerLoopItem, ITaskPoolNode<WaitUntilPromise>
        {
            static TaskPool<WaitUntilPromise> pool;
            WaitUntilPromise nextNode;
            public ref WaitUntilPromise NextNode => ref nextNode;

            static WaitUntilPromise()
            {
                TaskPool.RegisterSizeGetter(typeof(WaitUntilPromise), () => pool.Size);
            }

            Func<bool> predicate;
            CancellationToken cancellationToken;

            UniTaskCompletionSourceCore<object> core;

            WaitUntilPromise()
            {
            }

            public static IUniTaskSource Create(Func<bool> predicate, CancellationToken cancellationToken, out short token)
            {
                Assert.IsFalse(cancellationToken.IsCancellationRequested,
                    "cancellationToken.IsCancellationRequested must be checked before calling WaitUntilPromise.Create.");

                if (!pool.TryPop(out var result))
                {
                    result = new WaitUntilPromise();
                }

                result.predicate = predicate;
                result.cancellationToken = cancellationToken;

                TaskTracker.TrackActiveTask(result, 3);

                PlayerLoopHelper.AddAction(result);

                token = result.core.Version;
                return result;
            }

            public void GetResult(short token)
            {
                try
                {
                    core.GetResult(token);
                }
                finally
                {
                    TryReturn();
                }
            }

            public UniTaskStatus GetStatus(short token)
            {
                return core.GetStatus(token);
            }

            public UniTaskStatus UnsafeGetStatus()
            {
                return core.UnsafeGetStatus();
            }

            public void OnCompleted(Action<object> continuation, object state, short token)
            {
                core.OnCompleted(continuation, state, token);
            }

            public bool MoveNext()
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    core.TrySetCanceled(cancellationToken);
                    return false;
                }

                try
                {
                    if (!predicate())
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    core.TrySetException(ex);
                    return false;
                }

                core.TrySetResult(null);
                return false;
            }

            bool TryReturn()
            {
                TaskTracker.RemoveTracking(this);
                core.Reset();
                predicate = default;
                cancellationToken = default;
                return pool.TryPush(this);
            }
        }

        sealed class WaitWhilePromise : IUniTaskSource, IPlayerLoopItem, ITaskPoolNode<WaitWhilePromise>
        {
            static TaskPool<WaitWhilePromise> pool;
            WaitWhilePromise nextNode;
            public ref WaitWhilePromise NextNode => ref nextNode;

            static WaitWhilePromise()
            {
                TaskPool.RegisterSizeGetter(typeof(WaitWhilePromise), () => pool.Size);
            }

            Func<bool> predicate;
            CancellationToken cancellationToken;

            UniTaskCompletionSourceCore<object> core;

            WaitWhilePromise()
            {
            }

            public static IUniTaskSource Create(Func<bool> predicate, CancellationToken cancellationToken, out short token)
            {
                Assert.IsFalse(cancellationToken.IsCancellationRequested,
                    "cancellationToken.IsCancellationRequested must be checked before calling WaitWhilePromise.Create.");

                if (!pool.TryPop(out var result))
                {
                    result = new WaitWhilePromise();
                }

                result.predicate = predicate;
                result.cancellationToken = cancellationToken;

                TaskTracker.TrackActiveTask(result, 3);

                PlayerLoopHelper.AddAction(result);

                token = result.core.Version;
                return result;
            }

            public void GetResult(short token)
            {
                try
                {
                    core.GetResult(token);
                }
                finally
                {
                    TryReturn();
                }
            }

            public UniTaskStatus GetStatus(short token)
            {
                return core.GetStatus(token);
            }

            public UniTaskStatus UnsafeGetStatus()
            {
                return core.UnsafeGetStatus();
            }

            public void OnCompleted(Action<object> continuation, object state, short token)
            {
                core.OnCompleted(continuation, state, token);
            }

            public bool MoveNext()
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    core.TrySetCanceled(cancellationToken);
                    return false;
                }

                try
                {
                    if (predicate())
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    core.TrySetException(ex);
                    return false;
                }

                core.TrySetResult(null);
                return false;
            }

            bool TryReturn()
            {
                TaskTracker.RemoveTracking(this);
                core.Reset();
                predicate = default;
                cancellationToken = default;
                return pool.TryPush(this);
            }
        }
    }
}
