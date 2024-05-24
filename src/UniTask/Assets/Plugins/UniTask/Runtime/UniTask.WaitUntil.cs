#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Threading;

namespace Cysharp.Threading.Tasks
{
    public partial struct UniTask
    {
        public static UniTask WaitUntil(Func<bool> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return new UniTask(WaitUntilPromise.Create(predicate, cancellationToken, out var token), token);
        }

        public static UniTask WaitWhile(Func<bool> predicate, CancellationToken cancellationToken = default(CancellationToken))
        {
            return new UniTask(WaitWhilePromise.Create(predicate, cancellationToken, out var token), token);
        }

        public static UniTask WaitUntilCanceled(CancellationToken cancellationToken)
        {
            return new UniTask(WaitUntilCanceledPromise.Create(cancellationToken, out var token), token);
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
                if (cancellationToken.IsCancellationRequested)
                {
                    return AutoResetUniTaskCompletionSource.CreateFromCanceled(cancellationToken, out token);
                }

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
                if (cancellationToken.IsCancellationRequested)
                {
                    return AutoResetUniTaskCompletionSource.CreateFromCanceled(cancellationToken, out token);
                }

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

        sealed class WaitUntilCanceledPromise : IUniTaskSource, IPlayerLoopItem, ITaskPoolNode<WaitUntilCanceledPromise>
        {
            static TaskPool<WaitUntilCanceledPromise> pool;
            WaitUntilCanceledPromise nextNode;
            public ref WaitUntilCanceledPromise NextNode => ref nextNode;

            static WaitUntilCanceledPromise()
            {
                TaskPool.RegisterSizeGetter(typeof(WaitUntilCanceledPromise), () => pool.Size);
            }

            CancellationToken cancellationToken;

            UniTaskCompletionSourceCore<object> core;

            WaitUntilCanceledPromise()
            {
            }

            public static IUniTaskSource Create(CancellationToken cancellationToken, out short token)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return AutoResetUniTaskCompletionSource.CreateFromCanceled(cancellationToken, out token);
                }

                if (!pool.TryPop(out var result))
                {
                    result = new WaitUntilCanceledPromise();
                }

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
                    core.TrySetResult(null);
                    return false;
                }

                return true;
            }

            bool TryReturn()
            {
                TaskTracker.RemoveTracking(this);
                core.Reset();
                cancellationToken = default;
                return pool.TryPush(this);
            }
        }
    }
}
