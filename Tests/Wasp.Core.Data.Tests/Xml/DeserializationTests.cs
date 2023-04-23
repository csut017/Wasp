using System.Text;
using Wasp.Core.Data.Xml;

namespace Wasp.Core.Data.Tests.Xml
{
    public class DeserializationTests
    {
        [Fact]
        public async Task DeserializeAsyncHandlesCategoriesInForce()
        {
            // Arrange
            const string xml = @"<roster xmlns=""http://www.battlescribe.net/schema/rosterSchema"">
  <forces>
    <force>
      <categories>
        <category id=""ed4b-1921-c595-9fd6"" name=""Uncategorised"" entryId=""(No Category)"" primary=""false""/>
        <category id=""c825-58b8-c26c-67f3"" name=""Configuration"" entryId=""fcff-0f21-93e6-1ddc"" primary=""false""/>
        <category id=""789e-4caa-3ef0-408b"" name=""Stratagems"" entryId=""c845-c72c-6afe-3fc2"" primary=""false""/>
      </categories>
    </force>
  </forces>
</roster>";
            var deserializer = new Format();

            // Act
            var roster = await deserializer.DeserializeRosterAsync(xml);

            // Assert
            var force = roster?.Forces?.FirstOrDefault();
            Assert.NotNull(force?.Categories);
            Assert.Equal(
                new[] {
                    "Uncategorised[ed4b-1921-c595-9fd6:(No Category)]=False",
                    "Configuration[c825-58b8-c26c-67f3:fcff-0f21-93e6-1ddc]=False",
                    "Stratagems[789e-4caa-3ef0-408b:c845-c72c-6afe-3fc2]=False",
                },
                force.Categories.Select(c => $"{c.Name}[{c.Id}:{c.EntryId}]={c.IsPrimary}").ToArray());
        }

        [Fact]
        public async Task DeserializeAsyncHandlesForces()
        {
            // Arrange
            const string xml = @"<roster xmlns=""http://www.battlescribe.net/schema/rosterSchema"">
  <forces>
    <force id=""f142-9b68-fca9-be91"" name=""Test 1""/>
    <force id=""a403-a1ff-b08b-ad85"" name=""Test 2""/>
  </forces>
</roster>";
            var deserializer = new Format();

            // Act
            var roster = await deserializer.DeserializeRosterAsync(xml);

            // Assert
            Assert.NotNull(roster.Forces);
            Assert.Equal(
                new[] {
                    "Test 1[f142-9b68-fca9-be91]",
                    "Test 2[a403-a1ff-b08b-ad85]",
                },
                roster.Forces.Select(f => $"{f.Name}[{f.Id}]").ToArray());
        }

        [Fact]
        public async Task DeserializeAsyncHandlesPublicationsInForce()
        {
            // Arrange
            const string xml = @"<roster xmlns=""http://www.battlescribe.net/schema/rosterSchema"">
  <forces>
    <force>
      <publications>
        <publication id=""85df-1155-c986-4d71"" name=""Psychic Awakening IX: Pariah""/>
        <publication id=""c9fe-4431-b76d-267a"" name=""Psychic Awakening VIII: War of the Spider""/>
      </publications>
    </force>
  </forces>
</roster>";
            var deserializer = new Format();

            // Act
            var roster = await deserializer.DeserializeRosterAsync(xml);

            // Assert
            var force = roster?.Forces?.FirstOrDefault();
            Assert.NotNull(force?.Publications);
            Assert.Equal(
                new[] {
                    "Psychic Awakening IX: Pariah[85df-1155-c986-4d71]",
                    "Psychic Awakening VIII: War of the Spider[c9fe-4431-b76d-267a]",
                },
                force.Publications.Select(p => $"{p.FullName}[{p.Id}]").ToArray());
        }

        [Fact]
        public async Task DeserializeAsyncHandlesRootLevelCosts()
        {
            // Arrange
            const string xml = @"<roster xmlns=""http://www.battlescribe.net/schema/rosterSchema"">
  <costs>
    <cost name="" PL"" typeId=""e356-c769-5920-6e14"" value=""56.0""/>
    <cost name=""CP"" typeId=""2d3b-b544-ad49-fb75"" value=""12.0""/>
    <cost name=""pts"" typeId=""points"" value=""1220.0""/>
  </costs>
</roster>";
            var deserializer = new Format();

            // Act
            var roster = await deserializer.DeserializeRosterAsync(xml);

            // Assert
            Assert.NotNull(roster?.Costs);
            Assert.Equal(
                new[] {
                    " PL[e356-c769-5920-6e14]=56.0",
                    "CP[2d3b-b544-ad49-fb75]=12.0",
                    "pts[points]=1220.0",
                },
                roster.Costs.Select(c => $"{c.Name}[{c.TypeId}]={c.Value}").ToArray());
        }

        [Fact]
        public async Task DeserializeAsyncHandlesRulesInForce()
        {
            // Arrange
            const string xml = @"<roster xmlns=""http://www.battlescribe.net/schema/rosterSchema"">
  <forces>
    <force>
      <rules>
        <rule id=""4ca6-425a-cd52-5057"" name=""Philosophies of War"" publicationId=""6d04-0cc1-aee0-9041"" page=""92"" hidden=""false"">
          <description>Read here -&gt;</description>
        </rule>
        <rule id=""28ec-711c-pubN73170"" name=""Another test rule"" page=""112""/>
      </rules>
    </force>
  </forces>
</roster>";
            var deserializer = new Format();

            // Act
            var roster = await deserializer.DeserializeRosterAsync(xml);

            // Assert
            var force = roster?.Forces?.FirstOrDefault();
            Assert.NotNull(force?.Rules);
            Assert.Equal(
                new[] {
                    "Philosophies of War[4ca6-425a-cd52-5057]=92:Read here ->",
                    "Another test rule[28ec-711c-pubN73170]=112:",
                },
                force.Rules.Select(r => $"{r.Name}[{r.Id}]={r.Page}:{r.Description}").ToArray());
        }

        [Fact]
        public async Task DeserializeAsyncHandlesSelectionSettings()
        {
            // Arrange
            const string xml = @"<roster xmlns=""http://www.battlescribe.net/schema/rosterSchema"">
  <forces>
    <force>
      <selections>
        <selection id=""a676-78a5-0c69-fdb7"" name=""Sept Choice"" entryId=""bcb6-b88a-d3e5-48d5::36d6-7bcd-f258-e86d"" number=""1"" type=""upgrade"" />
      </selections>
    </force>
  </forces>
</roster>";
            var deserializer = new Format();

            // Act
            var roster = await deserializer.DeserializeRosterAsync(xml);

            // Assert
            var selection = roster?.Forces?.FirstOrDefault()?.Selections?.FirstOrDefault();
            Assert.NotNull(selection);
            Assert.Equal("a676-78a5-0c69-fdb7", selection.Id);
            Assert.Equal("Sept Choice", selection.Name);
            Assert.Equal("bcb6-b88a-d3e5-48d5::36d6-7bcd-f258-e86d", selection.EntryId);
            Assert.Equal("1", selection.Number);
            Assert.Equal("upgrade", selection.Type);
            Assert.Null(selection.CustomName);
        }

        [Fact]
        public async Task DeserializeAsyncHandlesSelectionsInForce()
        {
            // Arrange
            const string xml = @"<roster xmlns=""http://www.battlescribe.net/schema/rosterSchema"">
  <forces>
    <force>
      <selections>
        <selection id=""6c42-8858-a7ae-c66e"" name=""Narrative (Crusade)"" entryId=""4bcc-b0f4-b425-f38e::2aee-51b0-2b2b-218d"" entryGroupId=""4bcc-b0f4-b425-f38e::c702-d73b-dccf-5617"" number=""1"" type=""upgrade"">
            <costs>
                <cost name="" PL"" typeId=""e356-c769-5920-6e14"" value=""0.0""/>
                <cost name=""CP"" typeId=""2d3b-b544-ad49-fb75"" value=""0.0""/>
                <cost name=""pts"" typeId=""points"" value=""0.0""/>
            </costs>
        </selection>
      </selections>
    </force>
  </forces>
</roster>";
            var deserializer = new Format();

            // Act
            var roster = await deserializer.DeserializeRosterAsync(xml);

            // Assert
            var force = roster?.Forces?.FirstOrDefault();
            Assert.NotNull(force?.Selections);
            Assert.Equal(
                new[] {
                    "Narrative (Crusade)[6c42-8858-a7ae-c66e]=upgrade(3)",
                },
                force.Selections.Select(s => $"{s.Name}[{s.Id}]={s.Type}({s.Costs?.Count})").ToArray());
        }

        [Fact]
        public async Task DeserializeAsyncHandlesSelectionWithCategories()
        {
            // Arrange
            const string xml = @"<roster xmlns=""http://www.battlescribe.net/schema/rosterSchema"">
  <forces>
    <force>
      <selections>
        <selection id=""6c42-8858-a7ae-c66e"" name=""Narrative (Crusade)"" entryId=""4bcc-b0f4-b425-f38e::2aee-51b0-2b2b-218d"" entryGroupId=""4bcc-b0f4-b425-f38e::c702-d73b-dccf-5617"" number=""1"" type=""upgrade"">
            <costs>
                <cost name="" PL"" typeId=""e356-c769-5920-6e14"" value=""0.0""/>
                <cost name=""CP"" typeId=""2d3b-b544-ad49-fb75"" value=""0.0""/>
                <cost name=""pts"" typeId=""points"" value=""0.0""/>
            </costs>
            <categories>
                <category id=""48db-f2c3-4cc7-170f"" name=""Configuration"" entryId=""fcff-0f21-93e6-1ddc"" primary=""true""/>
            </categories>
        </selection>
      </selections>
    </force>
  </forces>
</roster>";
            var deserializer = new Format();

            // Act
            var roster = await deserializer.DeserializeRosterAsync(xml);

            // Assert
            var selection = roster?.Forces?.FirstOrDefault()?.Selections?.FirstOrDefault();
            Assert.NotNull(selection);
            Assert.Equal(
                new[] {
                    "Configuration[48db-f2c3-4cc7-170f:fcff-0f21-93e6-1ddc]=True",
                },
                selection.Categories?.Select(c => $"{c.Name}[{c.Id}:{c.EntryId}]={c.IsPrimary}").ToArray());
        }

        [Fact]
        public async Task DeserializeAsyncHandlesSelectionWithCustomName()
        {
            // Arrange
            const string xml = @"<roster xmlns=""http://www.battlescribe.net/schema/rosterSchema"">
  <forces>
    <force>
      <selections>
        <selection id=""1240-f481-a3f3-cd99"" name=""Cadre Fireblade"" entryId=""6824-74af-c876-deea::fc98-45a1-95bf-d267"" customName=""Suun&apos;yo"" page="""" number=""1"" type=""model"" />
      </selections>
    </force>
  </forces>
</roster>";
            var deserializer = new Format();

            // Act
            var roster = await deserializer.DeserializeRosterAsync(xml);

            // Assert
            var selection = roster?.Forces?.FirstOrDefault()?.Selections?.FirstOrDefault();
            Assert.NotNull(selection);
            Assert.Equal("Suun'yo", selection.CustomName);
        }

        [Fact]
        public async Task DeserializeAsyncHandlesSelectionWithProfiles()
        {
            // Arrange
            const string xml = @"<roster xmlns=""http://www.battlescribe.net/schema/rosterSchema"">
  <forces>
    <force>
      <selections>
        <selection id=""6c42-8858-a7ae-c66e"" name=""Narrative (Crusade)"" entryId=""4bcc-b0f4-b425-f38e::2aee-51b0-2b2b-218d"" entryGroupId=""4bcc-b0f4-b425-f38e::c702-d73b-dccf-5617"" number=""1"" type=""upgrade"">
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
        </selection>
      </selections>
    </force>
  </forces>
</roster>";
            var deserializer = new Format();

            // Act
            var roster = await deserializer.DeserializeRosterAsync(xml);

            // Assert
            var selection = roster?.Forces?.FirstOrDefault()?.Selections?.FirstOrDefault();
            Assert.NotNull(selection?.Profiles);
            var actual = selection
                .Profiles
                .Select(p => $"{p.Name}[{p.Id}]={p.TypeName}({string.Join(' ', p.Characteristics?.Select(c => c.Name) ?? Array.Empty<string>())})")
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
        public async Task DeserializeAsyncHandlesSelectionWithRules()
        {
            // Arrange
            const string xml = @"<roster xmlns=""http://www.battlescribe.net/schema/rosterSchema"">
  <forces>
    <force>
      <selections>
        <selection id=""6c42-8858-a7ae-c66e"" name=""Narrative (Crusade)"" entryId=""4bcc-b0f4-b425-f38e::2aee-51b0-2b2b-218d"" entryGroupId=""4bcc-b0f4-b425-f38e::c702-d73b-dccf-5617"" number=""1"" type=""upgrade"">
            <rules>
                <rule id=""0fb5-37b8-c8ea-622d::df2f-c364-5cf6-d067"" name=""Markerlights"" publicationId=""6d04-0cc1-aee0-9041"" page=""93"" hidden=""false"">
                    <description>REMOVED</description>
                </rule>
            </rules>
        </selection>
      </selections>
    </force>
  </forces>
</roster>";
            var deserializer = new Format();

            // Act
            var roster = await deserializer.DeserializeRosterAsync(xml);

            // Assert
            var selection = roster?.Forces?.FirstOrDefault()?.Selections?.FirstOrDefault();
            Assert.NotNull(selection?.Rules);
            Assert.Equal(
                new[] {
                    "Markerlights[0fb5-37b8-c8ea-622d::df2f-c364-5cf6-d067]",
                },
                selection.Rules.Select(r => $"{r.Name}[{r.Id}]").ToArray());
        }

        [Fact]
        public async Task DeserializeAsyncHandlesSelectionWithSelections()
        {
            // Arrange
            const string xml = @"<roster xmlns=""http://www.battlescribe.net/schema/rosterSchema"">
  <forces>
    <force>
      <selections>
        <selection id=""6c42-8858-a7ae-c66e"" name=""Narrative (Crusade)"" entryId=""4bcc-b0f4-b425-f38e::2aee-51b0-2b2b-218d"" entryGroupId=""4bcc-b0f4-b425-f38e::c702-d73b-dccf-5617"" number=""1"" type=""upgrade"">
            <selections>
                <selection id=""6c42-8858-a7ae-c66e"" name=""Narrative (Crusade)"" entryId=""4bcc-b0f4-b425-f38e::2aee-51b0-2b2b-218d"" entryGroupId=""4bcc-b0f4-b425-f38e::c702-d73b-dccf-5617"" number=""1"" type=""upgrade"">
                    <costs>
                        <cost name="" PL"" typeId=""e356-c769-5920-6e14"" value=""0.0""/>
                        <cost name=""CP"" typeId=""2d3b-b544-ad49-fb75"" value=""0.0""/>
                        <cost name=""pts"" typeId=""points"" value=""0.0""/>
                    </costs>
                </selection>
            </selections>
        </selection>
      </selections>
    </force>
  </forces>
</roster>";
            var deserializer = new Format();

            // Act
            var roster = await deserializer.DeserializeRosterAsync(xml);

            // Assert
            var selection = roster?.Forces?.FirstOrDefault()?.Selections?.FirstOrDefault();
            Assert.NotNull(selection);
            Assert.Equal(
                new[] {
                    "Narrative (Crusade)[6c42-8858-a7ae-c66e]=upgrade(3)",
                },
                selection.Selections?.Select(s => $"{s.Name}[{s.Id}]={s.Type}({s.Costs?.Count})").ToArray());
        }

        [Fact]
        public async Task DeserializeAsyncHandlesStreams()
        {
            // Arrange
            const string xml = @"<roster id=""2fc0-37ba-c765-ba82"" name=""Test roster"" battleScribeVersion=""2.03"" gameSystemId=""28ec-711c-d87f-3aeb"" gameSystemName=""Warhammer 40,000 9th Edition"" gameSystemRevision=""248"" xmlns=""http://www.battlescribe.net/schema/rosterSchema""></roster>";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(xml));
            var deserializer = new Format();

            // Act
            var roster = await deserializer.DeserializeRosterAsync(stream);

            // Assert
            Assert.NotNull(roster);
            Assert.Equal("2fc0-37ba-c765-ba82", roster.Id);
            Assert.Equal("Test roster", roster.Name);
            Assert.Equal("2.03", roster.BattleScribeVersion);
            Assert.Equal("28ec-711c-d87f-3aeb", roster.GameSystemId);
            Assert.Equal("Warhammer 40,000 9th Edition", roster.GameSystemName);
            Assert.Equal("248", roster.GameSystemRevision);
        }

        [Fact]
        public async Task DeserializeAsyncHandlesStrings()
        {
            // Arrange
            const string xml = @"<roster id=""2fc0-37ba-c765-ba82"" name=""Test roster"" battleScribeVersion=""2.03"" gameSystemId=""28ec-711c-d87f-3aeb"" gameSystemName=""Warhammer 40,000 9th Edition"" gameSystemRevision=""248"" xmlns=""http://www.battlescribe.net/schema/rosterSchema""></roster>";
            var deserializer = new Format();

            // Act
            var roster = await deserializer.DeserializeRosterAsync(xml);

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