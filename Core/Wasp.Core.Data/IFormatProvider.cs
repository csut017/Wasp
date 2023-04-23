namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines a format provider for a BattleSribe data format.
    /// </summary>
    public interface IFormatProvider
    {
        /// <summary>
        /// Deserializes a game system definition from a string.
        /// </summary>
        /// <param name="definition">The definition to deserialize.</param>
        /// <returns>A <see cref="GameSystem"/> definition.</returns>
        Task<GameSystem> DeserializeGameSystemAsync(string definition);

        /// <summary>
        /// Deserializes a game system definition from a <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> containing the definition to deserialize.</param>
        /// <returns>A <see cref="GameSystem"/> definition.</returns>
        Task<GameSystem> DeserializeGameSystemAsync(Stream stream);

        /// <summary>
        /// Deserializes a roster definition from a string.
        /// </summary>
        /// <param name="definition">The definition to deserialize.</param>
        /// <returns>A <see cref="Roster"/> definition.</returns>
        Task<Roster> DeserializeRosterAsync(string definition);

        /// <summary>
        /// Deserializes a roster definition from a <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> containing the definition to deserialize.</param>
        /// <returns>A <see cref="Roster"/> definition.</returns>
        Task<Roster> DeserializeRosterAsync(Stream stream);

        /// <summary>
        /// Serializes a game system definition to a string.
        /// </summary>
        /// <param name="gameSystem">The <see cref="GameSystem"/> instance to serialize.</param>
        /// <returns>The serialized definition.</returns>
        Task<string> SerializeGameSystemAsync(GameSystem gameSystem);

        /// <summary>
        /// Serializes a game system definition to a <see cref="Stream"/>.
        /// </summary>
        /// <param name="gameSystem">The <see cref="GameSystem"/> instance to serialize.</param>
        /// <param name="stream">The <see cref="Stream"/> to serialize to.</param>
        /// <returns>A <see cref="GameSystem"/> definition.</returns>
        Task SerializeGameSystemAsync(GameSystem gameSystem, Stream stream);

        /// <summary>
        /// Serializes a roster definition to a string.
        /// </summary>
        /// <param name="roster">The <see cref="Roster"/> instance to serialize.</param>
        /// <returns>The serialized definition.</returns>
        Task<string> SerializeRosterAsync(Roster roster);

        /// <summary>
        /// Serializes a roster definition to a <see cref="Stream"/>.
        /// </summary>
        /// <param name="roster">The <see cref="Roster"/> instance to serialize.</param>
        /// <param name="stream">The <see cref="Stream"/> to serialize to.</param>
        /// <returns>A <see cref="Roster"/> definition.</returns>
        Task SerializeRosterAsync(Roster roster, Stream stream);
    }
}