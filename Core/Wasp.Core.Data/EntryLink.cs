﻿namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines an entry link.
    /// </summary>
    public class EntryLink
        : ConfigurationEntry
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
        /// Gets or sets the costs.
        /// </summary>
        public List<ItemCost>? Costs { get; set; }

        /// <summary>
        /// Gets or sets the entry links.
        /// </summary>
        public List<EntryLink>? EntryLinks { get; set; }

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
        /// Gets or sets the modifier groups.
        /// </summary>
        public List<ModifierGroup>? ModifierGroups { get; set; }

        /// <summary>
        /// Gets or sets the modifiers.
        /// </summary>
        public List<Modifier>? Modifiers { get; set; }

        /// <summary>
        /// Gets or sets the name of the entry link.
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