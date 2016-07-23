using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Collections.Async
{
    public static class IAsyncEnumerableExtensions
    {
        public static async Task<List<T>> ToListAsync<T>(this IAsyncEnumerable<T> source, CancellationToken cancellationToken = default(CancellationToken))
        {
            var resultList = new List<T>();
            using (var enumerator = await source.GetAsyncEnumeratorAsync(cancellationToken)) {
                while (await enumerator.MoveNextAsync(cancellationToken)) {
                    resultList.Add(enumerator.Current);
                }
            }
            return resultList;
        }

        public static async Task<T[]> ToArrayAsync<T>(this IAsyncEnumerable<T> source, CancellationToken cancellationToken = default(CancellationToken))
        {
            var list = await source.ToListAsync(cancellationToken);
            return list.ToArray();
        }
    }
}
