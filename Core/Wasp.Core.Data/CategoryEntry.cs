namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines a category entry.
    /// </summary>
    public class CategoryEntry
        : ConfigurableEntryBase
    {
        /// <summary>
        /// Gets or sets a flag indicating whether this category entry is hidden or not.
        /// </summary>
        public bool? IsHidden { get; set; }
    }
}