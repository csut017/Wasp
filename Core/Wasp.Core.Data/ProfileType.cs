namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines a type of profile
    /// </summary>
    public class ProfileType
        : GameSystemEntryBase
    {
        /// <summary>
        /// Gets or sets the characteristic types.
        /// </summary>
        public List<CharacteristicType>? CharacteristicTypes { get; set; }
    }
}