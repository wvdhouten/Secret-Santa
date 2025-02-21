namespace SecretSanta.Test
{
    using SecretSanta.Extensions;
    using Shouldly;
    using Xunit;

    public class SecretSantaExtensionsTests
    {
        private readonly List<string> participants;

        public SecretSantaExtensionsTests()
        {
            participants = ["A", "B", "C", "D", "E"];
        }

        [Fact]
        public void GetPermutations_ReturnsAllPermutations()
        {
            // Arrange
            var expected = Factoral(participants.Count);

            // Act
            var result = participants.GetPermutations();

            // Assert
            result.Count().ShouldBe(expected);
        }

        private static int Factoral(int n)
        {
            return n <= 1 
                ? 1 
                : n * Factoral(n - 1);
        }

        [Fact]
        public void GetPermutations_AreAllUnique()
        {
            // Act
            var permutations = participants.GetPermutations().ToList();

            // Assert
            for (int current = 0; current < permutations.Count; current++)
                for (int next = current + 1; next < permutations.Count; next++)
                    AssertPermutationsAreUnique(permutations[current], permutations[next]);
        }

        private static void AssertPermutationsAreUnique<T>(IList<T> left, IList<T> right) where T : notnull
        {
            left.Count.ShouldBeSameAs(right.Count, "All permutations should be of the same length.");

            bool isUnique = false;
            for (int i = 0; i < left.Count; i++)
                if (!left[i].Equals(right[i]))
                {
                    isUnique = true;
                    break;
                }

            isUnique.ShouldBeTrue("All permutations should be unique.");
        }

        [Fact]
        public void ZipToKeyValuePairs_ReturnsValidZip()
        {
            // Act
            var result = participants.ZipToKeyValuePairs(participants);

            // Assert
            result.Count().ShouldBe(participants.Count, "The zipped list should be of equal length as the participant list.");

            foreach (var pair in result)
            {
                pair.Key.ShouldBe(pair.Value, "The zipped enumerable should contain all the assigned pairs.");
            }
        }
    }
}
