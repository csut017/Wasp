namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines a selection entry.
    /// </summary>
    public class SelectionEntry
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
        /// Gets or sets the information links.
        /// </summary>
        public List<InformationLink>? InformationLinks { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether this selection entry is collective or not.
        /// </summary>
        public bool IsCollective { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether this selection entry is hidden or not.
        /// </summary>
        public bool IsHidden { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether this selection entry is an import or not.
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
        /// Gets or sets the name of the selection entry.
        /// </summary>
        public string? Name { get; set; }

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
        /// Gets or sets the rules.
        /// </summary>
        public List<Rule>? Rules { get; set; }

        /// <summary>
        /// Gets or sets the selection entries.
        /// </summary>
        public List<SelectionEntry>? SelectionEntries { get; set; }

        /// <summary>
        /// Gets or sets the selection entry groups.
        /// </summary>
        public List<SelectionEntryGroup>? SelectionEntryGroups { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        public string? Type { get; set; }
    }
}