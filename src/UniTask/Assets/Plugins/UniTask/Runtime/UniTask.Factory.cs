#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Runtime.ExceptionServices;
using System.Threading;

namespace Cysharp.Threading.Tasks
{
    public partial struct UniTask
    {
        static readonly UniTask CanceledUniTask = new Func<UniTask>(() =>
        {
            return new UniTask(new CanceledResultSource(CancellationToken.None), 0);
        })();

        static class CanceledUniTaskCache<T>
        {
            public static readonly UniTask<T> Task;

            static CanceledUniTaskCache()
            {
                Task = new UniTask<T>(new CanceledResultSource<T>(CancellationToken.None), 0);
            }
        }

        public static readonly UniTask CompletedTask = new UniTask();

        public static UniTask FromException(Exception ex)
        {
            if (ex is OperationCanceledException oce)
            {
                return FromCanceled(oce.CancellationToken);
            }

            return new UniTask(new ExceptionResultSource(ex), 0);
        }

        public static UniTask<T> FromException<T>(Exception ex)
        {
            if (ex is OperationCanceledException oce)
            {
                return FromCanceled<T>(oce.CancellationToken);
            }

            return new UniTask<T>(new ExceptionResultSource<T>(ex), 0);
        }

        public static UniTask<T> FromResult<T>(T value)
        {
            return new UniTask<T>(value);
        }

        public static UniTask FromCanceled(CancellationToken cancellationToken = default)
        {
            if (cancellationToken == CancellationToken.None)
            {
                return CanceledUniTask;
            }
            else
            {
                return new UniTask(new CanceledResultSource(cancellationToken), 0);
            }
        }

        public static UniTask<T> FromCanceled<T>(CancellationToken cancellationToken = default)
        {
            if (cancellationToken == CancellationToken.None)
            {
                return CanceledUniTaskCache<T>.Task;
            }
            else
            {
                return new UniTask<T>(new CanceledResultSource<T>(cancellationToken), 0);
            }
        }

        sealed class ExceptionResultSource : IUniTaskSource
        {
            readonly ExceptionDispatchInfo exception;
            bool calledGet;

            public ExceptionResultSource(Exception exception)
            {
                this.exception = ExceptionDispatchInfo.Capture(exception);
            }

            public void GetResult(short token)
            {
                if (!calledGet)
                {
                    calledGet = true;
                    GC.SuppressFinalize(this);
                }
                exception.Throw();
            }

            public UniTaskStatus GetStatus(short token)
            {
                return UniTaskStatus.Faulted;
            }

            public UniTaskStatus UnsafeGetStatus()
            {
                return UniTaskStatus.Faulted;
            }

            public void OnCompleted(Action<object> continuation, object state, short token)
            {
                continuation(state);
            }

            ~ExceptionResultSource()
            {
                if (!calledGet)
                {
                    L.E(exception.SourceException);
                }
            }
        }

        sealed class ExceptionResultSource<T> : IUniTaskSource<T>
        {
            readonly ExceptionDispatchInfo exception;
            bool calledGet;

            public ExceptionResultSource(Exception exception)
            {
                this.exception = ExceptionDispatchInfo.Capture(exception);
            }

            public T GetResult(short token)
            {
                if (!calledGet)
                {
                    calledGet = true;
                    GC.SuppressFinalize(this);
                }
                exception.Throw();
                return default;
            }

            void IUniTaskSource.GetResult(short token)
            {
                if (!calledGet)
                {
                    calledGet = true;
                    GC.SuppressFinalize(this);
                }
                exception.Throw();
            }

            public UniTaskStatus GetStatus(short token)
            {
                return UniTaskStatus.Faulted;
            }

            public UniTaskStatus UnsafeGetStatus()
            {
                return UniTaskStatus.Faulted;
            }

            public void OnCompleted(Action<object> continuation, object state, short token)
            {
                continuation(state);
            }

            ~ExceptionResultSource()
            {
                if (!calledGet)
                {
                    L.E(exception.SourceException);
                }
            }
        }

        sealed class CanceledResultSource : IUniTaskSource
        {
            readonly CancellationToken cancellationToken;

            public CanceledResultSource(CancellationToken cancellationToken)
            {
                this.cancellationToken = cancellationToken;
            }

            public void GetResult(short token)
            {
                throw new OperationCanceledException(cancellationToken);
            }

            public UniTaskStatus GetStatus(short token)
            {
                return UniTaskStatus.Canceled;
            }

            public UniTaskStatus UnsafeGetStatus()
            {
                return UniTaskStatus.Canceled;
            }

            public void OnCompleted(Action<object> continuation, object state, short token)
            {
                continuation(state);
            }
        }

        sealed class CanceledResultSource<T> : IUniTaskSource<T>
        {
            readonly CancellationToken cancellationToken;

            public CanceledResultSource(CancellationToken cancellationToken)
            {
                this.cancellationToken = cancellationToken;
            }

            public T GetResult(short token)
            {
                throw new OperationCanceledException(cancellationToken);
            }

            void IUniTaskSource.GetResult(short token)
            {
                throw new OperationCanceledException(cancellationToken);
            }

            public UniTaskStatus GetStatus(short token)
            {
                return UniTaskStatus.Canceled;
            }

            public UniTaskStatus UnsafeGetStatus()
            {
                return UniTaskStatus.Canceled;
            }

            public void OnCompleted(Action<object> continuation, object state, short token)
            {
                continuation(state);
            }
        }
    }

    internal static class CompletedTasks
    {
        public static readonly UniTask<AsyncUnit> AsyncUnit = UniTask.FromResult(Cysharp.Threading.Tasks.AsyncUnit.Default);
        public static readonly UniTask<bool> True = UniTask.FromResult(true);
        public static readonly UniTask<bool> False = UniTask.FromResult(false);
    }
}
