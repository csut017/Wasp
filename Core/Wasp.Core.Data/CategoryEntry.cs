namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines a category entry.
    /// </summary>
    public class CategoryEntry
        : IModifiersParent
    {
        /// <summary>
        /// Gets or sets an optional comment on the entry.
        /// </summary>
        public string? Comment { get; internal set; }

        /// <summary>
        /// Gets or sets the constraints.
        /// </summary>
        public List<Constraint>? Constraints { get; set; }

        /// <summary>
        /// Gets or sets the id of the category entry.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether this category entry is hidden or not.
        /// </summary>
        public bool IsHidden { get; set; }

        /// <summary>
        /// Gets or sets the modifiers.
        /// </summary>
        public List<Modifier>? Modifiers { get; set; }

        /// <summary>
        /// Gets or sets the name of the category entry.
        /// </summary>
        public string? Name { get; set; }

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