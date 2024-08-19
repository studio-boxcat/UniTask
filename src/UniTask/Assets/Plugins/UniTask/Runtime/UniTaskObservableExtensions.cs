#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

using System;
using System.Threading;
using Cysharp.Threading.Tasks.Internal;

namespace Cysharp.Threading.Tasks
{
    public static class UniTaskObservableExtensions
    {
        public static UniTask<T> ToUniTask<T>(this IObservable<T> source, bool useFirstValue = false, CancellationToken cancellationToken = default)
        {
            var promise = new UniTaskCompletionSource<T>();
            var disposable = new SingleAssignmentDisposable();

            var observer = useFirstValue
                ? (IObserver<T>)new FirstValueToUniTaskObserver<T>(promise, disposable, cancellationToken)
                : (IObserver<T>)new ToUniTaskObserver<T>(promise, disposable, cancellationToken);

            try
            {
                disposable.Disposable = source.Subscribe(observer);
            }
            catch (Exception ex)
            {
                promise.TrySetException(ex);
            }

            return promise.Task;
        }

        class ToUniTaskObserver<T> : IObserver<T>
        {
            static readonly Action<object> callback = OnCanceled;

            readonly UniTaskCompletionSource<T> promise;
            readonly SingleAssignmentDisposable disposable;
            readonly CancellationToken cancellationToken;
            readonly CancellationTokenRegistration registration;

            bool hasValue;
            T latestValue;

            public ToUniTaskObserver(UniTaskCompletionSource<T> promise, SingleAssignmentDisposable disposable, CancellationToken cancellationToken)
            {
                this.promise = promise;
                this.disposable = disposable;
                this.cancellationToken = cancellationToken;

                if (this.cancellationToken.CanBeCanceled)
                {
                    this.registration = this.cancellationToken.RegisterWithoutCaptureExecutionContext(callback, this);
                }
            }

            static void OnCanceled(object state)
            {
                var self = (ToUniTaskObserver<T>)state;
                self.disposable.Dispose();
                self.promise.TrySetCanceled(self.cancellationToken);
            }

            public void OnNext(T value)
            {
                hasValue = true;
                latestValue = value;
            }

            public void OnError(Exception error)
            {
                try
                {
                    promise.TrySetException(error);
                }
                finally
                {
                    registration.Dispose();
                    disposable.Dispose();
                }
            }

            public void OnCompleted()
            {
                try
                {
                    if (hasValue)
                    {
                        promise.TrySetResult(latestValue);
                    }
                    else
                    {
                        promise.TrySetException(new InvalidOperationException("Sequence has no elements"));
                    }
                }
                finally
                {
                    registration.Dispose();
                    disposable.Dispose();
                }
            }
        }

        class FirstValueToUniTaskObserver<T> : IObserver<T>
        {
            static readonly Action<object> callback = OnCanceled;

            readonly UniTaskCompletionSource<T> promise;
            readonly SingleAssignmentDisposable disposable;
            readonly CancellationToken cancellationToken;
            readonly CancellationTokenRegistration registration;

            bool hasValue;

            public FirstValueToUniTaskObserver(UniTaskCompletionSource<T> promise, SingleAssignmentDisposable disposable, CancellationToken cancellationToken)
            {
                this.promise = promise;
                this.disposable = disposable;
                this.cancellationToken = cancellationToken;

                if (this.cancellationToken.CanBeCanceled)
                {
                    this.registration = this.cancellationToken.RegisterWithoutCaptureExecutionContext(callback, this);
                }
            }

            static void OnCanceled(object state)
            {
                var self = (FirstValueToUniTaskObserver<T>)state;
                self.disposable.Dispose();
                self.promise.TrySetCanceled(self.cancellationToken);
            }

            public void OnNext(T value)
            {
                hasValue = true;
                try
                {
                    promise.TrySetResult(value);
                }
                finally
                {
                    registration.Dispose();
                    disposable.Dispose();
                }
            }

            public void OnError(Exception error)
            {
                try
                {
                    promise.TrySetException(error);
                }
                finally
                {
                    registration.Dispose();
                    disposable.Dispose();
                }
            }

            public void OnCompleted()
            {
                try
                {
                    if (!hasValue)
                    {
                        promise.TrySetException(new InvalidOperationException("Sequence has no elements"));
                    }
                }
                finally
                {
                    registration.Dispose();
                    disposable.Dispose();
                }
            }
        }
    }
}

namespace Cysharp.Threading.Tasks.Internal
{
    // Bridges for Rx.

    internal sealed class SingleAssignmentDisposable : IDisposable
    {
        readonly object gate = new object();
        IDisposable current;
        bool disposed;

        public bool IsDisposed { get { lock (gate) { return disposed; } } }

        public IDisposable Disposable
        {
            get
            {
                return current;
            }
            set
            {
                var old = default(IDisposable);
                bool alreadyDisposed;
                lock (gate)
                {
                    alreadyDisposed = disposed;
                    old = current;
                    if (!alreadyDisposed)
                    {
                        if (value == null) return;
                        current = value;
                    }
                }

                if (alreadyDisposed && value != null)
                {
                    value.Dispose();
                    return;
                }

                if (old != null) throw new InvalidOperationException("Disposable is already set");
            }
        }


        public void Dispose()
        {
            IDisposable old = null;

            lock (gate)
            {
                if (!disposed)
                {
                    disposed = true;
                    old = current;
                    current = null;
                }
            }

            if (old != null) old.Dispose();
        }
    }

}

