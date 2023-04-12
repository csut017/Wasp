namespace Wasp.Core.Data.Tests
{
    public class RuleTests
    {
        [Fact]
        public void DefinitionHandlesRootLevelInformation()
        {
            // Arrange
            const string xml = @"<rule id=""4ca6-425a-cd52-5057"" name=""Philosophies of War"" publicationId=""6d04-0cc1-aee0-9041"" page=""92"" hidden=""false"" xmlns=""http://www.battlescribe.net/schema/rosterSchema"">
    <description>Read here -&gt;</description>
</rule>";

            // Act
            var rule = Rule.FromXml(xml).Entity;

            // Assert
            Assert.NotNull(rule);
            Assert.Equal("4ca6-425a-cd52-5057", rule.Id);
            Assert.Equal("Philosophies of War", rule.Name);
            Assert.Equal("6d04-0cc1-aee0-9041", rule.PublicationId);
            Assert.Equal(92, rule.Page);
            Assert.False(rule.IsHidden);
            Assert.Equal("Read here ->", rule.Description);
        }
    }
}