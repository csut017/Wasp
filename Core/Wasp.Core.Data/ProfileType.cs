namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines a type of profile
    /// </summary>
    public class ProfileType
    {
        /// <summary>
        /// Gets or sets the characteristic types.
        /// </summary>
        public List<CharacteristicType>? CharacteristicTypes { get; set; }

        /// <summary>
        /// Gets or sets the id of the profile type.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the profile type.
        /// </summary>
        public string? Name { get; set; }
    }
}