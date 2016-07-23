using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Collections.Async
{
    public interface IAsyncEnumerator : IEnumerator, IDisposable
    {
        Task<bool> MoveNextAsync(CancellationToken cancellationToken = default(CancellationToken));
        
        Task ResetAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
    
    public interface IAsyncEnumerator<T> : IEnumerator<T>, IAsyncEnumerator
    {
    }
}