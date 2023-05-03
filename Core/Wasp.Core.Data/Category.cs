namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines a category.
    /// </summary>
    public class Category
        : PublicationEntryBase
    {
        /// <summary>
        /// Gets or sets the entry id.
        /// </summary>
        public string? EntryId { get; set; }
    
        /// <summary>
        /// Gets or sets a flag indicating whether this is the primary category.
        /// </summary>
        public bool IsPrimary { get; set; }
    }
}