namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines a format provider for a BattleSribe data format.
    /// </summary>
    public interface IFormatProvider
    {
        /// <summary>
        /// Deserializes a configuration definition from a string.
        /// </summary>
        /// <param name="definition">The definition to deserialize.</param>
        /// <param name="configurationType">Defines the type of configuration to deserialize.</param>
        /// <returns>A <see cref="GameSystemConfiguration"/> definition.</returns>
        Task<GameSystemConfiguration> DeserializeConfigurationAsync(string definition, ConfigurationType configurationType);

        /// <summary>
        /// Deserializes a configuration definition from a <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> containing the definition to deserialize.</param>
        /// <param name="configurationType">Defines the type of configuration to deserialize.</param>
        /// <returns>A <see cref="GameSystemConfiguration"/> definition.</returns>
        Task<GameSystemConfiguration> DeserializeConfigurationAsync(Stream stream, ConfigurationType configurationType);

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
        /// <param name="gameSystem">The <see cref="GameSystemConfiguration"/> instance to serialize.</param>
        /// <param name="configurationType">Defines the type of configuration to serialize.</param>
        /// <returns>The serialized definition.</returns>
        Task<string> SerializeGameSystemAsync(GameSystemConfiguration gameSystem, ConfigurationType configurationType);

        /// <summary>
        /// Serializes a game system definition to a <see cref="Stream"/>.
        /// </summary>
        /// <param name="gameSystem">The <see cref="GameSystemConfiguration"/> instance to serialize.</param>
        /// <param name="stream">The <see cref="Stream"/> to serialize to.</param>
        /// <returns>A <see cref="GameSystemConfiguration"/> definition.</returns>
        /// <param name="configurationType">Defines the type of configuration to serialize.</param>
        Task SerializeGameSystemAsync(GameSystemConfiguration gameSystem, Stream stream, ConfigurationType configurationType);

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