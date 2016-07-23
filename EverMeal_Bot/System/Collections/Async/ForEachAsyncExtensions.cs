using System.Threading;
using System.Threading.Tasks;

namespace System.Collections.Async
{
    public static class ForEachAsyncExtensions
    {
        public static async Task ForEachAsync(this IAsyncEnumerable enumerable, Action<object> action, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var enumerator = await enumerable.GetAsyncEnumeratorAsync(cancellationToken))
            {
                cancellationToken.ThrowIfCancellationRequested();

                while (await enumerator.MoveNextAsync(cancellationToken))
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    action(enumerator.Current);

                    cancellationToken.ThrowIfCancellationRequested();
                }
            }
        }

        public static async Task ForEachAsync(this IAsyncEnumerable enumerable, Action<object, long> action, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var enumerator = await enumerable.GetAsyncEnumeratorAsync(cancellationToken))
            {
                cancellationToken.ThrowIfCancellationRequested();

                long index = 0;

                while (await enumerator.MoveNextAsync(cancellationToken))
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    action(enumerator.Current, index);

                    cancellationToken.ThrowIfCancellationRequested();

                    index++;
                }
            }
        }

        public static async Task ForEachAsync(this IAsyncEnumerable enumerable, Func<object, Task> action, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var enumerator = await enumerable.GetAsyncEnumeratorAsync(cancellationToken))
            {
                cancellationToken.ThrowIfCancellationRequested();

                while (await enumerator.MoveNextAsync(cancellationToken))
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    await action(enumerator.Current);

                    cancellationToken.ThrowIfCancellationRequested();
                }
            }
        }

        public static async Task ForEachAsync(this IAsyncEnumerable enumerable, Func<object, long, Task> action, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var enumerator = await enumerable.GetAsyncEnumeratorAsync(cancellationToken))
            {
                cancellationToken.ThrowIfCancellationRequested();

                long index = 0;

                while (await enumerator.MoveNextAsync(cancellationToken))
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    await action(enumerator.Current, index);

                    cancellationToken.ThrowIfCancellationRequested();

                    index++;
                }
            }
        }

        public static async Task ForEachAsync<T>(this IAsyncEnumerable<T> enumerable, Action<T> action, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var enumerator = await enumerable.GetAsyncEnumeratorAsync(cancellationToken))
            {
                cancellationToken.ThrowIfCancellationRequested();

                while (await enumerator.MoveNextAsync(cancellationToken))
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    action(enumerator.Current);

                    cancellationToken.ThrowIfCancellationRequested();
                }
            }
        }

        public static async Task ForEachAsync<T>(this IAsyncEnumerable<T> enumerable, Action<T, long> action, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var enumerator = await enumerable.GetAsyncEnumeratorAsync(cancellationToken))
            {
                cancellationToken.ThrowIfCancellationRequested();

                long index = 0;

                while (await enumerator.MoveNextAsync(cancellationToken))
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    action(enumerator.Current, index);

                    cancellationToken.ThrowIfCancellationRequested();

                    index++;
                }
            }
        }

        public static async Task ForEachAsync<T>(this IAsyncEnumerable<T> enumerable, Func<T, Task> action, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var enumerator = await enumerable.GetAsyncEnumeratorAsync(cancellationToken))
            {

                cancellationToken.ThrowIfCancellationRequested();

                while (await enumerator.MoveNextAsync(cancellationToken))
                {

                    cancellationToken.ThrowIfCancellationRequested();

                    await action(enumerator.Current);

                    cancellationToken.ThrowIfCancellationRequested();
                }
            }
        }

        public static async Task ForEachAsync<T>(this IAsyncEnumerable<T> enumerable, Func<T, long, Task> action, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var enumerator = await enumerable.GetAsyncEnumeratorAsync(cancellationToken))
            {
                cancellationToken.ThrowIfCancellationRequested();

                long index = 0;

                while (await enumerator.MoveNextAsync(cancellationToken))
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    await action(enumerator.Current, index);

                    cancellationToken.ThrowIfCancellationRequested();

                    index++;
                }
            }
        }
    }
}
