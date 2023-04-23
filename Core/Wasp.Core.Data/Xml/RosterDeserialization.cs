using System.Xml;

namespace Wasp.Core.Data.Xml
{
    /// <summary>
    /// Deserializes a definition from an XML format.
    /// </summary>
    internal static class RosterDeserialization
    {
        private static readonly Dictionary<string, Func<XmlReader, Category, Task>> categoryAttributes = new()
        {
            { "entryId", async (reader, category) => category.EntryId = await reader.GetValueAsync() },
            { "id", async (reader, category) => category.Id = await reader.GetValueAsync() },
            { "name", async (reader, category) => category.Name = await reader.GetValueAsync() },
            { "primary", async (reader, category) => category.IsPrimary = await reader.GetValueAsync() == "true" },
        };

        private static readonly Dictionary<string, Func<XmlReader, Characteristic, Task>> characteristicAttributes = new()
        {
            { "name", async (reader, characteristic) => characteristic.Name = await reader.GetValueAsync() },
            { "typeId", async (reader, characteristic) => characteristic.TypeId = await reader.GetValueAsync() },
        };

        private static readonly Dictionary<string, Func<XmlReader, ItemCost, Task>> costAttributes = new()
        {
            { "name", async (reader, cost) => cost.Name = await reader.GetValueAsync() },
            { "typeId", async (reader, cost) => cost.TypeId = await reader.GetValueAsync() },
            { "value", async (reader, cost) => cost.Value = await reader.GetValueAsync() },
        };

        private static readonly Dictionary<string, Func<XmlReader, Force, Task>> forceAttributes = new()
        {
            { "id", async (reader, force) => force.Id = await reader.GetValueAsync() },
            { "catalogueId", async (reader, force) => force.CatalogueId = await reader.GetValueAsync() },
            { "catalogueName", async (reader, force) => force.CatalogueName = await reader.GetValueAsync() },
            { "catalogueRevision", async (reader, force) => force.CatalogueRevision = await reader.GetValueAsync() },
            { "entryId", async (reader, force) => force.EntryId = await reader.GetValueAsync() },
            { "name", async (reader, force) => force.Name = await reader.GetValueAsync() },
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

        private static readonly Dictionary<string, Func<XmlReader, Roster, Task>> rosterAttributes = new()
        {
            { "battleScribeVersion", async (reader, roster) => roster.BattleScribeVersion = await reader.GetValueAsync() },
            { "gameSystemId", async (reader, roster) => roster.GameSystemId = await reader.GetValueAsync() },
            { "gameSystemName", async (reader, roster) => roster.GameSystemName = await reader.GetValueAsync() },
            { "gameSystemRevision", async (reader, roster) => roster.GameSystemRevision = await reader.GetValueAsync() },
            { "id", async (reader, roster) => roster.Id = await reader.GetValueAsync() },
            { "name", async (reader, roster) => roster.Name = await reader.GetValueAsync() },
        };

        private static readonly Dictionary<string, Func<XmlReader, Selection, Task>> selectionAttributes = new()
        {
            { "customName", async (reader, selection) => selection.CustomName = await reader.GetValueAsync() },
            { "entryId", async (reader, selection) => selection.EntryId = await reader.GetValueAsync() },
            { "entryGroupId", async (reader, selection) => selection.EntryGroupId = await reader.GetValueAsync() },
            { "id", async (reader, selection) => selection.Id = await reader.GetValueAsync() },
            { "name", async (reader, selection) => selection.Name = await reader.GetValueAsync() },
            { "number", async (reader, selection) => selection.Number = await reader.GetValueAsync() },
            { "page", async (reader, selection) => selection.Page = await reader.GetValueAsync() },
            { "publicationId",  async (reader, selection) => selection.PublicationId = await reader.GetValueAsync() },
            { "type", async (reader, selection) => selection.Type = await reader.GetValueAsync() },
        };

        /// <summary>
        /// Deserialize the root level element.
        /// </summary>
        /// <param name="reader">The <see cref="TextReader"/> containing the definition to deserialize.</param>
        /// <returns>A <see cref="Roster"/> containing the deserialized data.</returns>
        public static async Task<Roster> DeserializeRootAsync(TextReader reader)
        {
            var roster = new Roster();
            var settings = new XmlReaderSettings { Async = true };
            using var xmlReader = XmlReader.Create(reader, settings);
            while (await xmlReader.ReadAsync())
            {
                await xmlReader.MoveToContentAsync();
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        // Should only have a roster at the root level
                        if (xmlReader.Name != "roster") throw new Exception($"Invalid roster definition: expected a roster node, found {xmlReader.Name} instead");
                        await DeserializeRosterAsync(xmlReader, roster);
                        break;

                    default:
                        // Anything else is an error
                        throw new Exception($"Unexpected node type: {xmlReader.NodeType} ({xmlReader.Name})");
                }
            }

            return roster;
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

        /// <summary>
        /// Deserialize a force.
        /// </summary>
        /// <param name="xmlReader">The <see cref="XmlReader"/> containing the definition to deserialize.</param>
        /// <param name="roster">The <see cref="Roster"/> to populate.</param>
        private static async Task DeserializeForceAsync(XmlReader xmlReader, Roster roster)
        {
            var name = xmlReader.Name;
            var isReading = !xmlReader.IsEmptyElement;
            var force = new Force();
            await xmlReader.DeserializeAttributesAsync(force, forceAttributes);
            roster.Forces ??= new List<Force>();
            roster.Forces.Add(force);

            while (isReading && await xmlReader.ReadAsync())
            {
                await xmlReader.MoveToContentAsync();
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (xmlReader.Name)
                        {
                            case "categories":
                                force.Categories ??= new List<Category>();
                                await xmlReader.DeserializeArrayAsync(
                                    force,
                                    "category",
                                    async (reader, _) => await reader.DeserializeSingleItemAsync(force.Categories, categoryAttributes));
                                break;

                            case "publications":
                                force.Publications ??= new List<Publication>();
                                await xmlReader.DeserializeArrayAsync(
                                    force,
                                    "publication",
                                    async (reader, _) => await reader.DeserializeSingleItemAsync(force.Publications, CommonDeserialization.PublicationAttributes));
                                break;

                            case "rules":
                                await xmlReader.DeserializeArrayAsync(
                                    force,
                                    "rule",
                                    CommonDeserialization.DeserializeRuleAsync);
                                break;

                            case "selections":
                                await xmlReader.DeserializeArrayAsync(
                                    force,
                                    "selection",
                                    DeserializeSelectionAsync);
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
        /// <param name="selection">The <see cref="Selection"/> to populate.</param>
        private static async Task DeserializeProfileAsync(XmlReader xmlReader, Selection selection)
        {
            var name = xmlReader.Name;
            var isReading = !xmlReader.IsEmptyElement;
            var profile = new Profile();
            await xmlReader.DeserializeAttributesAsync(profile, profileAttributes);
            selection.Profiles ??= new List<Profile>();
            selection.Profiles.Add(profile);

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
        /// Deserialize the roster details.
        /// </summary>
        /// <param name="xmlReader">The <see cref="XmlReader"/> containing the definition to deserialize.</param>
        /// <param name="roster">The <see cref="Roster"/> to populate.</param>
        private static async Task DeserializeRosterAsync(XmlReader xmlReader, Roster roster)
        {
            var name = xmlReader.Name;
            var isReading = !xmlReader.IsEmptyElement;
            await xmlReader.DeserializeAttributesAsync(roster, rosterAttributes);

            while (isReading && await xmlReader.ReadAsync())
            {
                await xmlReader.MoveToContentAsync();
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (xmlReader.Name)
                        {
                            case "costs":
                                roster.Costs ??= new List<ItemCost>();
                                await xmlReader.DeserializeArrayAsync(
                                    roster,
                                    "cost",
                                    async (reader, _) => await reader.DeserializeSingleItemAsync(roster.Costs, costAttributes));
                                break;

                            case "costLimits":
                                roster.CostLimits ??= new List<ItemCost>();
                                await xmlReader.DeserializeArrayAsync(
                                    roster,
                                    "costLimit",
                                    async (reader, _) => await reader.DeserializeSingleItemAsync(roster.CostLimits, costAttributes));
                                break;

                            case "forces":
                                await xmlReader.DeserializeArrayAsync(roster, "force", DeserializeForceAsync);
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
        /// Deserialize a selection.
        /// </summary>
        /// <param name="xmlReader">The <see cref="XmlReader"/> containing the definition to deserialize.</param>
        /// <param name="parent">The <see cref="ISelectionParent"/> to populate.</param>
        private static async Task DeserializeSelectionAsync(XmlReader xmlReader, ISelectionParent parent)
        {
            var name = xmlReader.Name;
            var isReading = !xmlReader.IsEmptyElement;
            var selection = new Selection();
            await xmlReader.DeserializeAttributesAsync(selection, selectionAttributes);
            parent.Selections ??= new List<Selection>();
            parent.Selections.Add(selection);

            while (isReading && await xmlReader.ReadAsync())
            {
                await xmlReader.MoveToContentAsync();
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (xmlReader.Name)
                        {
                            case "categories":
                                selection.Categories ??= new List<Category>();
                                await xmlReader.DeserializeArrayAsync(
                                    selection,
                                    "category",
                                    async (reader, _) => await reader.DeserializeSingleItemAsync(selection.Categories, categoryAttributes));
                                break;

                            case "costs":
                                selection.Costs ??= new List<ItemCost>();
                                await xmlReader.DeserializeArrayAsync(
                                    selection,
                                    "cost",
                                    async (reader, _) => await reader.DeserializeSingleItemAsync(selection.Costs, costAttributes));
                                break;

                            case "customNotes":
                                selection.CustomNotes = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            case "profiles":
                                await xmlReader.DeserializeArrayAsync(
                                    selection,
                                    "profile",
                                    DeserializeProfileAsync);
                                break;

                            case "rules":
                                await xmlReader.DeserializeArrayAsync(
                                    selection,
                                    "rule",
                                    CommonDeserialization.DeserializeRuleAsync);
                                break;

                            case "selections":
                                await xmlReader.DeserializeArrayAsync(
                                    selection,
                                    "selection",
                                    DeserializeSelectionAsync);
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
    }
}