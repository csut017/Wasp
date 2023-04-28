namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines an entity as having category links.
    /// </summary>
    public interface ICategoryLinkable
    {
        /// <summary>
        /// Gets or sets the category links.
        /// </summary>
        List<CategoryLink>? CategoryLinks { get; set; }
    }
}