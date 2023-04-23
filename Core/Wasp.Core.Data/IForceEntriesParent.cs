namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines an entity that can contain force entries.
    /// </summary>
    public interface IForceEntriesParent
    {
        /// <summary>
        /// Gets or sets the force entries.
        /// </summary>
        List<ForceEntry>? ForceEntries { get; set; }
    }
}