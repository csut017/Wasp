namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines a characteristic.
    /// </summary>
    public class Characteristic
    {
        /// <summary>
        /// Gets or sets the name of the characteristic.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the type of the characteristic.
        /// </summary>
        public string? TypeId { get; set; }

        /// <summary>
        /// Gets or sets the value of the characteristic.
        /// </summary>
        public string? Value { get; set; }
    }
}