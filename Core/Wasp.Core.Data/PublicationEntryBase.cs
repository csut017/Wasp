namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines a <see cref="GameSystemEntryBase"/> that also has publication details.
    /// </summary>
    public abstract class PublicationEntryBase
        : GameSystemEntryBase
    {
        /// <summary>
        /// Gets or sets the page number of the entry.
        /// </summary>
        public string? Page { get; set; }

        /// <summary>
        /// Gets or sets the publication ID the entry is referencing.
        /// </summary>
        public string? PublicationId { get; set; }
    }
}