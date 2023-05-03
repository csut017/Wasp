namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines a characteristic.
    /// </summary>
    public class Characteristic
        : EntryBase
    {
        /// <summary>
        /// Gets or sets the type of the characteristic.
        /// </summary>
        public string? TypeId { get; set; }

        /// <summary>
        /// Gets or sets the value of the characteristic.
        /// </summary>
        public string? Value { get; set; }

        /// <summary>
        /// Clones this characteristic.
        /// </summary>
        /// <returns>A new <see cref="Characteristic"/> containing a clone of this characteristic.</returns>
        public Characteristic Clone()
        {
            return new Characteristic { Name = Name, TypeId = TypeId, Value = Value };
        }
    }
}