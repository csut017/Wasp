namespace Wasp.Core.Data.Tests
{
    public class RosterTests
    {
        [Fact]
        public void DefinitionHandlesCosts()
        {
            // Arrange
            const string xml = @"<roster xmlns=""http://www.battlescribe.net/schema/rosterSchema"">
  <costs>
    <cost name="" PL"" typeId=""e356-c769-5920-6e14"" value=""56.0""/>
    <cost name=""CP"" typeId=""2d3b-b544-ad49-fb75"" value=""12.0""/>
    <cost name=""pts"" typeId=""points"" value=""1220.0""/>
  </costs>
</roster>";

            // Act
            var roster = Roster.FromXml(xml).Entity;

            // Assert
            Assert.NotNull(roster);
            Assert.Equal(
                new[] {
                    " PL[e356-c769-5920-6e14]=56.0",
                    "CP[2d3b-b544-ad49-fb75]=12.0",
                    "pts[points]=1220.0",
                },
                roster.Costs.Select(c => $"{c.Name}[{c.TypeId}]={c.Value:0.0}").ToArray());
        }

        [Fact]
        public void DefinitionHandlesForces()
        {
            // Arrange
            const string xml = @"<roster xmlns=""http://www.battlescribe.net/schema/rosterSchema"">
  <forces>
    <force id=""f142-9b68-fca9-be91"" name=""Test 1""/>
    <force id=""a403-a1ff-b08b-ad85"" name=""Test 2""/>
  </forces>
</roster>";

            // Act
            var roster = Roster.FromXml(xml).Entity;

            // Assert
            Assert.NotNull(roster);
            Assert.Equal(
                new[] {
                    "Test 1[f142-9b68-fca9-be91]",
                    "Test 2[a403-a1ff-b08b-ad85]",
                },
                roster.Forces.Select(f => $"{f.Name}[{f.Id}]").ToArray());
        }

        [Fact]
        public void DefinitionHandlesRootLevelInformation()
        {
            // Arrange
            const string xml = @"<roster id=""2fc0-37ba-c765-ba82"" name=""Test roster"" battleScribeVersion=""2.03"" gameSystemId=""28ec-711c-d87f-3aeb"" gameSystemName=""Warhammer 40,000 9th Edition"" gameSystemRevision=""248"" xmlns=""http://www.battlescribe.net/schema/rosterSchema""></roster>";

            // Act
            var roster = Roster.FromXml(xml).Entity;

            // Assert
            Assert.NotNull(roster);
            Assert.Equal("2fc0-37ba-c765-ba82", roster.Id);
            Assert.Equal("Test roster", roster.Name);
            Assert.Equal("2.03", roster.BattleScribeVersion);
            Assert.Equal("28ec-711c-d87f-3aeb", roster.GameSystemId);
            Assert.Equal("Warhammer 40,000 9th Edition", roster.GameSystemName);
            Assert.Equal("248", roster.GameSystemRevision);
        }
    }
}