using System.Text;

namespace Wasp.Core.Data.Xml
{
    /// <summary>
    /// Defines the XML format provider for a BattleSribe data format.
    /// </summary>
    public class Format
        : IFormatProvider
    {
        /// <summary>
        /// Deserializes a definition from a string.
        /// </summary>
        /// <param name="definition">The definition to deserialize.</param>
        /// <returns>A <see cref="Roster"/> definition.</returns>
        public async Task<Roster> DeserializeAsync(string definition)
        {
            using var reader = new StringReader(definition);
            return await Deserialization.DeserializeRootAsync(reader);
        }

        /// <summary>
        /// Deserializes a definition from a <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> containing the definition to deserialize.</param>
        /// <returns>A <see cref="Roster"/> definition.</returns>
        public async Task<Roster> DeserializeAsync(Stream stream)
        {
            using var reader = new StreamReader(stream);
            return await Deserialization.DeserializeRootAsync(reader);
        }

        /// <summary>
        /// Serializes a definition to a <see cref="Stream"/>.
        /// </summary>
        /// <param name="roster">The <see cref="Roster"/> instance to serialize.</param>
        /// <param name="stream">The <see cref="Stream"/> to serialize to.</param>
        /// <returns>A <see cref="Roster"/> definition.</returns>
        public async Task SerializeAsync(Roster roster, Stream stream)
        {
            using var writer = new StreamWriter(stream);
            await Serialization.SerializeRootAsync(roster, writer);
        }

        /// <summary>
        /// Serializes a definition to a string.
        /// </summary>
        /// <param name="roster">The <see cref="Roster"/> instance to serialize.</param>
        /// <returns>The serialized definition.</returns>
        public async Task<string> SerializeAsync(Roster roster)
        {
            var builder = new StringBuilder();
            using var writer = new StringWriter(builder);
            await Serialization.SerializeRootAsync(roster, writer);
            return builder.ToString();
        }
    }
}