namespace SecretSanta.Core
{
    using SecretSanta.Core.Exceptions;
    using SecretSanta.Core.Extensions;
    using System.Linq;

    /// <summary>
    /// A generator for creating secret santa gifter-giftee lists.
    /// </summary>
    public static class SecretSantaGenerator
    {
        /// <summary>
        /// Generates a list of secret santa matches without matching a participant to themself.
        /// </summary>
        /// <typeparam name="T">The type of participant.</typeparam>
        /// <param name="participants">The list of participants.</param>
        /// <returns>A randomized list of valid secret santa matches.</returns>
        public static IDictionary<T, T> Generate<T>(IEnumerable<T> participants) where T : notnull
        {
            return Generate(participants, new List<KeyValuePair<T,T>>());
        }

        /// <summary>
        /// Generates a list of secret santa matches without matching a participant to themself or another participant they're banned from pairing to.
        /// </summary>
        /// <typeparam name="T">The type of participant.</typeparam>
        /// <param name="participants">The list of participants.</param>
        /// <returns>A randomized list of valid secret santa matches.</returns>
        public static IDictionary<T, T> Generate<T>(IEnumerable<T> participants, IEnumerable<KeyValuePair<T, T>> bannedPairs) where T : notnull
        {
            var to = participants.GetRandomizedList();

            var permutations = participants.GetRandomizedList().GetPermutations();
            foreach (var from in permutations)
            {
                var pairs = to.ZipToKeyValuePairs(from);
                if (IsPairValid(pairs, bannedPairs))
                {
                    return pairs.ToDictionary();
                }
            }

            throw new GenerationException("No list with valid pairs can be generated");
        }

        /// <summary>
        /// Generates all possible lists of secret santa matches without matching a participant to themself.
        /// </summary>
        /// <typeparam name="T">The type of participant.</typeparam>
        /// <param name="participants">The list of participants.</param>
        /// <returns>A randomized list of valid secret santa matches.</returns>
        public static IEnumerable<IDictionary<T, T>> GenerateAll<T>(IList<T> participants) where T : notnull
        {
            return GenerateAll(participants, new Dictionary<T, T>());
        }

        /// <summary>
        /// Generates all possible lists of secret santa matches without matching a participant to themself or another participant they're banned from pairing to.
        /// </summary>
        /// <typeparam name="T">The type of participant.</typeparam>
        /// <param name="participants">The list of participants.</param>
        /// <returns>A randomized list of valid secret santa matches.</returns>
        public static IEnumerable<IDictionary<T, T>> GenerateAll<T>(IList<T> participants, IEnumerable<KeyValuePair<T, T>> bannedPairs) where T : notnull
        {
            var to = participants.GetRandomizedList();

            foreach (var from in participants.GetRandomizedList().GetPermutations())
            {
                var pairs = to.ZipToKeyValuePairs(from);
                if (IsPairValid(pairs, bannedPairs))
                {
                    yield return new Dictionary<T, T>(pairs);
                }
            }
        }

        private static bool IsPairValid<T>(IEnumerable<KeyValuePair<T, T>> pairs, IEnumerable<KeyValuePair<T, T>> bannedPairs) where T : notnull
        {
            return !pairs.Any(pair => pair.Key.Equals(pair.Value) || bannedPairs.Contains(pair));
        }
    }
}