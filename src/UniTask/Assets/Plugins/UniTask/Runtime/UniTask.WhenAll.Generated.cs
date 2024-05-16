#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
using System;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Threading;
using Cysharp.Threading.Tasks.Internal;

namespace Cysharp.Threading.Tasks
{
    public partial struct UniTask
    {
        
        public static UniTask<(T1, T2)> WhenAll<T1, T2>(UniTask<T1> task1, UniTask<T2> task2)
        {
            if (task1.Status.IsCompletedSuccessfully() && task2.Status.IsCompletedSuccessfully())
            {
                return new UniTask<(T1, T2)>((task1.GetAwaiter().GetResult(), task2.GetAwaiter().GetResult()));
            }

            return new UniTask<(T1, T2)>(new WhenAllPromise<T1, T2>(task1, task2), 0);
        }

        sealed class WhenAllPromise<T1, T2> : IUniTaskSource<(T1, T2)>
        {
            T1 t1 = default;
            T2 t2 = default;
            int completedCount;
            UniTaskCompletionSourceCore<(T1, T2)> core;

            public WhenAllPromise(UniTask<T1> task1, UniTask<T2> task2)
            {
                TaskTracker.TrackActiveTask(this, 3);

                this.completedCount = 0;
                {
                    var awaiter = task1.GetAwaiter();
                    if (awaiter.IsCompleted)
                    {
                        TryInvokeContinuationT1(this, awaiter);
                    }
                    else
                    {
                        awaiter.SourceOnCompleted(state =>
                        {
                            using (var t = (StateTuple<WhenAllPromise<T1, T2>, UniTask<T1>.Awaiter>)state)
                            {
                                TryInvokeContinuationT1(t.Item1, t.Item2);
                            }
                        }, StateTuple.Create(this, awaiter));
                    }
                }
                {
                    var awaiter = task2.GetAwaiter();
                    if (awaiter.IsCompleted)
                    {
                        TryInvokeContinuationT2(this, awaiter);
                    }
                    else
                    {
                        awaiter.SourceOnCompleted(state =>
                        {
                            using (var t = (StateTuple<WhenAllPromise<T1, T2>, UniTask<T2>.Awaiter>)state)
                            {
                                TryInvokeContinuationT2(t.Item1, t.Item2);
                            }
                        }, StateTuple.Create(this, awaiter));
                    }
                }
            }

            static void TryInvokeContinuationT1(WhenAllPromise<T1, T2> self, in UniTask<T1>.Awaiter awaiter)
            {
                try
                {
                    self.t1 = awaiter.GetResult();
                }
                catch (Exception ex)
                {
                    self.core.TrySetException(ex);
                    return;
                }
                
                if (Interlocked.Increment(ref self.completedCount) == 2)
                {
                    self.core.TrySetResult((self.t1, self.t2));
                }
            }

            static void TryInvokeContinuationT2(WhenAllPromise<T1, T2> self, in UniTask<T2>.Awaiter awaiter)
            {
                try
                {
                    self.t2 = awaiter.GetResult();
                }
                catch (Exception ex)
                {
                    self.core.TrySetException(ex);
                    return;
                }
                
                if (Interlocked.Increment(ref self.completedCount) == 2)
                {
                    self.core.TrySetResult((self.t1, self.t2));
                }
            }


            public (T1, T2) GetResult(short token)
            {
                TaskTracker.RemoveTracking(this);
                GC.SuppressFinalize(this);
                return core.GetResult(token);
            }

            void IUniTaskSource.GetResult(short token)
            {
                GetResult(token);
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
        }
        
        public static UniTask<(T1, T2, T3)> WhenAll<T1, T2, T3>(UniTask<T1> task1, UniTask<T2> task2, UniTask<T3> task3)
        {
            if (task1.Status.IsCompletedSuccessfully() && task2.Status.IsCompletedSuccessfully() && task3.Status.IsCompletedSuccessfully())
            {
                return new UniTask<(T1, T2, T3)>((task1.GetAwaiter().GetResult(), task2.GetAwaiter().GetResult(), task3.GetAwaiter().GetResult()));
            }

            return new UniTask<(T1, T2, T3)>(new WhenAllPromise<T1, T2, T3>(task1, task2, task3), 0);
        }

        sealed class WhenAllPromise<T1, T2, T3> : IUniTaskSource<(T1, T2, T3)>
        {
            T1 t1 = default;
            T2 t2 = default;
            T3 t3 = default;
            int completedCount;
            UniTaskCompletionSourceCore<(T1, T2, T3)> core;

            public WhenAllPromise(UniTask<T1> task1, UniTask<T2> task2, UniTask<T3> task3)
            {
                TaskTracker.TrackActiveTask(this, 3);

                this.completedCount = 0;
                {
                    var awaiter = task1.GetAwaiter();
                    if (awaiter.IsCompleted)
                    {
                        TryInvokeContinuationT1(this, awaiter);
                    }
                    else
                    {
                        awaiter.SourceOnCompleted(state =>
                        {
                            using (var t = (StateTuple<WhenAllPromise<T1, T2, T3>, UniTask<T1>.Awaiter>)state)
                            {
                                TryInvokeContinuationT1(t.Item1, t.Item2);
                            }
                        }, StateTuple.Create(this, awaiter));
                    }
                }
                {
                    var awaiter = task2.GetAwaiter();
                    if (awaiter.IsCompleted)
                    {
                        TryInvokeContinuationT2(this, awaiter);
                    }
                    else
                    {
                        awaiter.SourceOnCompleted(state =>
                        {
                            using (var t = (StateTuple<WhenAllPromise<T1, T2, T3>, UniTask<T2>.Awaiter>)state)
                            {
                                TryInvokeContinuationT2(t.Item1, t.Item2);
                            }
                        }, StateTuple.Create(this, awaiter));
                    }
                }
                {
                    var awaiter = task3.GetAwaiter();
                    if (awaiter.IsCompleted)
                    {
                        TryInvokeContinuationT3(this, awaiter);
                    }
                    else
                    {
                        awaiter.SourceOnCompleted(state =>
                        {
                            using (var t = (StateTuple<WhenAllPromise<T1, T2, T3>, UniTask<T3>.Awaiter>)state)
                            {
                                TryInvokeContinuationT3(t.Item1, t.Item2);
                            }
                        }, StateTuple.Create(this, awaiter));
                    }
                }
            }

            static void TryInvokeContinuationT1(WhenAllPromise<T1, T2, T3> self, in UniTask<T1>.Awaiter awaiter)
            {
                try
                {
                    self.t1 = awaiter.GetResult();
                }
                catch (Exception ex)
                {
                    self.core.TrySetException(ex);
                    return;
                }
                
                if (Interlocked.Increment(ref self.completedCount) == 3)
                {
                    self.core.TrySetResult((self.t1, self.t2, self.t3));
                }
            }

            static void TryInvokeContinuationT2(WhenAllPromise<T1, T2, T3> self, in UniTask<T2>.Awaiter awaiter)
            {
                try
                {
                    self.t2 = awaiter.GetResult();
                }
                catch (Exception ex)
                {
                    self.core.TrySetException(ex);
                    return;
                }
                
                if (Interlocked.Increment(ref self.completedCount) == 3)
                {
                    self.core.TrySetResult((self.t1, self.t2, self.t3));
                }
            }

            static void TryInvokeContinuationT3(WhenAllPromise<T1, T2, T3> self, in UniTask<T3>.Awaiter awaiter)
            {
                try
                {
                    self.t3 = awaiter.GetResult();
                }
                catch (Exception ex)
                {
                    self.core.TrySetException(ex);
                    return;
                }
                
                if (Interlocked.Increment(ref self.completedCount) == 3)
                {
                    self.core.TrySetResult((self.t1, self.t2, self.t3));
                }
            }


            public (T1, T2, T3) GetResult(short token)
            {
                TaskTracker.RemoveTracking(this);
                GC.SuppressFinalize(this);
                return core.GetResult(token);
            }

            void IUniTaskSource.GetResult(short token)
            {
                GetResult(token);
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
        }
    }
}