using System.Xml;

namespace Wasp.Core.Data.Xml
{
    /// <summary>
    /// Helper methods that are used by multiple deserisaliztion definitions.
    /// </summary>
    internal static class CommonDeserialization
    {
        public static readonly Dictionary<string, Func<XmlReader, Publication, Task>> PublicationAttributes = new()
        {
            { "id", async (reader, publication) => publication.Id = await reader.GetValueAsync() },
            { "name", async (reader, publication) => publication.FullName = await reader.GetValueAsync() },
            { "publicationDate", async (reader, publication) => publication.PublicationDate = await reader.GetValueAsync() },
            { "publisher", async (reader, publication) => publication.PublisherName = await reader.GetValueAsync() },
            { "publisherUrl", async (reader, publication) => publication.PublisherUrl = await reader.GetValueAsync() },
            { "shortName", async (reader, publication) => publication.ShortName = await reader.GetValueAsync() },
        };

        /// <summary>
        /// Deserialize the items in an array.
        /// </summary>
        /// <param name="xmlReader">The <see cref="XmlReader"/> containing the definition to deserialize.</param>
        /// <param name="item">The item to populate.</param>
        /// <param name="arrayName">The name of the array.</param>
        /// <param name="itemName">The name of each item in the array.</param>
        /// <param name="itemLoader">The loader for populating the item.</param>
        public static async Task DeserializeArrayAsync<TItem>(this XmlReader xmlReader, TItem item, string arrayName, string itemName, Func<XmlReader, TItem, Task> itemLoader)
            where TItem : class
        {
            var isReading = !xmlReader.IsEmptyElement;
            while (isReading && await xmlReader.ReadAsync())
            {
                await xmlReader.MoveToContentAsync();
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (xmlReader.Name == itemName)
                        {
                            await itemLoader(xmlReader, item);
                        }
                        else
                        {
                            // Anything else is an error
                            throw new Exception($"Unexpected element: {xmlReader.Name}");
                        }
                        break;

                    case XmlNodeType.EndElement:
                        // Make sure we've got the correct end element
                        if (xmlReader.Name != arrayName) throw new Exception($"Unexpected end node: expected {arrayName}, found {xmlReader.Name}");
                        isReading = false;
                        break;

                    default:
                        // Anything else is an error
                        throw new Exception($"Unexpected node type: {xmlReader.NodeType} ({xmlReader.Name})");
                }
            }
        }

        /// <summary>
        /// Deserialize the attributes of an item using a dictionary of setters.
        /// </summary>
        /// <typeparam name="TItem">The type of item to populate.</typeparam>
        /// <param name="xmlReader">The <see cref="XmlReader"/> containing the definition to deserialize.</param>
        /// <param name="item">The item to populate.</param>
        /// <param name="setters">The dictionary of setters.</param>
        /// <param name="name">The name of the item.</param>
        public static async Task DeserializeAttributesAsync<TItem>(this XmlReader xmlReader, TItem item, Dictionary<string, Func<XmlReader, TItem, Task>> setters, string name)
            where TItem : class
        {
            if (xmlReader.MoveToFirstAttribute())
            {
                do
                {
                    if (!string.IsNullOrEmpty(xmlReader.NamespaceURI) && !string.Equals(xmlReader.NamespaceURI, Constants.XmlNamespace)) continue;
                    if (!setters.TryGetValue(xmlReader.Name, out var setter)) throw new Exception($"Unknown attribute '{xmlReader.Name}' on {name} element");
                    await setter(xmlReader, item);
                } while (xmlReader.MoveToNextAttribute());
            }
        }

        /// <summary>
        /// Deserializes an item.
        /// </summary>
        /// <param name="xmlReader">The <see cref="XmlReader"/> containing the definition to deserialize.</param>
        /// <param name="parent">The parent list.</param>
        /// <param name="name">The name of the item to deserialize.</param>
        /// <param name="setters">The attribute setters to use.</param>
        public static async Task DeserializeSingleItemAsync<TItem>(this XmlReader xmlReader, IList<TItem> parent, string name, Dictionary<string, Func<XmlReader, TItem, Task>> setters)
            where TItem : class, new()
        {
            var isReading = !xmlReader.IsEmptyElement;
            var item = new TItem();
            await xmlReader.DeserializeAttributesAsync(item, setters, name);
            parent.Add(item);

            while (isReading && await xmlReader.ReadAsync())
            {
                await xmlReader.MoveToContentAsync();
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.EndElement:
                        // Make sure we've got the correct end element
                        if (xmlReader.Name != name) throw new Exception($"Unexpected end node: expected {name}, found {xmlReader.Name}");
                        isReading = false;
                        break;

                    default:
                        // Anything else is an error
                        throw new Exception($"Unexpected node type: {xmlReader.NodeType} ({xmlReader.Name})");
                }
            }
        }
    }
}