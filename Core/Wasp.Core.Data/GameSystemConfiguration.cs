namespace Wasp.Core.Data
{
    /// <summary>
    /// Contains the configuration for a game system.
    /// </summary>
    public class GameSystemConfiguration
        : ConfigurationEntry
    {
        /// <summary>
        /// Gets or sets the contact details for the author.
        /// </summary>
        public string? AuthorContact { get; set; }

        /// <summary>
        /// Gets or sets the name of the author.
        /// </summary>
        public string? AuthorName { get; set; }

        /// <summary>
        /// Gets or sets the URL of the author.
        /// </summary>
        public string? AuthorUrl { get; set; }

        /// <summary>
        /// Gets or sets the version of BattleScribe used to generate this configuration.
        /// </summary>
        public string? BattleScribeVersion { get; set; }

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
        /// Gets or sets the id of the owning game system.
        /// </summary>
        public string? GameSystemId { get; set; }

        /// <summary>
        /// Gets or sets the revision number of the owning game system.
        /// </summary>
        public string? GameSystemRevision { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether this configuration is a library or not.
        /// </summary>
        public bool IsLibrary { get; set; }

        /// <summary>
        /// Gets the profile types defined by this configuration.
        /// </summary>
        public List<ProfileType>? ProfileTypes { get; set; }

        /// <summary>
        /// Gets the publications referenced by this configuration.
        /// </summary>
        public List<Publication>? Publications { get; set; }

        /// <summary>
        /// Gets or sets the readme text for the configuration.
        /// </summary>
        public string? ReadMe { get; set; }

        /// <summary>
        /// Gets or sets the revision of the configuration.
        /// </summary>
        public string? Revision { get; set; }

        /// <summary>
        /// Gets or sets the rules.
        /// </summary>
        public List<Rule>? Rules { get; set; }

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