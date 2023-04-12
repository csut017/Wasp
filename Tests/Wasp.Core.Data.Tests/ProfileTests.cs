namespace Wasp.Core.Data.Tests
{
    public class ProfileTests
    {
        [Fact]
        public void DefinitionHandlesRootLevelInformation()
        {
            // Arrange
            const string xml = @"<profile id=""678e-4d2c-cc8d-7626::2c53-c14a-e8ff-d763::85f1-8031-7572-7f6e::5b89-99a4-f13e-27a3"" name=""Hover Drone"" publicationId=""6d04-0cc1-aee0-9041"" page=""104"" hidden=""false"" typeId=""72c5eafc-75bf-4ed9-b425-78009f1efe82"" typeName=""Abilities"" xmlns=""http://www.battlescribe.net/schema/rosterSchema"">
    <characteristics>
    <characteristic name=""Description"" typeId=""21befb24-fc85-4f52-a745-64b2e48f8228"">The bearer has a Move characteristic of 10 and the FLY keyword.</characteristic>
    </characteristics>
</profile>";

            // Act
            var profile = Profile.FromXml(xml);

            // Assert
            Assert.NotNull(profile);
            Assert.Equal("678e-4d2c-cc8d-7626::2c53-c14a-e8ff-d763::85f1-8031-7572-7f6e::5b89-99a4-f13e-27a3", profile.Id);
            Assert.Equal("Hover Drone", profile.Name);
            Assert.Equal("6d04-0cc1-aee0-9041", profile.PublicationId);
            Assert.Equal(104, profile.Page);
            Assert.False(profile.IsHidden);
            Assert.Equal("72c5eafc-75bf-4ed9-b425-78009f1efe82", profile.TypeId);
            Assert.Equal("Abilities", profile.TypeName);
        }
    }
}