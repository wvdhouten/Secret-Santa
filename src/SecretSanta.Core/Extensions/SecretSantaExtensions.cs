using System.Linq;

namespace SecretSanta.Core.Extensions
{
    /// <summary>
    /// Extension methods that help the creation of randomized Secret Santa exchange lists.
    /// </summary>
    public static class SecretSantaExtensions
    {
        /// <summary>
        /// Gets the given list in a random order.
        /// </summary>
        /// <typeparam name="T">The type of the items in the list.</typeparam>
        /// <param name="source">A list of items.</param>
        /// <returns>The randomized list.</returns>
        public static IList<T> GetRandomizedList<T>(this IEnumerable<T> source) where T : notnull
        {
            return source.OrderBy(x => Random.Shared.Next()).ToList();
        }

        /// <summary>
        /// Gets an enumeration that runs through all permutations of a given list.
        /// </summary>
        /// <typeparam name="T">The type of the items in the list.</typeparam>
        /// <param name="source">A list of items.</param>
        /// <returns>All permutations of the list as an enumerable.</returns>
        public static IEnumerable<List<T>> GetPermutations<T>(IList<T> source)
        {
            var count = source.Count;
            var indices = new int[count];
            
            yield return source;
            
            for (var i = 0; i < count;)
            {
                if (indices[i] < i)
                {
                    Swap(source, i % 2 == 0 ? 0 : indices[i], i);
                    yield return list;
                    indices[i]++;
                    i = 0;
                }
                else
                {
                    indices[i++] = 0;
                }
            }
        }
        
        private static void SwapItems<T>(IList<T> list, int itemA, int itemB)
        {
            var temp = list[itemA];
            list[itemA] = list[itemB];
            list[itemB] = temp;
        }
        
        public static IEnumerable<IEnumerable<T>> GetPermutations<T>(this IList<T> source) where T : notnull
        {
            if (source.Count == 0)
            {
                yield return Enumerable.Empty<T>();
            }
            else
            {
                for (int i = 0; i < source.Count; i++)
                {
                    var remaining = new List<T>(source);
                    remaining.RemoveAt(i);
                    foreach (IEnumerable<T> permutation in GetPermutations(remaining))
                    {
                        yield return new[] { source[i] }.Concat(permutation);
                    }
                }
            }
        }

        /// <summary>
        /// Zips two enumerables with items together as an enumerable of key-value pairs.
        /// </summary>
        /// <typeparam name="TLeft">The type of the items in the left enumerable.</typeparam>
        /// <typeparam name="TRight">The type of the items in the right enumerable.</typeparam>
        /// <param name="left">An enumerable of items.</param>
        /// <param name="right">An enumerable of items.</param>
        /// <returns>The enumerables of items zipped together as an enumerable of key-value pairs.</returns>
        public static IEnumerable<KeyValuePair<TLeft, TRight>> ZipToKeyValuePairs<TLeft, TRight>(this IEnumerable<TLeft> left, IEnumerable<TRight> right) where TLeft : notnull
        {
            return left.Zip(right, (l, r) => new KeyValuePair<TLeft, TRight>(l, r));
        }

        /// <summary>
        /// Returns an enumerable of key-value pairs to a dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the dictionary key.</typeparam>
        /// <typeparam name="TValue">The type of the dictionary value.</typeparam>
        /// <param name="source">The enumerable of key-value pairs.</param>
        /// <returns>The enumerable of key-value pairs as a dictionary.</returns>
        public static IDictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source) where TKey : notnull
        {
            return new Dictionary<TKey, TValue>(source);
        }
    }
}
