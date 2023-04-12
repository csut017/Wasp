namespace Wasp.Core.Data.Tests
{
    public class SelectionTests
    {
        [Fact]
        public void DefinitionHandlesCostsAndCategories()
        {
            // Arrange
            const string xml = @"<selection id=""7e83-c695-5b41-e080"" name=""Detachment Command Cost"" entryId=""ec87-f19e-eee2-1ba8::9d97-2793-9882-d48a"" number=""1"" type=""upgrade"" xmlns=""http://www.battlescribe.net/schema/rosterSchema"">
    <costs>
    <cost name="" PL"" typeId=""e356-c769-5920-6e14"" value=""0.0""/>
    <cost name=""CP"" typeId=""2d3b-b544-ad49-fb75"" value=""0.0""/>
    <cost name=""pts"" typeId=""points"" value=""0.0""/>
    </costs>
    <categories>
    <category id=""48db-f2c3-4cc7-170f"" name=""Configuration"" entryId=""fcff-0f21-93e6-1ddc"" primary=""true""/>
    </categories>
</selection>";

            // Act
            var selection = Selection.FromXml(xml).Entity;

            // Assert
            Assert.NotNull(selection);
            Assert.Equal(
                new[] {
                    " PL[e356-c769-5920-6e14]=0.0",
                    "CP[2d3b-b544-ad49-fb75]=0.0",
                    "pts[points]=0.0",
                },
                selection.Costs.Select(c => $"{c.Name}[{c.TypeId}]={c.Value:0.0}").ToArray());
            Assert.Equal(
                new[] {
                    "Configuration[48db-f2c3-4cc7-170f:fcff-0f21-93e6-1ddc]=True",
                },
                selection.Categories.Select(c => $"{c.Name}[{c.Id}:{c.EntryId}]={c.Primary}").ToArray());
        }

        [Fact]
        public void DefinitionHandlesCustomName()
        {
            // Arrange
            const string xml = @"<selection id=""1240-f481-a3f3-cd99"" name=""Cadre Fireblade"" entryId=""6824-74af-c876-deea::fc98-45a1-95bf-d267"" customName=""Suun&apos;yo"" page="""" number=""1"" type=""model"" xmlns=""http://www.battlescribe.net/schema/rosterSchema"" />";

            // Act
            var selection = Selection.FromXml(xml).Entity;

            // Assert
            Assert.NotNull(selection);
            Assert.Equal("1240-f481-a3f3-cd99", selection.Id);
            Assert.Equal("Cadre Fireblade", selection.Name);
            Assert.Equal("6824-74af-c876-deea::fc98-45a1-95bf-d267", selection.EntryId);
            Assert.Equal(1, selection.Number);
            Assert.Equal("model", selection.Type);
            Assert.Equal("Suun'yo", selection.CustomName);
            Assert.Equal(string.Empty, selection.Page);
        }

        [Fact]
        public void DefinitionHandlesInnerSelections()
        {
            // Arrange
            const string xml = @"<selection id=""5450-36c6-9f50-17f1"" name=""Game Type"" entryId=""4bcc-b0f4-b425-f38e::bf09-85b2-c097-1071"" number=""1"" type=""upgrade"" xmlns=""http://www.battlescribe.net/schema/rosterSchema"">
    <selections>
        <selection id=""6c42-8858-a7ae-c66e"" name=""Narrative (Crusade)"" entryId=""4bcc-b0f4-b425-f38e::2aee-51b0-2b2b-218d"" entryGroupId=""4bcc-b0f4-b425-f38e::c702-d73b-dccf-5617"" number=""1"" type=""upgrade"">
            <costs>
                <cost name="" PL"" typeId=""e356-c769-5920-6e14"" value=""0.0""/>
                <cost name=""CP"" typeId=""2d3b-b544-ad49-fb75"" value=""0.0""/>
                <cost name=""pts"" typeId=""points"" value=""0.0""/>
            </costs>
        </selection>
    </selections>
</selection>";

            // Act
            var selection = Selection.FromXml(xml).Entity;

            // Assert
            Assert.NotNull(selection?.Selections);
            Assert.Equal(
                new[] {
                    "Narrative (Crusade)[6c42-8858-a7ae-c66e]=upgrade(3)",
                },
                selection.Selections.Select(s => $"{s.Name}[{s.Id}]={s.Type}({s.Costs.Count})").ToArray());
        }

        [Fact]
        public void DefinitionHandlesProfiles()
        {
            // Arrange
            const string xml = @"<selection id=""5450-36c6-9f50-17f1"" name=""Game Type"" entryId=""4bcc-b0f4-b425-f38e::bf09-85b2-c097-1071"" number=""1"" type=""upgrade"" xmlns=""http://www.battlescribe.net/schema/rosterSchema"">
    <profiles>
        <profile id=""6824-74af-c876-deea::b937-2d12-bb8d-beba::0ef2-2668-6edb-95a1"" name=""Cadre Fireblade"" publicationId=""6d04-0cc1-aee0-9041"" page=""103"" hidden=""false"" typeId=""800f-21d0-4387-c943"" typeName=""Unit"">
            <characteristics>
            <characteristic name=""M"" typeId=""0bdf-a96e-9e38-7779"">6&quot;</characteristic>
            <characteristic name=""WS"" typeId=""e7f0-1278-0250-df0c"">4+</characteristic>
            <characteristic name=""BS"" typeId=""381b-eb28-74c3-df5f"">3+</characteristic>
            <characteristic name=""S"" typeId=""2218-aa3c-265f-2939"">3</characteristic>
            <characteristic name=""T"" typeId=""9c9f-9774-a358-3a39"">3</characteristic>
            <characteristic name=""W"" typeId=""f330-5e6e-4110-0978"">4</characteristic>
            <characteristic name=""A"" typeId=""13fc-b29b-31f2-ab9f"">3</characteristic>
            <characteristic name=""Ld"" typeId=""00ca-f8b8-876d-b705"">8</characteristic>
            <characteristic name=""Save"" typeId=""c0df-df94-abd7-e8d3"">4+</characteristic>
            </characteristics>
        </profile>
        <profile id=""6824-74af-c876-deea::2bd3-bd6b-d73e-2998::edde-cecf-0d6b-b533"" name=""Target Sighted"" publicationId=""6d04-0cc1-aee0-9041"" page=""103"" hidden=""false"" typeId=""72c5eafc-75bf-4ed9-b425-78009f1efe82"" typeName=""Abilities"">
            <characteristics>
            <characteristic name=""Description"" typeId=""21befb24-fc85-4f52-a745-64b2e48f8228"">In your Command phase, select one friendly &lt;SEPT&gt; FIRE WARRIOR TEAM unit within 9&quot; of this unit&apos;s CADRE FIREBLADE model, until the start of your next Command phase, each time a CORE model in that unit makes a ranged attack, re-roll a hit roll of 1.</characteristic>
            </characteristics>
        </profile>
        <profile id=""6824-74af-c876-deea::49d7-f1fa-3dad-e2c2::50e6-fdbf-c3b7-5892"" name=""Volley Fire (Aura)"" publicationId=""6d04-0cc1-aee0-9041"" page=""103"" hidden=""false"" typeId=""72c5eafc-75bf-4ed9-b425-78009f1efe82"" typeName=""Abilities"">
            <characteristics>
            <characteristic name=""Description"" typeId=""21befb24-fc85-4f52-a745-64b2e48f8228"">While a friendly &lt;SEPT&gt; CORE unit is within 6&quot; of this unit&apos;s CADRE FIREBLADE model, each time a CORE model in that unit makes an attack with a pulse weapon (pg 130), an unmodified hit roll of 6 scores one additional hit.</characteristic>
            </characteristics>
        </profile>
    </profiles>
</selection>";

            // Act
            var selection = Selection.FromXml(xml).Entity;

            // Assert
            Assert.NotNull(selection?.Profiles);
            var actual = selection
                .Profiles
                .Select(p => $"{p.Name}[{p.Id}]={p.TypeName}({string.Join(' ', p.Characteristics.Select(c => c.Name))})")
                .ToArray();
            Assert.Equal(
                new[] {
                    "Cadre Fireblade[6824-74af-c876-deea::b937-2d12-bb8d-beba::0ef2-2668-6edb-95a1]=Unit(M WS BS S T W A Ld Save)",
                    "Target Sighted[6824-74af-c876-deea::2bd3-bd6b-d73e-2998::edde-cecf-0d6b-b533]=Abilities(Description)",
                    "Volley Fire (Aura)[6824-74af-c876-deea::49d7-f1fa-3dad-e2c2::50e6-fdbf-c3b7-5892]=Abilities(Description)",
                },
                actual);
        }

        [Fact]
        public void DefinitionHandlesRootLevelInformation()
        {
            // Arrange
            const string xml = @"<selection id=""a676-78a5-0c69-fdb7"" name=""Sept Choice"" entryId=""bcb6-b88a-d3e5-48d5::36d6-7bcd-f258-e86d"" number=""1"" type=""upgrade"" xmlns=""http://www.battlescribe.net/schema/rosterSchema"" />";

            // Act
            var selection = Selection.FromXml(xml).Entity;

            // Assert
            Assert.NotNull(selection);
            Assert.Equal("a676-78a5-0c69-fdb7", selection.Id);
            Assert.Equal("Sept Choice", selection.Name);
            Assert.Equal("bcb6-b88a-d3e5-48d5::36d6-7bcd-f258-e86d", selection.EntryId);
            Assert.Equal(1, selection.Number);
            Assert.Equal("upgrade", selection.Type);
            Assert.Null(selection.CustomName);
        }

        [Fact]
        public void DefinitionHandlesRules()
        {
            // Arrange
            const string xml = @"<selection id=""bdf4-ed12-f0b8-4fff"" name=""Markerlight"" entryId=""6824-74af-c876-deea::31db-81e5-9137-aa31::ca6b-6ce9-0f0f-8c1b"" number=""1"" type=""upgrade"" xmlns=""http://www.battlescribe.net/schema/rosterSchema"">
    <rules>
        <rule id=""0fb5-37b8-c8ea-622d::df2f-c364-5cf6-d067"" name=""Markerlights"" publicationId=""6d04-0cc1-aee0-9041"" page=""93"" hidden=""false"">
            <description>REMOVED</description>
        </rule>
    </rules>
</selection>";

            // Act
            var selection = Selection.FromXml(xml).Entity;

            // Assert
            Assert.NotNull(selection?.Rules);
            Assert.Equal(
                new[] {
                    "Markerlights[0fb5-37b8-c8ea-622d::df2f-c364-5cf6-d067]",
                },
                selection.Rules.Select(r => $"{r.Name}[{r.Id}]").ToArray());
        }
    }
}