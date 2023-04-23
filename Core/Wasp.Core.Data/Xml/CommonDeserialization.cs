using System.Xml;

namespace Wasp.Core.Data.Xml
{
    /// <summary>
    /// Helper methods that are used by multiple deserisaliztion definitions.
    /// </summary>
    internal static class CommonDeserialization
    {
        public static readonly Dictionary<string, Func<XmlReader, ItemCost, Task>> CostAttributes = new()
        {
            { "name", async (reader, cost) => cost.Name = await reader.GetValueAsync() },
            { "typeId", async (reader, cost) => cost.TypeId = await reader.GetValueAsync() },
            { "value", async (reader, cost) => cost.Value = await reader.GetValueAsync() },
        };

        public static readonly Dictionary<string, Func<XmlReader, Publication, Task>> PublicationAttributes = new()
        {
            { "id", async (reader, publication) => publication.Id = await reader.GetValueAsync() },
            { "name", async (reader, publication) => publication.FullName = await reader.GetValueAsync() },
            { "publicationDate", async (reader, publication) => publication.PublicationDate = await reader.GetValueAsync() },
            { "publisher", async (reader, publication) => publication.PublisherName = await reader.GetValueAsync() },
            { "publisherUrl", async (reader, publication) => publication.PublisherUrl = await reader.GetValueAsync() },
            { "shortName", async (reader, publication) => publication.ShortName = await reader.GetValueAsync() },
        };

        private static readonly Dictionary<string, Func<XmlReader, Characteristic, Task>> characteristicAttributes = new()
        {
            { "name", async (reader, characteristic) => characteristic.Name = await reader.GetValueAsync() },
            { "typeId", async (reader, characteristic) => characteristic.TypeId = await reader.GetValueAsync() },
        };

        private static readonly Dictionary<string, Func<XmlReader, Profile, Task>> profileAttributes = new()
        {
            { "hidden", async (reader, profile) => profile.IsHidden = await reader.GetValueAsync() == "true" },
            { "id", async (reader, profile) => profile.Id = await reader.GetValueAsync() },
            { "name", async (reader, profile) => profile.Name = await reader.GetValueAsync() },
            { "page", async (reader, profile) => profile.Page = await reader.GetValueAsync() },
            { "publicationId", async (reader, profile) => profile.PublicationId = await reader.GetValueAsync() },
            { "typeId", async (reader, profile) => profile.TypeId = await reader.GetValueAsync() },
            { "typeName", async (reader, profile) => profile.TypeName = await reader.GetValueAsync() },
        };

        private static readonly Dictionary<string, Func<XmlReader, Rule, Task>> ruleAttributes = new()
        {
            { "hidden", async (reader, rule) => rule.IsHidden = await reader.GetValueAsync() == "true" },
            { "id", async (reader, rule) => rule.Id = await reader.GetValueAsync() },
            { "name", async (reader, rule) => rule.Name = await reader.GetValueAsync() },
            { "page", async (reader, rule) => rule.Page = await reader.GetValueAsync() },
            { "publicationId", async (reader, rule) => rule.PublicationId = await reader.GetValueAsync() },
        };

        /// <summary>
        /// Deserialize the items in an array.
        /// </summary>
        /// <param name="xmlReader">The <see cref="XmlReader"/> containing the definition to deserialize.</param>
        /// <param name="item">The item to populate.</param>
        /// <param name="itemName">The name of each item in the array.</param>
        /// <param name="itemLoader">The loader for populating the item.</param>
        public static async Task DeserializeArrayAsync<TItem>(this XmlReader xmlReader, TItem item, string itemName, Func<XmlReader, TItem, Task> itemLoader)
            where TItem : class
        {
            var name = xmlReader.Name;
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
                        if (xmlReader.Name != name) throw new Exception($"Unexpected end node: expected {name}, found {xmlReader.Name}");
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
        public static async Task DeserializeAttributesAsync<TItem>(this XmlReader xmlReader, TItem item, Dictionary<string, Func<XmlReader, TItem, Task>> setters)
            where TItem : class
        {
            var name = xmlReader.Name;
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
        /// Deserialize a profile.
        /// </summary>
        /// <param name="xmlReader">The <see cref="XmlReader"/> containing the definition to deserialize.</param>
        /// <param name="parent">The <see cref="IProfilesParent"/> to populate.</param>
        public static async Task DeserializeProfileAsync(XmlReader xmlReader, IProfilesParent parent)
        {
            var name = xmlReader.Name;
            var isReading = !xmlReader.IsEmptyElement;
            var profile = new Profile();
            await xmlReader.DeserializeAttributesAsync(profile, profileAttributes);
            parent.Profiles ??= new List<Profile>();
            parent.Profiles.Add(profile);

            while (isReading && await xmlReader.ReadAsync())
            {
                await xmlReader.MoveToContentAsync();
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (xmlReader.Name)
                        {
                            case "characteristics":
                                await xmlReader.DeserializeArrayAsync(
                                    profile,
                                    "characteristic",
                                    DeserializeCharacteristicAsync);
                                break;

                            default:
                                // Anything else is an error
                                throw new Exception($"Unexpected element: {xmlReader.Name}");
                        }
                        break;

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

        /// <summary>
        /// Deserialize a rule.
        /// </summary>
        /// <param name="xmlReader">The <see cref="XmlReader"/> containing the definition to deserialize.</param>
        /// <param name="parent">The <see cref="IRulesParent"/> to populate.</param>
        public static async Task DeserializeRuleAsync(XmlReader xmlReader, IRulesParent parent)
        {
            var name = xmlReader.Name;
            var isReading = !xmlReader.IsEmptyElement;
            var rule = new Rule();
            await xmlReader.DeserializeAttributesAsync(rule, ruleAttributes);
            parent.Rules ??= new List<Rule>();
            parent.Rules.Add(rule);

            while (isReading && await xmlReader.ReadAsync())
            {
                await xmlReader.MoveToContentAsync();
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (xmlReader.Name)
                        {
                            case "description":
                                rule.Description = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            default:
                                // Anything else is an error
                                throw new Exception($"Unexpected element: {xmlReader.Name}");
                        }
                        break;

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

        /// <summary>
        /// Deserializes an item.
        /// </summary>
        /// <param name="xmlReader">The <see cref="XmlReader"/> containing the definition to deserialize.</param>
        /// <param name="parent">The parent list.</param>
        /// <param name="setters">The attribute setters to use.</param>
        public static async Task DeserializeSingleItemAsync<TItem>(this XmlReader xmlReader, IList<TItem> parent, Dictionary<string, Func<XmlReader, TItem, Task>> setters)
            where TItem : class, new()
        {
            var name = xmlReader.Name;
            var isReading = !xmlReader.IsEmptyElement;
            var item = new TItem();
            await xmlReader.DeserializeAttributesAsync(item, setters);
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

        /// <summary>
        /// Deserialize a characteristic.
        /// </summary>
        /// <param name="xmlReader">The <see cref="XmlReader"/> containing the definition to deserialize.</param>
        /// <param name="profile">The <see cref="Profile"/> to populate.</param>
        private static async Task DeserializeCharacteristicAsync(XmlReader xmlReader, Profile profile)
        {
            var name = xmlReader.Name;
            var isReading = !xmlReader.IsEmptyElement;
            var characteristic = new Characteristic();
            await xmlReader.DeserializeAttributesAsync(characteristic, characteristicAttributes);
            profile.Characteristics ??= new List<Characteristic>();
            profile.Characteristics.Add(characteristic);

            while (isReading && await xmlReader.ReadAsync())
            {
                await xmlReader.MoveToContentAsync();
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Text:
                        characteristic.Value = xmlReader.Value;
                        break;

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