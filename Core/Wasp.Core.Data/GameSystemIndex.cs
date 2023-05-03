namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines a game system that can be used in a roster.
    /// </summary>
    public class GameSystemIndex
        : GameSystemEntryBase
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
        /// Gets or sets the children.
        /// </summary>
        public List<GameSystemIndex>? Children { get; set; }

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
        public bool? IsLibrary { get; set; }

        /// <summary>
        /// Gets or sets the readme text for the configuration.
        /// </summary>
        public string? ReadMe { get; set; }

        /// <summary>
        /// Gets or sets the revision of the configuration.
        /// </summary>
        public string? Revision { get; set; }

        /// <summary>
        /// Gets or sets the configuration type.
        /// </summary>
        public ConfigurationType Type { get; set; }

        /// <summary>
        /// Converts the current configuration into an index.
        /// </summary>
        /// <returns>A <see cref="GameSystemIndex"/> index with the relevant details.</returns>
        public GameSystemIndex AsIndex()
        {
            var value = new GameSystemIndex
            {
                AuthorContact = AuthorContact,
                AuthorName = AuthorName,
                AuthorUrl = AuthorUrl,
                BattleScribeVersion = BattleScribeVersion,
                Comment = Comment,
                GameSystemId = GameSystemId,
                GameSystemRevision = GameSystemRevision,
                Id = Id,
                IsLibrary = IsLibrary,
                Name = Name,
                ReadMe = ReadMe,
                Revision = Revision,
                Type = Type,
            };
            return value;
        }
    }
}