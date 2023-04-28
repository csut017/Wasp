namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines a cost type in the game system.
    /// </summary>
    public class CostType
        : NamedEntry
    {
        /// <summary>
        /// Gets or sets the default cost limit.
        /// </summary>
        public string? DefaultCostLimit { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether this cost type is hidden or not.
        /// </summary>
        public bool? IsHidden { get; set; }
    }
}