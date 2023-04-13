namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines a cost of an item.
    /// </summary>
    public class ItemCost
    {
        /// <summary>
        /// Gets or sets the name of the cost.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the id of the cost type.
        /// </summary>
        public string? TypeId { get; set; }

        /// <summary>
        /// Gets or sets the cost value.
        /// </summary>
        public string? Value { get; set; }
    }
}