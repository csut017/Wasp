namespace Wasp.Core.Data
{
    /// <summary>
    /// Contains the configuration for a game system.
    /// </summary>
    public class GameSystem
        : IForceEntriesParent
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
        /// Gets or sets the version of BattleScribe used to generate this game system.
        /// </summary>
        public string? BattleScribeVersion { get; set; }

        /// <summary>
        /// Gets or sets the category entries for the game system.
        /// </summary>
        public List<CategoryEntry>? CategoryEntries { get; set; }

        /// <summary>
        /// Gets or sets the cost types in the game system.
        /// </summary>
        public List<CostType>? CostTypes { get; set; }

        /// <summary>
        /// Gets or sets the entry links.
        /// </summary>
        public List<EntryLink>? EntryLinks { get; set; }

        /// <summary>
        /// Gets or sets the force entries for the game system.
        /// </summary>
        public List<ForceEntry>? ForceEntries { get; set; }

        /// <summary>
        /// Gets or sets the id of the game system.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the game system.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets the profile types defined by this game system.
        /// </summary>
        public List<ProfileType>? ProfileTypes { get; set; }

        /// <summary>
        /// Gets the publications referenced by this game system.
        /// </summary>
        public List<Publication>? Publications { get; set; }

        /// <summary>
        /// Gets or sets the readme text for the game system.
        /// </summary>
        public string? ReadMe { get; set; }

        /// <summary>
        /// Gets or sets the revision of the game system.
        /// </summary>
        public string? Revision { get; set; }
    }
}