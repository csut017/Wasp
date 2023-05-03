namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines a catalogue link
    /// </summary>
    public class CatalogueLink
        : GameSystemEntryBase
    {
        /// <summary>
        /// Gets or sets a flag indicating whether to import the root entries.
        /// </summary>
        public bool ImportRootEntries { get; set; }

        /// <summary>
        /// Gets or sets the target id.
        /// </summary>
        public string? TargetId { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        public string? Type { get; set; }
    }
}