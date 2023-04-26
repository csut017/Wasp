namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines a category link.
    /// </summary>
    public class CategoryLink
        : NamedEntry
    {
        /// <summary>
        /// Gets or sets the constraints in the category link.
        /// </summary>
        public List<Constraint>? Constraints { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether this category link is hidden or not.
        /// </summary>
        public bool IsHidden { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether this category link is primary or not.
        /// </summary>
        public bool IsPrimary { get; set; }

        /// <summary>
        /// Gets or sets the modifiers.
        /// </summary>
        public List<Modifier>? Modifiers { get; set; }

        /// <summary>
        /// Gets or sets the target id.
        /// </summary>
        public string? TargetId { get; set; }
    }
}