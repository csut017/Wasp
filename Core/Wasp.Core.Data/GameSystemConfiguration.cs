namespace Wasp.Core.Data
{
    /// <summary>
    /// Contains the configuration for a game system.
    /// </summary>
    public class GameSystemConfiguration
        : GameSystemIndex
    {
        /// <summary>
        /// Gets or sets the catalogue links.
        /// </summary>
        public List<CatalogueLink>? CatalogueLinks { get; set; }

        /// <summary>
        /// Gets or sets the category entries for the configuration.
        /// </summary>
        public List<CategoryEntry>? CategoryEntries { get; set; }

        /// <summary>
        /// Gets or sets the cost types in the configuration.
        /// </summary>
        public List<CostType>? CostTypes { get; set; }

        /// <summary>
        /// Gets or sets the entry links.
        /// </summary>
        public List<EntryLink>? EntryLinks { get; set; }

        /// <summary>
        /// Gets or sets the force entries for the configuration.
        /// </summary>
        public List<ForceEntry>? ForceEntries { get; set; }

        /// <summary>
        /// Gets or sets the information links.
        /// </summary>
        public List<InformationLink>? InformationLinks { get; set; }

        /// <summary>
        /// Gets the profile types defined by this configuration.
        /// </summary>
        public List<ProfileType>? ProfileTypes { get; set; }

        /// <summary>
        /// Gets the publications referenced by this configuration.
        /// </summary>
        public List<Publication>? Publications { get; set; }

        /// <summary>
        /// Gets or sets the rules.
        /// </summary>
        public List<Rule>? Rules { get; set; }

        /// <summary>
        /// Gets or sets the selection entries.
        /// </summary>
        public List<SelectionEntry>? SelectionEntries { get; set; }

        /// <summary>
        /// Gets or sets the shared information groups.
        /// </summary>
        public List<InformationGroup>? SharedInformationGroups { get; set; }

        /// <summary>
        /// Gets or sets the shared profiles.
        /// </summary>
        public List<Profile>? SharedProfiles { get; set; }

        /// <summary>
        /// Gets or sets the shared rules.
        /// </summary>
        public List<Rule>? SharedRules { get; set; }

        /// <summary>
        /// Gets or sets the shared selection entries for the configuration.
        /// </summary>
        public List<SelectionEntry>? SharedSelectionEntries { get; set; }

        /// <summary>
        /// Gets or sets the shared selection entry groups.
        /// </summary>
        public List<SelectionEntryGroup>? SharedSelectionEntryGroups { get; set; }
    }
}