using System.Xml.Serialization;

namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines an entity as serializable.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity.</typeparam>
    public abstract class Serializable<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Loads an entity definition from a <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> containing the definition.</param>
        /// <returns>An entity instance if successful; null otherwise.</returns>
        public static SerializationResult<TEntity> FromXml(Stream stream)
        {
            var result = new SerializationResult<TEntity>();
            using var reader = new StreamReader(stream);
            DeserializeEntity(result, reader);
            return result;
        }

        /// <summary>
        /// Loads an entity definition from an XML string.
        /// </summary>
        /// <param name="xml">The XML containing the definition.</param>
        /// <returns>An entity instance if successful; null otherwise.</returns>
        public static SerializationResult<TEntity> FromXml(string xml)
        {
            var result = new SerializationResult<TEntity>();
            using var reader = new StringReader(xml);
            DeserializeEntity(result, reader);
            return result;
        }

        /// <summary>
        /// Generates the entity as an XML definition.
        /// </summary>
        /// <returns>A string containing the XML.</returns>
        public string GenerateXml()
        {
            using var writer = new StringWriter();
            var serializer = new XmlSerializer(typeof(TEntity));
            serializer.Serialize(writer, this);
            return writer.ToString();
        }

        /// <summary>
        /// Saves the entity as an XML definition.
        /// </summary>
        /// <param name="stream">The stream to save to.</param>
        public void SaveXml(Stream stream)
        {
            using var writer = new StreamWriter(stream);
            var serializer = new XmlSerializer(typeof(TEntity));
            serializer.Serialize(writer, this);
        }

        private static void DeserializeEntity(SerializationResult<TEntity> result, TextReader reader)
        {
            var serializer = new XmlSerializer(typeof(TEntity));
            serializer.UnknownNode += (o, e) => result.UnknownNodes.Add(new UnknownNode(e.LocalName, e.LineNumber, e.LinePosition));
            result.Entity = serializer.Deserialize(reader) as TEntity;
        }
    }
}