namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines an item with entry links.
    /// </summary>
    public interface ILinkedEntry
    {
        /// <summary>
        /// Gets or sets the entry links.
        /// </summary>
        List<EntryLink>? EntryLinks { get; set; }
    }
}