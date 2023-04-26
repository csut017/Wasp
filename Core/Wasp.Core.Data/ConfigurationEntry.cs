namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines a configuration entry in a BattleScribe definition.
    /// </summary>
    public abstract class ConfigurationEntry
    {
        /// <summary>
        /// Gets or sets an optional comment on this configuration entry.
        /// </summary>
        public string? Comment { get; set; }

        /// <summary>
        /// Gets or sets the id of the configuration entry.
        /// </summary>
        public string? Id { get; set; }
    }
}