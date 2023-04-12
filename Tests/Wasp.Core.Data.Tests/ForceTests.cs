namespace Wasp.Core.Data.Tests
{
    public class ForceTests
    {
        [Fact]
        public void DefinitionHandlesCategories()
        {
            // Arrange
            const string xml = @"<force xmlns=""http://www.battlescribe.net/schema/rosterSchema"">
  <categories>
    <category id=""ed4b-1921-c595-9fd6"" name=""Uncategorised"" entryId=""(No Category)"" primary=""false""/>
    <category id=""c825-58b8-c26c-67f3"" name=""Configuration"" entryId=""fcff-0f21-93e6-1ddc"" primary=""false""/>
    <category id=""789e-4caa-3ef0-408b"" name=""Stratagems"" entryId=""c845-c72c-6afe-3fc2"" primary=""false""/>
  </categories>
</force>";

            // Act
            var force = Force.FromXml(xml).Entity;

            // Assert
            Assert.NotNull(force);
            Assert.Equal(
                new[] {
                    "Uncategorised[ed4b-1921-c595-9fd6:(No Category)]=False",
                    "Configuration[c825-58b8-c26c-67f3:fcff-0f21-93e6-1ddc]=False",
                    "Stratagems[789e-4caa-3ef0-408b:c845-c72c-6afe-3fc2]=False",
                },
                force.Categories.Select(c => $"{c.Name}[{c.Id}:{c.EntryId}]={c.Primary}").ToArray());
        }

        [Fact]
        public void DefinitionHandlesPublications()
        {
            // Arrange
            const string xml = @"<force xmlns=""http://www.battlescribe.net/schema/rosterSchema"">
  <publications>
    <publication id=""85df-1155-c986-4d71"" name=""Psychic Awakening IX: Pariah""/>
    <publication id=""c9fe-4431-b76d-267a"" name=""Psychic Awakening VIII: War of the Spider""/>
  </publications>
</force>";

            // Act
            var force = Force.FromXml(xml).Entity;

            // Assert
            Assert.NotNull(force);
            Assert.Equal(
                new[] {
                    "Psychic Awakening IX: Pariah[85df-1155-c986-4d71]",
                    "Psychic Awakening VIII: War of the Spider[c9fe-4431-b76d-267a]",
                },
                force.Publications.Select(p => $"{p.Name}[{p.Id}]").ToArray());
        }

        [Fact]
        public void DefinitionHandlesRootLevelInformation()
        {
            // Arrange
            const string xml = @"<force id=""f142-9b68-fca9-be91"" name=""Order of Battle"" entryId=""7986-68a6-9e59-faac"" catalogueId=""c0bb-c0cd-a715-99c6"" catalogueRevision=""146"" catalogueName=""T&apos;au Empire"" xmlns=""http://www.battlescribe.net/schema/rosterSchema""></force>";

            // Act
            var force = Force.FromXml(xml).Entity;

            // Assert
            Assert.NotNull(force);
            Assert.Equal("f142-9b68-fca9-be91", force.Id);
            Assert.Equal("Order of Battle", force.Name);
            Assert.Equal("7986-68a6-9e59-faac", force.EntryId);
            Assert.Equal("c0bb-c0cd-a715-99c6", force.CatalogueId);
            Assert.Equal("146", force.CatalogueRevision);
            Assert.Equal("T'au Empire", force.CatalogueName);
        }

        [Fact]
        public void DefinitionHandlesRules()
        {
            // Arrange
            const string xml = @"<force xmlns=""http://www.battlescribe.net/schema/rosterSchema"">
  <rules>
    <rule id=""4ca6-425a-cd52-5057"" name=""Philosophies of War"" page=""92""/>
    <rule id=""28ec-711c-pubN73170"" name=""Another test rule"" page=""112""/>
  </rules>
</force>";

            // Act
            var force = Force.FromXml(xml).Entity;

            // Assert
            Assert.NotNull(force);
            Assert.Equal(
                new[] {
                    "Philosophies of War[4ca6-425a-cd52-5057]=92",
                    "Another test rule[28ec-711c-pubN73170]=112",
                },
                force.Rules.Select(c => $"{c.Name}[{c.Id}]={c.Page}").ToArray());
        }

        [Fact]
        public void DefinitionHandlesSelections()
        {
            // Arrange
            const string xml = @"<force xmlns=""http://www.battlescribe.net/schema/rosterSchema"">
    <selections>
        <selection id=""6c42-8858-a7ae-c66e"" name=""Narrative (Crusade)"" entryId=""4bcc-b0f4-b425-f38e::2aee-51b0-2b2b-218d"" entryGroupId=""4bcc-b0f4-b425-f38e::c702-d73b-dccf-5617"" number=""1"" type=""upgrade"">
            <costs>
                <cost name="" PL"" typeId=""e356-c769-5920-6e14"" value=""0.0""/>
                <cost name=""CP"" typeId=""2d3b-b544-ad49-fb75"" value=""0.0""/>
                <cost name=""pts"" typeId=""points"" value=""0.0""/>
            </costs>
        </selection>
    </selections>
</force>";

            // Act
            var force = Force.FromXml(xml).Entity;

            // Assert
            Assert.NotNull(force?.Selections);
            Assert.Equal(
                new[] {
                    "Narrative (Crusade)[6c42-8858-a7ae-c66e]=upgrade(3)",
                },
                force.Selections.Select(s => $"{s.Name}[{s.Id}]={s.Type}({s.Costs.Count})").ToArray());
        }
    }
}