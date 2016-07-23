using System.Threading;
using System.Threading.Tasks;

namespace System.Collections.Async
{
    public sealed class AsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        public sealed class AsyncEnumerationCanceledException : OperationCanceledException { }

        public sealed class Yield
        {
            private TaskCompletionSource<bool> _resumeTCS;
            private TaskCompletionSource<T> _yieldTCS;

            public CancellationToken CancellationToken { get; private set; }

            public Task ReturnAsync(T item)
            {
                _resumeTCS = new TaskCompletionSource<bool>();
                _yieldTCS.TrySetResult(item);
                return _resumeTCS.Task;
            }

            public void Break()
            {
                SetCanceled();
                throw new AsyncEnumerationCanceledException();
            }

            internal void SetComplete()
            {
                _yieldTCS.TrySetCanceled();
                IsComplete = true;
            }

            internal void SetCanceled()
            {
                SetComplete();
            }

            internal void SetFailed(Exception ex)
            {
                _yieldTCS.TrySetException(ex);
                IsComplete = true;
            }

            internal Task<T> OnMoveNext(CancellationToken cancellationToken)
            {
                if (!IsComplete) {
                    _yieldTCS = new TaskCompletionSource<T>();
                    CancellationToken = cancellationToken;
                    if (_resumeTCS != null)
                        _resumeTCS.SetResult(true);
                }
                return _yieldTCS.Task;
            }

            internal void Finilize()
            {
                SetCanceled();
            }

            internal bool IsComplete { get; set; }
        }

        private Func<Yield, Task> _enumerationFunction;
        private bool _oneTimeUse;
        private Yield _yield;
        private T _current;
        private Task _enumerationTask;
        private Exception _enumerationException;

        public AsyncEnumerator(Func<Yield, Task> enumerationFunction)
            : this(enumerationFunction, oneTimeUse: false)
        {
        }

        public AsyncEnumerator(Func<Yield, Task> enumerationFunction, bool oneTimeUse)
        {
            _enumerationFunction = enumerationFunction;
            _oneTimeUse = oneTimeUse;
            ClearState();
        }

        public T Current
        {
            get
            {
                if (_enumerationTask == null)
                    throw new InvalidOperationException("Call MoveNext() or MoveNextAsync() before accessing the Current item");
                return _current;
            }
            private set
            {
                _current = value;
            }
        }

        object IEnumerator.Current => Current;

        public Task<bool> MoveNextAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (_enumerationException != null) {
                var tcs = new TaskCompletionSource<bool>();
                tcs.SetException(_enumerationException);
                return tcs.Task;
            }
            var moveNextTask = _yield.OnMoveNext(cancellationToken).ContinueWith(OnMoveNextComplete, _yield);
            if (_enumerationTask == null)
                _enumerationTask = _enumerationFunction(_yield).ContinueWith(OnEnumerationComplete, _yield);
            return moveNextTask;
        }

        public bool MoveNext()
        {
            return MoveNextAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public Task ResetAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            Reset();
            return Task.FromResult<object>(null);
        }

        public void Reset()
        {
            if (_oneTimeUse)
                throw new InvalidOperationException("The enumeration can be performed once only");
            ClearState();
        }

        public void Dispose()
        {
            ClearState();
        }

        private void ClearState()
        {
            if (_yield != null)
                _yield.Finilize();

            _yield = new Yield();
            _enumerationTask = null;
            _enumerationException = null;
        }

        private bool OnMoveNextComplete(Task<T> task, object state)
        {
            var yield = (Yield)state;
            if (yield.IsComplete) {
                return false;
            }

            if (task.IsFaulted) {
                _enumerationException = task.Exception;
                throw _enumerationException;
            } else if (task.IsCanceled) {
                return false;
            } else {
                Current = task.Result;
                return true;
            }
        }

        private static void OnEnumerationComplete(Task task, object state)
        {
            var yield = (Yield)state;
            if (task.IsFaulted) {
                if (task.Exception is AsyncEnumerationCanceledException) {
                    yield.SetCanceled();
                } else {
                    yield.SetFailed(task.Exception);
                }
            } else if (task.IsCanceled) {
                yield.SetCanceled();
            } else {
                yield.SetComplete();
            }
        }
    }
}
