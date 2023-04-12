namespace Wasp.Core.Data.Tests
{
    public class CharacteristicTests
    {
        [Fact]
        public void DefinitionHandlesRootLevelInformation()
        {
            // Arrange
            const string xml = @"<characteristic name=""Description"" typeId=""21befb24-fc85-4f52-a745-64b2e48f8228"" xmlns=""http://www.battlescribe.net/schema/rosterSchema"">
    The bearer has a Move characteristic of 10 and the FLY keyword.
</characteristic>";

            // Act
            var characteristic = Characteristic.FromXml(xml);

            // Assert
            Assert.NotNull(characteristic);
            Assert.Equal("21befb24-fc85-4f52-a745-64b2e48f8228", characteristic.TypeId);
            Assert.Equal("Description", characteristic.Name);
            Assert.Equal("The bearer has a Move characteristic of 10 and the FLY keyword.", characteristic.Value?.Trim());
        }
    }
}