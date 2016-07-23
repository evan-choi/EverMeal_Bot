using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Collections.Async
{
    public interface IAsyncEnumerable : IEnumerable
    {
        Task<IAsyncEnumerator> GetAsyncEnumeratorAsync(CancellationToken cancellationToken = default(CancellationToken));
    }

    public interface IAsyncEnumerable<T> : IEnumerable<T>, IAsyncEnumerable
    {
        new Task<IAsyncEnumerator<T>> GetAsyncEnumeratorAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}