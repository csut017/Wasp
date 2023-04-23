namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines an entity with category links.
    /// </summary>
    public interface ICategoryLinksParent
    {
        /// <summary>
        /// Gets or sets the category links.
        /// </summary>
        List<CategoryLink>? CategoryLinks { get; set; }
    }
}