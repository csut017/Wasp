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

        private static readonly Dictionary<string, Func<XmlReader, Characteristic, Task>> characteristicAttributes = new()
        {
            { "name", async (reader, characteristic) => characteristic.Name = await reader.GetValueAsync() },
            { "typeId", async (reader, characteristic) => characteristic.TypeId = await reader.GetValueAsync() },
        };

        private static readonly Dictionary<string, Func<XmlReader, ConditionGroup, Task>> conditionGroupAttributes = new()
        {
            { "type", async (reader, conditionGroup) => conditionGroup.Type = await reader.GetValueAsync() },
        };

        private static readonly Dictionary<string, Func<XmlReader, Constraint, Task>> constraintAttributes = new()
        {
            { "childId", async (reader, categoryEntry) => categoryEntry.ChildId = await reader.GetValueAsync() },
            { "field", async (reader, categoryEntry) => categoryEntry.Field = await reader.GetValueAsync() },
            { "id", async (reader, categoryEntry) => categoryEntry.Id = await reader.GetValueAsync() },
            { "includeChildForces", async (reader, categoryEntry) => categoryEntry.IncludeChildForces = await reader.GetValueAsync() == "true" },
            { "includeChildSelections", async (reader, categoryEntry) => categoryEntry.IncludeChildSelections = await reader.GetValueAsync() == "true" },
            { "percentValue", async (reader, categoryEntry) => categoryEntry.IsPercentage = await reader.GetValueAsync() == "true" },
            { "repeats", async (reader, categoryEntry) => categoryEntry.NumberOfRepeats = await reader.GetValueAsync() },
            { "roundUp", async (reader, categoryEntry) => categoryEntry.ShouldRoundUp = HandleTrueFalse(await reader.GetValueAsync()) },
            { "scope", async (reader, categoryEntry) => categoryEntry.Scope = await reader.GetValueAsync() },
            { "shared", async (reader, categoryEntry) => categoryEntry.IsShared = await reader.GetValueAsync() == "true" },
            { "type", async (reader, categoryEntry) => categoryEntry.Type = await reader.GetValueAsync() },
            { "value", async (reader, categoryEntry) => categoryEntry.Value = await reader.GetValueAsync() },
        };

        private static readonly Dictionary<string, Func<XmlReader, Modifier, Task>> modifierAttributes = new()
        {
            { "field", async (reader, modifier) => modifier.Field = await reader.GetValueAsync() },
            { "type", async (reader, modifier) => modifier.Type = await reader.GetValueAsync() },
            { "value", async (reader, modifier) => modifier.Value = await reader.GetValueAsync() },
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

        private static readonly Dictionary<string, Func<XmlReader, Publication, Task>> publicationAttributes = new()
        {
            { "id", async (reader, publication) => publication.Id = await reader.GetValueAsync() },
            { "name", async (reader, publication) => publication.FullName = await reader.GetValueAsync() },
            { "publicationDate", async (reader, publication) => publication.PublicationDate = await reader.GetValueAsync() },
            { "publisher", async (reader, publication) => publication.PublisherName = await reader.GetValueAsync() },
            { "publisherUrl", async (reader, publication) => publication.PublisherUrl = await reader.GetValueAsync() },
            { "shortName", async (reader, publication) => publication.ShortName = await reader.GetValueAsync() },
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
                    if (!string.IsNullOrEmpty(xmlReader.NamespaceURI)) continue;
                    if (!setters.TryGetValue(xmlReader.Name, out var setter)) throw xmlReader.GenerateUnexpectedAttributeException(name);
                    await setter(xmlReader, item);
                } while (xmlReader.MoveToNextAttribute());
            }
        }

        /// <summary>
        /// Deserialize a constraint.
        /// </summary>
        /// <param name="xmlReader">The <see cref="XmlReader"/> containing the definition to deserialize.</param>
        /// <param name="parent">The <see cref="List{Constraint}"/> to populate.</param>
        public static async Task DeserializeConstraint(XmlReader xmlReader, List<Constraint>? parent)
        {
            var name = xmlReader.Name;
            var isReading = !xmlReader.IsEmptyElement;
            var item = new Constraint();
            await xmlReader.DeserializeAttributesAsync(item, constraintAttributes);
            if (parent == null) throw new Exception("Constraints has not been initialised");
            parent.Add(item);

            while (isReading && await xmlReader.ReadAsync())
            {
                await xmlReader.MoveToContentAsync();
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (xmlReader.Name)
                        {
                            case "comment":
                                item.Comment = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            default:
                                // Anything else is an error
                                throw xmlReader.GenerateUnexpectedElementException();
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
        /// Deserialize a modifier.
        /// </summary>
        /// <param name="xmlReader">The <see cref="XmlReader"/> containing the definition to deserialize.</param>
        /// <param name="parent">The <see cref="GameSystemConfiguration"/> to populate.</param>
        public static async Task DeserializeModifierAsync(XmlReader xmlReader, List<Modifier>? parent)
        {
            var name = xmlReader.Name;
            var isReading = !xmlReader.IsEmptyElement;
            var item = new Modifier();
            await xmlReader.DeserializeAttributesAsync(item, modifierAttributes);
            if (parent == null) throw new Exception("Modifiers has not been initialised");
            parent.Add(item);

            while (isReading && await xmlReader.ReadAsync())
            {
                await xmlReader.MoveToContentAsync();
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (xmlReader.Name)
                        {
                            case "comment":
                                item.Comment = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            case "conditionGroups":
                                item.ConditionGroups ??= new List<ConditionGroup>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "conditionGroup",
                                    async (reader, _) => await DeserializeConditionGroupAsync(reader, item.ConditionGroups));
                                break;

                            case "conditions":
                                item.Conditions ??= new List<Constraint>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "condition",
                                    async (reader, _) => await DeserializeConstraint(reader, item.Conditions));
                                break;

                            case "repeats":
                                item.Repeats ??= new List<Constraint>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "repeat",
                                    async (reader, _) => await DeserializeConstraint(reader, item.Repeats));
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
        /// Deserialize a profile.
        /// </summary>
        /// <param name="xmlReader">The <see cref="XmlReader"/> containing the definition to deserialize.</param>
        /// <param name="parent">The <see cref="List{Profile}"/> to populate.</param>
        public static async Task DeserializeProfileAsync(XmlReader xmlReader, List<Profile>? parent)
        {
            var name = xmlReader.Name;
            var isReading = !xmlReader.IsEmptyElement;
            var item = new Profile();
            await xmlReader.DeserializeAttributesAsync(item, profileAttributes);
            if (parent == null) throw new Exception("Profiles has not been set");
            parent.Add(item);

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
                                    item,
                                    "characteristic",
                                    DeserializeCharacteristicAsync);
                                break;

                            case "modifiers":
                                item.Modifiers ??= new List<Modifier>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "modifier",
                                    async (reader, _) => await DeserializeModifierAsync(reader, item.Modifiers));
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
        /// Deserialize a publication.
        /// </summary>
        /// <param name="xmlReader">The <see cref="XmlReader"/> containing the definition to deserialize.</param>
        /// <param name="parent">The <see cref="List{Publication}"/> to populate.</param>
        public static async Task DeserializePublication(XmlReader xmlReader, List<Publication>? parent)
        {
            var name = xmlReader.Name;
            var isReading = !xmlReader.IsEmptyElement;
            var item = new Publication();
            await xmlReader.DeserializeAttributesAsync(item, publicationAttributes);
            if (parent == null) throw new Exception("Publications has not been initialised");
            parent.Add(item);

            while (isReading && await xmlReader.ReadAsync())
            {
                await xmlReader.MoveToContentAsync();
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (xmlReader.Name)
                        {
                            case "comment":
                                item.Comment = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            default:
                                // Anything else is an error
                                throw xmlReader.GenerateUnexpectedElementException();
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
        /// <param name="parent">The <see cref="List{Rule}"/> to populate.</param>
        public static async Task DeserializeRuleAsync(XmlReader xmlReader, List<Rule>? parent)
        {
            var name = xmlReader.Name;
            var isReading = !xmlReader.IsEmptyElement;
            var item = new Rule();
            await xmlReader.DeserializeAttributesAsync(item, ruleAttributes);
            if (parent == null) throw new Exception("Rules has not been set");
            parent.Add(item);

            while (isReading && await xmlReader.ReadAsync())
            {
                await xmlReader.MoveToContentAsync();
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (xmlReader.Name)
                        {
                            case "description":
                                item.Description = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            case "modifiers":
                                item.Modifiers ??= new List<Modifier>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "modifier",
                                    async (reader, _) => await DeserializeModifierAsync(reader, item.Modifiers));
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

        public static Exception GenerateUnexpectedAttributeException(this XmlReader xmlReader, string name)
        {
            int? lineNumber = null;
            int? linePosition = null;
            if ((xmlReader is IXmlLineInfo lineInfo) && lineInfo.HasLineInfo())
            {
                lineNumber = lineInfo.LineNumber;
                linePosition = lineInfo.LinePosition;
            }

            var message = $"Unknown attribute '{xmlReader.Name}' found on element {name}" + (lineNumber.HasValue ? $" at line {lineNumber}" : string.Empty);
            var error = new DeserializationException(message, lineNumber, linePosition);
            return error;
        }

        public static Exception GenerateUnexpectedElementException(this XmlReader xmlReader)
        {
            int? lineNumber = null;
            int? linePosition = null;
            if ((xmlReader is IXmlLineInfo lineInfo) && lineInfo.HasLineInfo())
            {
                lineNumber = lineInfo.LineNumber;
                linePosition = lineInfo.LinePosition;
            }

            var message = $"Unexpected element '{xmlReader.Name}'" + (lineNumber.HasValue ? $" at line {lineNumber}" : string.Empty);
            var error = new DeserializationException(message, lineNumber, linePosition);
            return error;
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
            var item = new Characteristic();
            await xmlReader.DeserializeAttributesAsync(item, characteristicAttributes);
            profile.Characteristics ??= new List<Characteristic>();
            profile.Characteristics.Add(item);

            while (isReading && await xmlReader.ReadAsync())
            {
                await xmlReader.MoveToContentAsync();
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Text:
                        item.Value = xmlReader.Value;
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
        /// Deserialize a condition group.
        /// </summary>
        /// <param name="xmlReader">The <see cref="XmlReader"/> containing the definition to deserialize.</param>
        /// <param name="parent">The <see cref="GameSystemConfiguration"/> to populate.</param>
        private static async Task DeserializeConditionGroupAsync(XmlReader xmlReader, List<ConditionGroup>? parent)
        {
            var name = xmlReader.Name;
            var isReading = !xmlReader.IsEmptyElement;
            var item = new ConditionGroup();
            await xmlReader.DeserializeAttributesAsync(item, conditionGroupAttributes);
            if (parent == null) throw new Exception("ConditionGroups has not been initialised");
            parent.Add(item);

            while (isReading && await xmlReader.ReadAsync())
            {
                await xmlReader.MoveToContentAsync();
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (xmlReader.Name)
                        {
                            case "comment":
                                item.Comment = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            case "conditionGroups":
                                item.ConditionGroups ??= new List<ConditionGroup>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "conditionGroup",
                                    async (reader, _) => await DeserializeConditionGroupAsync(reader, item.ConditionGroups));
                                break;

                            case "conditions":
                                item.Conditions ??= new List<Constraint>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "condition",
                                    async (reader, _) => await DeserializeConstraint(reader, item.Conditions));
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

        private static bool? HandleTrueFalse(string value)
        {
            return string.IsNullOrEmpty(value) ? null : value == "true";
        }
    }
}