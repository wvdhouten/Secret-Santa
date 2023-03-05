namespace SecretSanta.Test
{
    using FluentAssertions;
    using SecretSanta.Core;
    using SecretSanta.Core.Exceptions;
    using Xunit;

    public class SecretSantaGeneratorTests
    {
        private readonly IList<string> participants;
        private readonly IEnumerable<KeyValuePair<string, string>> banned;
        private readonly IEnumerable<KeyValuePair<string, string>> bannedWithUnpairable;

        public SecretSantaGeneratorTests()
        {
            participants = new List<string>()
            {
                "A", "B", "C", "D"
            };

            banned = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(participants[0], participants[2]),
                new KeyValuePair<string, string>(participants[1], participants[3]),
            };

            bannedWithUnpairable = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(participants[0], participants[1]),
                new KeyValuePair<string, string>(participants[0], participants[2]),
                new KeyValuePair<string, string>(participants[0], participants[3]),
            };
        }

        [Fact]
        public void Generate_ReturnsAList()
        {
            // Act
            var result = SecretSantaGenerator.Generate(participants);

            // Assert
            AssertResultIsValid(result);
        }

        [Fact]
        public void GernerateAll_ReturnsAllList()
        {
            // Act
            var result = SecretSantaGenerator.GenerateAll(participants);

            // Assert
            foreach (var list in result)
            {
                AssertResultIsValid(list);
            }
        }

        [Fact]
        public void Generate_WithBanned_ReturnsAList()
        {
            // Act
            var result = SecretSantaGenerator.Generate(participants, banned);

            // Assrt
            AssertResultIsValid(result);
            AssertResultHasNoBannedPair(result);
        }

        [Fact]
        public void Generate_WithParticipantThatCannotBePaired_ThrowsException()
        {
            // Act
            var action = () => { SecretSantaGenerator.Generate(participants, bannedWithUnpairable); };

            // Assrt
            action.Should().ThrowExactly<GenerationException>(because: "a participant should at least be pairable to one other participant");
        }

        [Fact]
        public void GenerateAll_WithBanned_ReturnsAllLists()
        {
            // Act
            var result = SecretSantaGenerator.GenerateAll(participants, banned);

            // Assert
            foreach (var list in result)
            {
                AssertResultIsValid(list);
                AssertResultHasNoBannedPair(list);
            }
        }

        private void AssertResultIsValid(IDictionary<string, string> list)
        {
            foreach (var gifter in list.Keys)
            {
                participants.Should().Contain(gifter, because: "each participant should be included as a gifter");
            }

            foreach (var giftee in list.Values)
            {
                participants.Should().Contain(giftee, because: "each participant should be included as a giftee");
            }

            foreach (var pair in list)
            {
                pair.Key.Should().NotBe(pair.Value, because: "a participant may never have to gift to themselves");
            }
        }

        private void AssertResultHasNoBannedPair(IDictionary<string, string> result)
        {
            foreach (var bannedPair in banned)
            {
                result.Should().NotContain(bannedPair, because: "a participant may not gift to a participant they're banned from gifting to");
            }
        }
    }
}
