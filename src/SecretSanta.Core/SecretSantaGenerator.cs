namespace SecretSanta;

using SecretSanta.Extensions;
using System.Linq;

/// <summary>
/// A generator for creating secret santa lists.
/// </summary>
public static class SecretSantaGenerator
{
    /// <summary>
    /// Generates a list of secret santa pairs without matching a participant with itself.
    /// </summary>
    /// <typeparam name="T">The type of participant.</typeparam>
    /// <param name="participants">The list of participants.</param>
    /// <returns>A randomized list of valid secret santa pairs.</returns>
    public static GenerationResult<Dictionary<T, T>> Generate<T>(IEnumerable<T> participants) where T : notnull
    {
        return Generate(participants, []);
    }

    /// <summary>
    /// Generates a list of secret santa pairs without matching a participant with itself or another participant it's banned from pairing with.
    /// </summary>
    /// <typeparam name="T">The type of participant.</typeparam>
    /// <param name="participants">The list of participants.</param>
    /// <returns>A randomized list of valid secret santa pairs.</returns>
    public static GenerationResult<Dictionary<T, T>> Generate<T>(IEnumerable<T> participants, IEnumerable<KeyValuePair<T, T>> bannedPairs) where T : notnull
    {
        if (participants.HasDuplicates())
            return GenerationResult<Dictionary<T, T>>.Failure("Participants list may not contain duplicates.");

        var from = participants.ToList();

        foreach (var to in participants.ToShuffledList().GetPermutations())
        {
            var pairs = from.ZipToKeyValuePairs(to);
            if (IsValidList(pairs, bannedPairs))
                return pairs.ToDictionary();
        }

        return GenerationResult<Dictionary<T, T>>.Failure("No list with valid pairs can be generated.");
    }

    /// <summary>
    /// Generates all possible lists of secret santa pairs without matching a participant with itself.
    /// </summary>
    /// <typeparam name="T">The type of participant.</typeparam>
    /// <param name="participants">The list of participants.</param>
    /// <returns>A randomized list of valid secret santa pairs.</returns>
    public static GenerationResult<IEnumerable<Dictionary<T, T>>> GenerateAll<T>(IList<T> participants) where T : notnull
    {
        return GenerateAll(participants, []);
    }

    /// <summary>
    /// Generates all possible lists of secret santa pairs without matching a participant with itself or another participant it's banned from pairing with.
    /// </summary>
    /// <typeparam name="T">The type of participant.</typeparam>
    /// <param name="participants">The list of participants.</param>
    /// <returns>A randomized list of valid secret santa pairs.</returns>
    public static GenerationResult<IEnumerable<Dictionary<T, T>>> GenerateAll<T>(IList<T> participants, IEnumerable<KeyValuePair<T, T>> bannedPairs) where T : notnull
    {
        if (participants.HasDuplicates())
            return GenerationResult<IEnumerable<Dictionary<T, T>>>.Failure("Participants list may not contain duplicates.");

        return GenerationResult<IEnumerable<Dictionary<T, T>>>.Success(EnumerateAll(participants, bannedPairs));
    }

    private static IEnumerable<Dictionary<T, T>> EnumerateAll<T>(IList<T> participants, IEnumerable<KeyValuePair<T, T>> bannedPairs) where T : notnull
    {
        var from = participants.ToList();
        foreach (var to in participants.ToShuffledList().GetPermutations())
        {
            var pairs = from.ZipToKeyValuePairs(to);
            if (IsValidList(pairs, bannedPairs))
                yield return new Dictionary<T, T>(pairs);
        }
    }

    private static bool IsValidList<T>(IEnumerable<KeyValuePair<T, T>> pairs, IEnumerable<KeyValuePair<T, T>> bannedPairs) where T : notnull
    {
        return !pairs.Any(pair => pair.Key.Equals(pair.Value) || bannedPairs.Contains(pair));
    }
}