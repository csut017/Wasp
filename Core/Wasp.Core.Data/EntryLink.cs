namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines an entry link.
    /// </summary>
    public class EntryLink
        : IModifiersParent, ICategoryLinksParent
    {
        /// <summary>
        /// Gets or sets the category links.
        /// </summary>
        public List<CategoryLink>? CategoryLinks { get; set; }

        /// <summary>
        /// Gets or sets an optional comment on the link.
        /// </summary>
        public string? Comment { get; internal set; }

        /// <summary>
        /// Gets or sets the constraints.
        /// </summary>
        public List<Constraint>? Constraints { get; set; }

        /// <summary>
        /// Gets or sets the id of the entry link.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether this entry link is collective or not.
        /// </summary>
        public bool IsCollective { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether this entry link is hidden or not.
        /// </summary>
        public bool IsHidden { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether this entry link is an import or not.
        /// </summary>
        public bool IsImport { get; set; }

        /// <summary>
        /// Gets or sets the modifiers.
        /// </summary>
        public List<Modifier>? Modifiers { get; set; }

        /// <summary>
        /// Gets or sets the name of the entry link.
        /// </summary>
        public string? Name { get; set; }

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