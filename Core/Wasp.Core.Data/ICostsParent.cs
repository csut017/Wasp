namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines an item that can have associated costs.
    /// </summary>
    public interface ICostsParent
    {
        /// <summary>
        /// Gets the costs of the item.
        /// </summary>
        List<ItemCost>? Costs { get; set; }
    }
}