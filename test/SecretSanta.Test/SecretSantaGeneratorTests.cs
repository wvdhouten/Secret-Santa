namespace SecretSanta.Test
{
    using SecretSanta;
    using Shouldly;
    using Xunit;

    public class SecretSantaGeneratorTests
    {
        private readonly IList<string> participants = [];
        private readonly IEnumerable<KeyValuePair<string, string>> banned;
        private readonly IEnumerable<KeyValuePair<string, string>> bannedWithUnpairable;

        public SecretSantaGeneratorTests()
        {
            participants = ["A", "B", "C", "D"];

            banned =
            [
                new KeyValuePair<string, string>(participants[0], participants[2]),
                new KeyValuePair<string, string>(participants[1], participants[3]),
            ];

            bannedWithUnpairable =
            [
                new KeyValuePair<string, string>(participants[0], participants[1]),
                new KeyValuePair<string, string>(participants[0], participants[2]),
                new KeyValuePair<string, string>(participants[0], participants[3]),
            ];
        }

        [Fact]
        public void Generate_ReturnsAList()
        {
            // Act
            var result = SecretSantaGenerator.Generate(participants);

            // Assert
            AssertResultIsValid(result.Value);
        }

        [Fact]
        public void GernerateAll_ReturnsAllList()
        {
            // Act
            var result = SecretSantaGenerator.GenerateAll(participants);

            // Assert
            foreach (var list in result.Value)
                AssertResultIsValid(list);
        }

        [Fact]
        public void Generate_WithBanned_ReturnsAList()
        {
            // Act
            var result = SecretSantaGenerator.Generate(participants, banned);

            // Assrt
            AssertResultIsValid(result.Value);
            AssertResultHasNoBannedPair(result.Value);
        }

        [Fact]
        public void Generate_WithParticipantThatCannotBePaired_ReturnsFailedResult()
        {
            // Act
            var result = SecretSantaGenerator.Generate(participants, bannedWithUnpairable);

            // Assert
            result.IsSuccess.ShouldBeFalse("Unpairable participants should not produce a successful result.");
        }

        [Fact]
        public void GenerateAll_WithBanned_ReturnsAllLists()
        {
            // Act
            var result = SecretSantaGenerator.GenerateAll(participants, banned);

            // Assert
            foreach (var list in result.Value)
            {
                AssertResultIsValid(list);
                AssertResultHasNoBannedPair(list);
            }
        }

        private void AssertResultIsValid(IDictionary<string, string> list)
        {
            foreach (var gifter in list.Keys)
                participants.ShouldContain(gifter, "Each participant should be included as a gifter.");

            foreach (var giftee in list.Values)
                participants.ShouldContain(giftee, "Each participant should be included as a giftee.");

            foreach (var pair in list)
                pair.Key.ShouldNotBe(pair.Value, "A participant may never have to gift to themselves.");
        }

        private void AssertResultHasNoBannedPair(IDictionary<string, string> result)
        {
            foreach (var bannedPair in banned)
                result.ShouldNotContain(bannedPair, "The result should not contain a banned pair.");
        }
    }
}
