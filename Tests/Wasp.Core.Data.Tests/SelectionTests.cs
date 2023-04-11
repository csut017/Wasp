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
            var selection = Selection.FromXml(xml);

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
        public void DefinitionHandlesRootLevelInformation()
        {
            // Arrange
            const string xml = @"<selection id=""a676-78a5-0c69-fdb7"" name=""Sept Choice"" entryId=""bcb6-b88a-d3e5-48d5::36d6-7bcd-f258-e86d"" number=""1"" type=""upgrade"" xmlns=""http://www.battlescribe.net/schema/rosterSchema"" />";

            // Act
            var selection = Selection.FromXml(xml);

            // Assert
            Assert.NotNull(selection);
            Assert.Equal("a676-78a5-0c69-fdb7", selection.Id);
            Assert.Equal("Sept Choice", selection.Name);
            Assert.Equal("bcb6-b88a-d3e5-48d5::36d6-7bcd-f258-e86d", selection.EntryId);
            Assert.Equal(1, selection.Number);
            Assert.Equal("upgrade", selection.Type);
        }
    }
}