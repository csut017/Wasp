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

        private static readonly Dictionary<string, Func<XmlReader, Rule, Task>> ruleAttributes = new()
        {
            { "hidden", async (reader, rule) => rule.IsHidden = await reader.GetValueAsync() == "true" },
            { "id", async (reader, rule) => rule.Id = await reader.GetValueAsync() },
            { "name", async (reader, rule) => rule.Name = await reader.GetValueAsync() },
            { "page", async (reader, rule) => rule.Page = await reader.GetValueAsync() },
            { "publicationId", async (reader, rule) => rule.PublicationId = await reader.GetValueAsync() },
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
            var isReading = !xmlReader.IsEmptyElement;
            var characteristic = new Characteristic();
            await xmlReader.DeserializeAttributesAsync(characteristic, characteristicAttributes, "profile");
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
                        if (xmlReader.Name != "characteristic") throw new Exception($"Unexpected end node: expected characteristic, found {xmlReader.Name}");
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
            var isReading = !xmlReader.IsEmptyElement;
            var force = new Force();
            await xmlReader.DeserializeAttributesAsync(force, forceAttributes, "force");
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
                                    "categories",
                                    "category",
                                    async (reader, _) => await reader.DeserializeSingleItemAsync(force.Categories, "category", categoryAttributes));
                                break;

                            case "publications":
                                force.Publications ??= new List<Publication>();
                                await xmlReader.DeserializeArrayAsync(
                                    force,
                                    "publications",
                                    "publication",
                                    async (reader, _) => await reader.DeserializeSingleItemAsync(force.Publications, "publication", CommonDeserialization.PublicationAttributes));
                                break;

                            case "rules":
                                await xmlReader.DeserializeArrayAsync(
                                    force,
                                    "rules",
                                    "rule",
                                    DeserializeRuleAsync);
                                break;

                            case "selections":
                                await xmlReader.DeserializeArrayAsync(
                                    force,
                                    "selections",
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
                        if (xmlReader.Name != "force") throw new Exception($"Unexpected end node: expected force, found {xmlReader.Name}");
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
            var isReading = !xmlReader.IsEmptyElement;
            var profile = new Profile();
            await xmlReader.DeserializeAttributesAsync(profile, profileAttributes, "profile");
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
                                    "characteristics",
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
                        if (xmlReader.Name != "profile") throw new Exception($"Unexpected end node: expected profile, found {xmlReader.Name}");
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
            var isReading = !xmlReader.IsEmptyElement;
            await xmlReader.DeserializeAttributesAsync(roster, rosterAttributes, "roster");

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
                                    "costs",
                                    "cost",
                                    async (reader, _) => await reader.DeserializeSingleItemAsync(roster.Costs, "cost", costAttributes));
                                break;

                            case "costLimits":
                                roster.CostLimits ??= new List<ItemCost>();
                                await xmlReader.DeserializeArrayAsync(
                                    roster,
                                    "costLimits",
                                    "costLimit",
                                    async (reader, _) => await reader.DeserializeSingleItemAsync(roster.CostLimits, "costLimit", costAttributes));
                                break;

                            case "forces":
                                await xmlReader.DeserializeArrayAsync(roster, "forces", "force", DeserializeForceAsync);
                                break;

                            default:
                                // Anything else is an error
                                throw new Exception($"Unexpected element: {xmlReader.Name}");
                        }
                        break;

                    case XmlNodeType.EndElement:
                        // Make sure we've got the correct end element
                        if (xmlReader.Name != "roster") throw new Exception($"Unexpected end node: expected roster, found {xmlReader.Name}");
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
        private static async Task DeserializeRuleAsync(XmlReader xmlReader, IRulesParent parent)
        {
            var isReading = !xmlReader.IsEmptyElement;
            var rule = new Rule();
            await xmlReader.DeserializeAttributesAsync(rule, ruleAttributes, "rule");
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
                        if (xmlReader.Name != "rule") throw new Exception($"Unexpected end node: expected rule, found {xmlReader.Name}");
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
            var isReading = !xmlReader.IsEmptyElement;
            var selection = new Selection();
            await xmlReader.DeserializeAttributesAsync(selection, selectionAttributes, "selection");
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
                                    "categories",
                                    "category",
                                    async (reader, _) => await reader.DeserializeSingleItemAsync(selection.Categories, "category", categoryAttributes));
                                break;

                            case "costs":
                                selection.Costs ??= new List<ItemCost>();
                                await xmlReader.DeserializeArrayAsync(
                                    selection,
                                    "costs",
                                    "cost",
                                    async (reader, _) => await reader.DeserializeSingleItemAsync(selection.Costs, "cost", costAttributes));
                                break;

                            case "customNotes":
                                selection.CustomNotes = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            case "profiles":
                                await xmlReader.DeserializeArrayAsync(
                                    selection,
                                    "profiles",
                                    "profile",
                                    DeserializeProfileAsync);
                                break;

                            case "rules":
                                await xmlReader.DeserializeArrayAsync(
                                    selection,
                                    "rules",
                                    "rule",
                                    DeserializeRuleAsync);
                                break;

                            case "selections":
                                await xmlReader.DeserializeArrayAsync(
                                    selection,
                                    "selections",
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
                        if (xmlReader.Name != "selection") throw new Exception($"Unexpected end node: expected selection, found {xmlReader.Name}");
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