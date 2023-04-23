namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines a cost type in the game system.
    /// </summary>
    public class CostType
    {
        /// <summary>
        /// Gets or sets the default cost limit.
        /// </summary>
        public string? DefaultCostLimit { get; set; }

        /// <summary>
        /// Gets or sets the id of the cost type.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether this cost type is hidden or not.
        /// </summary>
        public bool IsHidden { get; set; }

        /// <summary>
        /// Gets or sets the name of the cost type.
        /// </summary>
        public string? Name { get; set; }
    }
}