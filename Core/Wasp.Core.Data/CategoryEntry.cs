namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines a category entry.
    /// </summary>
    public class CategoryEntry
        : ConfigurableNamedEntry
    {
        /// <summary>
        /// Gets or sets a flag indicating whether this category entry is hidden or not.
        /// </summary>
        public bool IsHidden { get; set; }

        /// <summary>
        /// Gets or sets the page.
        /// </summary>
        public string? Page { get; set; }

        /// <summary>
        /// Gets or sets the publication id.
        /// </summary>
        public string? PublicationId { get; set; }
    }
}