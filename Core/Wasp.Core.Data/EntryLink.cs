namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines an entry link.
    /// </summary>
    public class EntryLink
        : ConfigurableEntryBase, ICategoryLinkable
    {
        /// <summary>
        /// Gets or sets the category links.
        /// </summary>
        public List<CategoryLink>? CategoryLinks { get; set; }

        /// <summary>
        /// Gets or sets the costs.
        /// </summary>
        public List<ItemCost>? Costs { get; set; }

        /// <summary>
        /// Gets or sets the entry links.
        /// </summary>
        public List<EntryLink>? EntryLinks { get; set; }

        /// <summary>
        /// Gets or sets the information groups.
        /// </summary>
        public List<InformationGroup>? InformationGroups { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether this entry link is collective or not.
        /// </summary>
        public bool? IsCollective { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether this entry link is hidden or not.
        /// </summary>
        public bool? IsHidden { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether this entry link is an import or not.
        /// </summary>
        public bool? IsImport { get; set; }

        /// <summary>
        /// Gets or sets the profiles.
        /// </summary>
        public List<Profile>? Profiles { get; set; }

        /// <summary>
        /// Gets or sets the selection entries.
        /// </summary>
        public List<SelectionEntry>? SelectionEntries { get; set; }

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