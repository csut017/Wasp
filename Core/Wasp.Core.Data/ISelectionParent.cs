namespace Wasp.Core.Data
{
    /// <summary>
    /// Allows selections on an item.
    /// </summary>
    public interface ISelectionParent
    {
        /// <summary>
        /// Gets or sets the child selections.
        /// </summary>
        List<Selection>? Selections { get; set; }
    }
}