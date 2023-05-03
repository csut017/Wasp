using System.Text;
using System.Xml;

namespace Wasp.Core.Data.Xml
{
    /// <summary>
    /// Defines the XML format provider for a BattleSribe data format.
    /// </summary>
    public class Format
        : IFormatProvider
    {
        /// <summary>
        /// Deserializes a game system definition from a string.
        /// </summary>
        /// <param name="definition">The definition to deserialize.</param>
        /// <param name="configurationType">Defines the type of configuration to serialize.</param>
        /// <param name="level">How much of the definition to deserialize.</param>
        /// <returns>A <see cref="GameSystemConfiguration"/> definition.</returns>
        public async Task<GameSystemConfiguration> DeserializeConfigurationAsync(string definition, ConfigurationType configurationType, ConfigurationLevel level = ConfigurationLevel.All)
        {
            using var reader = new StringReader(definition);
            return await ConfigurationDeserialization.DeserializeRootAsync(reader, configurationType, level);
        }

        /// <summary>
        /// Deserializes a game system definition from a <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> containing the definition to deserialize.</param>
        /// <param name="configurationType">Defines the type of configuration to serialize.</param>
        /// <param name="level">How much of the definition to deserialize.</param>
        /// <returns>A <see cref="GameSystemConfiguration"/> definition.</returns>
        public async Task<GameSystemConfiguration> DeserializeConfigurationAsync(Stream stream, ConfigurationType configurationType, ConfigurationLevel level = ConfigurationLevel.All)
        {
            using var reader = new StreamReader(stream);
            return await ConfigurationDeserialization.DeserializeRootAsync(reader, configurationType, level);
        }

        /// <summary>
        /// Attempts to deserialize the identifier from a definition in a <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> containing the definition to deserialize.</param>
        /// <returns>The identifier if found; null otherwise.</returns>
        public async Task<string?> DeserializeIdAsync(Stream stream)
        {
            using var reader = new StreamReader(stream);
            var settings = new XmlReaderSettings { Async = true };
            using var xmlReader = XmlReader.Create(reader, settings);
            await xmlReader.MoveToContentAsync();
            if ((xmlReader.NodeType == XmlNodeType.Element) && xmlReader.MoveToAttribute("id"))
            {
                return xmlReader.Value;
            }

            return null;
        }

        /// <summary>
        /// Deserializes a definition from a string.
        /// </summary>
        /// <param name="definition">The definition to deserialize.</param>
        /// <returns>A <see cref="Roster"/> definition.</returns>
        public async Task<Roster> DeserializeRosterAsync(string definition)
        {
            using var reader = new StringReader(definition);
            return await RosterDeserialization.DeserializeRootAsync(reader);
        }

        /// <summary>
        /// Deserializes a definition from a <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> containing the definition to deserialize.</param>
        /// <returns>A <see cref="Roster"/> definition.</returns>
        public async Task<Roster> DeserializeRosterAsync(Stream stream)
        {
            using var reader = new StreamReader(stream);
            return await RosterDeserialization.DeserializeRootAsync(reader);
        }

        /// <summary>
        /// Serializes a game system definition to a string.
        /// </summary>
        /// <param name="gameSystem">The <see cref="GameSystemConfiguration"/> instance to serialize.</param>
        /// <param name="configurationType">Defines the type of configuration to serialize.</param>
        /// <returns>The serialized definition.</returns>
        public async Task<string> SerializeGameSystemAsync(GameSystemConfiguration gameSystem, ConfigurationType configurationType)
        {
            var builder = new StringBuilder();
            using var writer = new StringWriter(builder);
            await ConfigurationSerialization.SerializeRootAsync(gameSystem, writer, configurationType);
            return builder.ToString();
        }

        /// <summary>
        /// Serializes a game system definition to a <see cref="Stream"/>.
        /// </summary>
        /// <param name="gameSystem">The <see cref="GameSystemConfiguration"/> instance to serialize.</param>
        /// <param name="stream">The <see cref="Stream"/> to serialize to.</param>
        /// <param name="configurationType">Defines the type of configuration to serialize.</param>
        /// <returns>A <see cref="GameSystemConfiguration"/> definition.</returns>
        public async Task SerializeGameSystemAsync(GameSystemConfiguration gameSystem, Stream stream, ConfigurationType configurationType)
        {
            using var writer = new StreamWriter(stream);
            await ConfigurationSerialization.SerializeRootAsync(gameSystem, writer, configurationType);
        }

        /// <summary>
        /// Serializes a definition to a <see cref="Stream"/>.
        /// </summary>
        /// <param name="roster">The <see cref="Roster"/> instance to serialize.</param>
        /// <param name="stream">The <see cref="Stream"/> to serialize to.</param>
        /// <returns>A <see cref="Roster"/> definition.</returns>
        public async Task SerializeRosterAsync(Roster roster, Stream stream)
        {
            using var writer = new StreamWriter(stream);
            await RosterSerialization.SerializeRootAsync(roster, writer);
        }

        /// <summary>
        /// Serializes a definition to a string.
        /// </summary>
        /// <param name="roster">The <see cref="Roster"/> instance to serialize.</param>
        /// <returns>The serialized definition.</returns>
        public async Task<string> SerializeRosterAsync(Roster roster)
        {
            var builder = new StringBuilder();
            using var writer = new StringWriter(builder);
            await RosterSerialization.SerializeRootAsync(roster, writer);
            return builder.ToString();
        }
    }
}