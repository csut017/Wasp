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
        public static TEntity? FromXml(Stream stream)
        {
            using var reader = new StreamReader(stream);
            var serializer = new XmlSerializer(typeof(TEntity));
            var entity = serializer.Deserialize(reader) as TEntity;
            return entity;
        }

        /// <summary>
        /// Loads an entity definition from an XML string.
        /// </summary>
        /// <param name="xml">The XML containing the definition.</param>
        /// <returns>An entity instance if successful; null otherwise.</returns>
        public static TEntity? FromXml(string xml)
        {
            using var reader = new StringReader(xml);
            var serializer = new XmlSerializer(typeof(TEntity));
            var entity = serializer.Deserialize(reader) as TEntity;
            return entity;
        }

        /// <summary>
        /// Saves the entity as an XML definition.
        /// </summary>
        /// <returns>A string containing the XML.</returns>
        public string AsXml()
        {
            using var writer = new StringWriter();
            var serializer = new XmlSerializer(typeof(TEntity));
            serializer.Serialize(writer, this);
            return writer.ToString();
        }
    }
}