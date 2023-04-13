namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines a format provider for a BattleSribe data format.
    /// </summary>
    public interface IFormatProvider
    {
        /// <summary>
        /// Deserializes a definition from a string.
        /// </summary>
        /// <param name="definition">The definition to deserialize.</param>
        /// <returns>A <see cref="Roster"/> definition.</returns>
        Task<Roster> DeserializeAsync(string definition);

        /// <summary>
        /// Deserializes a definition from a <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> containing the definition to deserialize.</param>
        /// <returns>A <see cref="Roster"/> definition.</returns>
        Task<Roster> DeserializeAsync(Stream stream);

        /// <summary>
        /// Serializes a definition to a <see cref="Stream"/>.
        /// </summary>
        /// <param name="roster">The <see cref="Roster"/> instance to serialize.</param>
        /// <param name="stream">The <see cref="Stream"/> to serialize to.</param>
        /// <returns>A <see cref="Roster"/> definition.</returns>
        Task SerializeAsync(Roster roster, Stream stream);

        /// <summary>
        /// Serializes a definition to a string.
        /// </summary>
        /// <param name="roster">The <see cref="Roster"/> instance to serialize.</param>
        /// <returns>The serialized definition.</returns>
        Task<string> SerializeAsync(Roster roster);
    }
}