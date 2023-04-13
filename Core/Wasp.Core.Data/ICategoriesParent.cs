namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines an item as having categories.
    /// </summary>
    public interface ICategoriesParent
    {
        /// <summary>
        /// Gets the categories in the force.
        /// </summary>
        List<Category>? Categories { get; set; }
    }
}