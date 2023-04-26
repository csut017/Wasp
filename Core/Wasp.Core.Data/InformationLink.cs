namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines an information link.
    /// </summary>
    public class InformationLink
        : ConfigurationEntry
    {
        /// <summary>
        /// Gets or sets a flag indicating whether this information link is hidden or not.
        /// </summary>
        public bool IsHidden { get; set; }

        /// <summary>
        /// Gets or sets the modifiers.
        /// </summary>
        public List<Modifier>? Modifiers { get; set; }

        /// <summary>
        /// Gets or sets the page.
        /// </summary>
        public string? Page { get; set; }

        /// <summary>
        /// Gets or sets the publication id.
        /// </summary>
        public string? PublicationId { get; set; }

        /// <summary>
        /// Gets or sets the target id.
        /// </summary>
        public string? TargetId { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        public string? Type { get; set; }
    }
}