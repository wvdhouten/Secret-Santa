namespace SecretSanta.Extensions;

internal static class ListExtensions
{
    internal static bool HasDuplicates<TType>(this IEnumerable<TType> source) where TType : notnull
    {
        var diffChecker = new HashSet<TType>();
        return !source.All(diffChecker.Add);
    }

    internal static IList<TType> ToShuffledList<TType>(this IEnumerable<TType> source) where TType : notnull
    {
        return new List<TType>(source).Shuffle();
    }

    private static List<TType> Shuffle<TType>(this List<TType> list, Random? random = null)
    {
        random ??= Random.Shared;

        for (int i = list.Count - 1; i > 0; i--)
            list.Swap(i, random.Next(i + 1));

        return list;
    }

    private static void Swap<TType>(this List<TType> list, int left, int right)
    {
        (list[right], list[left]) = (list[left], list[right]);
    }

    internal static IEnumerable<IList<TType>> GetPermutations<TType>(this IList<TType> source)
    {
        if (source.Count == 1)
            yield return source;
        else
            for (int i = 0; i < source.Count; i++)
            {
                var current = source[i];
                var remaining = source.Take(i).Concat(source.Skip(i + 1)).ToList();
                foreach (var permutation in GetPermutations(remaining))
                    yield return new List<TType> { current }.Concat(permutation).ToList();
            }
    }

    internal static IEnumerable<KeyValuePair<TType, TType>> ZipToKeyValuePairs<TType>(this IEnumerable<TType> left, IEnumerable<TType> right) where TType : notnull
    {
        return left.Zip(right, (l, r) => new KeyValuePair<TType, TType>(l, r));
    }
}
