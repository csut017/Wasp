namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines a profile.
    /// </summary>
    public class Profile
        : PublicationEntryBase
    {
        /// <summary>
        /// Gets the characteristics for this profile.
        /// </summary>
        public List<Characteristic>? Characteristics { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether this rule is hidden or not.
        /// </summary>
        public bool? IsHidden { get; set; }

        /// <summary>
        /// Gets or sets the modifier groups.
        /// </summary>
        public List<ModifierGroup>? ModifierGroups { get; set; }

        /// <summary>
        /// Gets or sets the modifiers.
        /// </summary>
        public List<Modifier>? Modifiers { get; set; }

        /// <summary>
        /// Gets or sets the id of the profile type.
        /// </summary>
        public string? TypeId { get; set; }

        /// <summary>
        /// Gets or sets the name of the profile type.
        /// </summary>
        public string? TypeName { get; set; }
    }
}