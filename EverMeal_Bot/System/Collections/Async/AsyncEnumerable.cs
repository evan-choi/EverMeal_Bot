using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Collections.Async
{
    public sealed class AsyncEnumerable<T> : IAsyncEnumerable<T>
    {
        private Func<AsyncEnumerator<T>.Yield, Task> _enumerationFunction;
        private bool _oneTimeUse;
        
        public AsyncEnumerable(Func<AsyncEnumerator<T>.Yield, Task> enumerationFunction)
            : this(enumerationFunction, oneTimeUse: false)
        {
        }
        
        public AsyncEnumerable(Func<AsyncEnumerator<T>.Yield, Task> enumerationFunction, bool oneTimeUse)
        {
            _enumerationFunction = enumerationFunction;
            _oneTimeUse = oneTimeUse;
        }
        
        public Task<IAsyncEnumerator<T>> GetAsyncEnumeratorAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var enumerator = new AsyncEnumerator<T>(_enumerationFunction, _oneTimeUse);
            return Task.FromResult<IAsyncEnumerator<T>>(enumerator);
        }
        
        Task<IAsyncEnumerator> IAsyncEnumerable.GetAsyncEnumeratorAsync(CancellationToken cancellationToken) => GetAsyncEnumeratorAsync(cancellationToken).ContinueWith<IAsyncEnumerator>(task => task.Result);
        
        public IEnumerator<T> GetEnumerator() => GetAsyncEnumeratorAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        
        IEnumerator IEnumerable.GetEnumerator() => GetAsyncEnumeratorAsync().ConfigureAwait(false).GetAwaiter().GetResult();
    }
}
