using System.Collections.Async;
using System.Linq;

namespace System.Collections
{
    public static class IEnumerableExtensions
    {
        public static IAsyncEnumerable ToAsyncEnumerable(this IEnumerable enumerable, bool runSynchronously = false)
        {
            if (enumerable == null)
                throw new ArgumentNullException(nameof(enumerable));
            return enumerable as IAsyncEnumerable ?? new AsyncEnumerableWrapper<object>(enumerable.Cast<object>(), runSynchronously);
        }
    }
}

namespace System.Collections.Generic
{
    public static class IEnumerableExtensions
    {
        public static IAsyncEnumerable<T> ToAsyncEnumerable<T>(this IEnumerable<T> enumerable, bool runSynchronously = false)
        {
            if (enumerable == null)
                throw new ArgumentNullException(nameof(enumerable));
            return enumerable as IAsyncEnumerable<T> ?? new AsyncEnumerableWrapper<T>(enumerable, runSynchronously);
        }
    }
}
