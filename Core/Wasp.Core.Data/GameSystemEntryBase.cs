namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines the base game system entry type. All configuration entries must derive from this class.
    /// </summary>
    public abstract class GameSystemEntryBase
        : EntryBase
    {
        /// <summary>
        /// Gets or sets an optional comment on this configuration entry.
        /// </summary>
        public string? Comment { get; set; }
    }
}