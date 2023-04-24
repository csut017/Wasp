namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines a force entry.
    /// </summary>
    public class ForceEntry
    {
        /// <summary>
        /// Gets or sets the category links.
        /// </summary>
        public List<CategoryLink>? CategoryLinks { get; set; }

        /// <summary>
        /// Gets or sets the constraints.
        /// </summary>
        public List<Constraint>? Constraints { get; set; }

        /// <summary>
        /// Gets or sets the child force entries.
        /// </summary>
        public List<ForceEntry>? ForceEntries { get; set; }

        /// <summary>
        /// Gets or sets the id of the force entry.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether this force entry is hidden or not.
        /// </summary>
        public bool IsHidden { get; set; }

        /// <summary>
        /// Gets or sets the modifiers.
        /// </summary>
        public List<Modifier>? Modifiers { get; set; }

        /// <summary>
        /// Gets or sets the name of the force entry.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the child rules.
        /// </summary>
        public List<Rule>? Rules { get; set; }
    }
}