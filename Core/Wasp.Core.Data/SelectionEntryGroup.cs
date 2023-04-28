namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines a selection entry group.
    /// </summary>
    public class SelectionEntryGroup
        : ConfigurableNamedEntry, ILinkedEntry
    {
        /// <summary>
        /// Gets or sets the category links.
        /// </summary>
        public List<CategoryLink>? CategoryLinks { get; set; }

        /// <summary>
        /// Gets or sets the default selection entry id.
        /// </summary>
        public string? DefaultSelectionEntryId { get; set; }

        /// <summary>
        /// Gets or sets the entry links.
        /// </summary>
        public List<EntryLink>? EntryLinks { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether this selection entry group is collective or not.
        /// </summary>
        public bool IsCollective { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether this selection entry group is hidden or not.
        /// </summary>
        public bool? IsHidden { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether this selection entry group is an import or not.
        /// </summary>
        public bool IsImport { get; set; }

        /// <summary>
        /// Gets or sets the page number for finding this selection entry.
        /// </summary>
        public string? Page { get; set; }

        /// <summary>
        /// Gets or sets the profiles.
        /// </summary>
        public List<Profile>? Profiles { get; set; }

        /// <summary>
        /// Gets or sets the id of the publication this selection entry is from.
        /// </summary>
        public string? PublicationId { get; set; }

        /// <summary>
        /// Gets or sets the selection entries.
        /// </summary>
        public List<SelectionEntry>? SelectionEntries { get; set; }

        /// <summary>
        /// Gets or sets the selection entry groups.
        /// </summary>
        public List<SelectionEntryGroup>? SelectionEntryGroups { get; set; }
    }
}