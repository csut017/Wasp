namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines a profile.
    /// </summary>
    public class Profile
        : ConfigurationEntry
    {
        /// <summary>
        /// Gets the characteristics for this profile.
        /// </summary>
        public List<Characteristic>? Characteristics { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether this rule is hidden or not.
        /// </summary>
        public bool IsHidden { get; set; }

        /// <summary>
        /// Gets or sets the modifiers.
        /// </summary>
        public List<Modifier>? Modifiers { get; set; }

        /// <summary>
        /// Gets or sets the page number for finding this rule.
        /// </summary>
        public string? Page { get; set; }

        /// <summary>
        /// Gets or sets the id of the publication this profile is from.
        /// </summary>
        public string? PublicationId { get; set; }

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