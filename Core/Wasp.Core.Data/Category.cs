namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines a category.
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Gets or sets the entry id.
        /// </summary>
        public string? EntryId { get; set; }

        /// <summary>
        /// Gets or sets the id of the category.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the category.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether this is the primary category.
        /// </summary>
        public bool IsPrimary { get; set; }
    }
}