namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines a roster (army list).
    /// </summary>
    public class Roster
    {
        /// <summary>
        /// Gets or sets the version of BattleScribe used to generate this roster.
        /// </summary>
        public string? BattleScribeVersion { get; set; }

        /// <summary>
        /// Gets or sets the cost limits of the roster.
        /// </summary>
        public List<ItemCost>? CostLimits { get; set; }

        /// <summary>
        /// Gets the costs of the roster.
        /// </summary>
        public List<ItemCost>? Costs { get; set; }

        /// <summary>
        /// Gets the forces in the roster.
        /// </summary>
        public List<Force>? Forces { get; set; }

        /// <summary>
        /// Gets or sets the id of the game system this roster is for.
        /// </summary>
        public string? GameSystemId { get; set; }

        /// <summary>
        /// Gets or sets the name of the game system this roster is for.
        /// </summary>
        public string? GameSystemName { get; set; }

        /// <summary>
        /// Gets or sets the revision of the game system this roster is for.
        /// </summary>
        public string? GameSystemRevision { get; set; }

        /// <summary>
        /// Gets or sets the id of the roster.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the roster.
        /// </summary>
        public string? Name { get; set; }
    }
}