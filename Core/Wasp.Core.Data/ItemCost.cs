namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines a cost of an item.
    /// </summary>
    public class ItemCost
        : EntryBase
    {
        /// <summary>
        /// Gets or sets the value as a number.
        /// </summary>
        public double? NumericValue
        {
            get
            {
                return double.TryParse(Value, out var value) ? value : null;
            }

            set
            {
                if (value == null)
                {
                    Value = null;
                }
                else
                {
                    Value = value.Value.ToString("0.0");
                }
            }
        }

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