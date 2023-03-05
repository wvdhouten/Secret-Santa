namespace SecretSanta.Test
{
    using FluentAssertions;
    using SecretSanta.Core.Extensions;
    using Xunit;

    public class SecretSantaExtensionsTests
    {
        private readonly IList<string> participants;

        public SecretSantaExtensionsTests()
        {
            participants = new List<string> { "A", "B", "C", "D", "E" };
        }

        [Fact]
        public void GetPermutations_ReturnsAllPermutations()
        {
            // Arrange
            var expected = Factoral(participants.Count);

            // Act
            var result = participants.GetPermutations().Count();

            // Assert
            result.Should().Be(expected, because: $"there are {expected} permutations available with {participants.Count} participants");
        }

        private int Factoral(int n)
        {
            return n <= 1 ? 1 : n * Factoral(n - 1);
        }

        [Fact]
        public void GetPermutations_AreAllUnique()
        {
            // Act
            var permutations = participants.GetPermutations().ToList();

            // Assert
            for (int current = 0; current < permutations.Count; current++)
            {
                for (int next = current + 1; next < permutations.Count; next++)
                {
                    AssertPermutationsAreUnique(permutations[current].ToList(), permutations[next].ToList());
                }
            }
        }

        private static void AssertPermutationsAreUnique<T>(IList<T> left, IList<T> right) where T : notnull
        {
            left.Count.Should().Be(right.Count, because: "both permutations should have the same amount of participants");

            bool isUnique = false;
            for (int i = 0; i < left.Count; i++)
            {
                if (!left[i].Equals(right[i]))
                {
                    isUnique = true;
                    break;
                }
            }

            isUnique.Should().BeTrue(because: "all permutations should be unique");
        }

        [Fact]
        public void ToDictionary_ReturnsDictionary()
        {
            // Arrange
            var pairs = GetPairs();

            // Act
            var result = pairs.ToDictionary();

            // Assert
            result.Count.Should().Be(pairs.Count(), because: "the dictonary should be of equal length as the input");

            foreach (var pair in pairs)
            {
                result.Should().Contain(pair, because: "all pairs from the input should be included in the result");
            }
        }

        private IEnumerable<KeyValuePair<string, string>> GetPairs()
        {
            for (int i = 0; i < participants.Count; i++)
            {
                yield return i < participants.Count - 1
                    ? new KeyValuePair<string, string>(participants[i], participants[i + 1])
                    : new KeyValuePair<string, string>(participants[i], participants[0]);
            }
        }

        [Fact]
        public void ZipToKeyValuePairs_ReturnsValidZip()
        {
            // Act
            var result = participants.ZipToKeyValuePairs(participants);

            // Assert
            result.Count().Should().Be(participants.Count, because: "the zipped list should be of equal length as the participant list");

            foreach (var pair in result)
            {
                pair.Key.Should().Be(pair.Value, because: "zipped enumerable should contain all the assigned pairs");
            }
        }
    }
}
