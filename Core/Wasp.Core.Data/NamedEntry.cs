namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines a configuration entry in a BattleScribe definition.
    /// </summary>
    public abstract class NamedEntry
        : CommentedEntry
    {
        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        public string? DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the id of the configuration entry.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public virtual string? Name { get; set; }
    }
}