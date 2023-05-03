using System.Xml;

namespace Wasp.Core.Data.Xml
{
    internal class ConfigurationDeserialization
    {
        private static readonly Dictionary<string, Func<XmlReader, CatalogueLink, Task>> catalogueLinkAttributes = new()
        {
            { "id", async (reader, item) => item.Id = await reader.GetValueAsync() },
            { "importRootEntries", async (reader, item) => item.ImportRootEntries = await reader.GetValueAsync() == "true" },
            { "name", async (reader, item) => item.Name = await reader.GetValueAsync() },
            { "targetId", async (reader, item) => item.TargetId = await reader.GetValueAsync() },
            { "type", async (reader, item) => item.Type = await reader.GetValueAsync() },
        };

        private static readonly Dictionary<string, Func<XmlReader, CategoryEntry, Task>> categoryEntryAttributes = new()
        {
            { "hidden", async (reader, item) => item.IsHidden = await reader.GetValueAsync() == "true" },
            { "id", async (reader, item) => item.Id = await reader.GetValueAsync() },
            { "name", async (reader, item) => item.Name = await reader.GetValueAsync() },
            { "page", async (reader, item) => item.Page = await reader.GetValueAsync() },
            { "publicationId", async (reader, item) => item.PublicationId = await reader.GetValueAsync() },
        };

        private static readonly Dictionary<string, Func<XmlReader, CategoryLink, Task>> categoryLinkAttributes = new()
        {
            { "hidden", async (reader, item) => item.IsHidden = await reader.GetValueAsync() == "true" },
            { "id", async (reader, item) => item.Id = await reader.GetValueAsync() },
            { "name", async (reader, item) => item.Name = await reader.GetValueAsync() },
            { "primary", async (reader, item) => item.IsPrimary = await reader.GetValueAsync() == "true" },
            { "targetId", async (reader, item) => item.TargetId = await reader.GetValueAsync() },
        };

        private static readonly Dictionary<string, Func<XmlReader, CharacteristicType, Task>> characteristicTypeAttributes = new()
        {
            { "id", async (reader, item) => item.Id = await reader.GetValueAsync() },
            { "name", async (reader, item) => item.Name = await reader.GetValueAsync() },
        };

        private static readonly Dictionary<string, Func<XmlReader, CostType, Task>> costTypeAttributes = new()
        {
            { "defaultCostLimit", async (reader, item) => item.DefaultCostLimit = await reader.GetValueAsync() },
            { "hidden", async (reader, item) => item.IsHidden = await reader.GetValueAsync() == "true" },
            { "id", async (reader, item) => item.Id = await reader.GetValueAsync() },
            { "name", async (reader, item) => item.Name = await reader.GetValueAsync() },
        };

        private static readonly Dictionary<string, Func<XmlReader, EntryLink, Task>> entryLinkAttributes = new()
        {
            { "collective", async (reader, item) => item.IsCollective = await reader.GetValueAsync() == "true" },
            { "hidden", async (reader, item) => item.IsHidden = await reader.GetValueAsync() == "true" },
            { "import", async (reader, item) => item.IsImport = await reader.GetValueAsync() == "true" },
            { "id", async (reader, item) => item.Id = await reader.GetValueAsync() },
            { "name", async (reader, item) => item.Name = await reader.GetValueAsync() },
            { "page", async (reader, item) => item.Page = await reader.GetValueAsync() },
            { "publicationId", async (reader, item) => item.PublicationId = await reader.GetValueAsync() },
            { "targetId", async (reader, item) => item.TargetId = await reader.GetValueAsync() },
            { "type", async (reader, item) => item.Type = await reader.GetValueAsync() },
        };

        private static readonly Dictionary<string, Func<XmlReader, ForceEntry, Task>> forceEntryAttributes = new()
        {
            { "hidden", async (reader, item) => item.IsHidden = await reader.GetValueAsync() == "true" },
            { "id", async (reader, item) => item.Id = await reader.GetValueAsync() },
            { "name", async (reader, item) => item.Name = await reader.GetValueAsync() },
        };

        private static readonly Dictionary<string, Func<XmlReader, GameSystemConfiguration, Task>> gameSystemAttributes = new()
        {
            { "battleScribeVersion", async (reader, item) => item.BattleScribeVersion = await reader.GetValueAsync() },
            { "authorContact", async (reader, item) => item.AuthorContact = await reader.GetValueAsync() },
            { "authorName", async (reader, item) => item.AuthorName = await reader.GetValueAsync() },
            { "authorUrl", async (reader, item) => item.AuthorUrl = await reader.GetValueAsync() },
            { "gameSystemId", async (reader, item) => item.GameSystemId = await reader.GetValueAsync() },
            { "gameSystemRevision", async (reader, item) => item.GameSystemRevision = await reader.GetValueAsync() },
            { "id", async (reader, item) => item.Id = await reader.GetValueAsync() },
            { "library", async (reader, item) => item.IsLibrary = CommonDeserialization.HandleTrueFalse(await reader.GetValueAsync()) },
            { "name", async (reader, item) => item.Name = await reader.GetValueAsync() },
            { "revision", async (reader, item) => item.Revision = await reader.GetValueAsync() },
        };

        private static readonly Dictionary<string, Func<XmlReader, InformationGroup, Task>> informationGroupAttributes = new()
        {
            { "hidden", async (reader, item) => item.IsHidden = await reader.GetValueAsync() == "true" },
            { "id", async (reader, item) => item.Id = await reader.GetValueAsync() },
            { "name", async (reader, item) => item.Name = await reader.GetValueAsync() },
            { "page", async (reader, item) => item.Page = await reader.GetValueAsync() },
            { "publicationId", async (reader, item) => item.PublicationId = await reader.GetValueAsync() },
        };

        private static readonly Dictionary<string, Func<XmlReader, InformationLink, Task>> informationLinkAttributes = new()
        {
            { "hidden", async (reader, item) => item.IsHidden = await reader.GetValueAsync() == "true" },
            { "id", async (reader, item) => item.Id = await reader.GetValueAsync() },
            { "name", async (reader, item) => item.Name = await reader.GetValueAsync() },
            { "page", async (reader, item) => item.Page = await reader.GetValueAsync() },
            { "publicationId", async (reader, item) => item.PublicationId = await reader.GetValueAsync() },
            { "targetId", async (reader, item) => item.TargetId = await reader.GetValueAsync() },
            { "type", async (reader, item) => item.Type = await reader.GetValueAsync() },
        };

        private static readonly Dictionary<string, Func<XmlReader, ProfileType, Task>> profileTypeAttributes = new()
        {
            { "id", async (reader, item) => item.Id = await reader.GetValueAsync() },
            { "name", async (reader, item) => item.Name = await reader.GetValueAsync() },
        };

        private static readonly Dictionary<string, Func<XmlReader, SelectionEntry, Task>> selectionEntryAttributes = new()
        {
            { "collective", async (reader, item) => item.IsCollective = await reader.GetValueAsync() == "true" },
            { "hidden", async (reader, item) => item.IsHidden = await reader.GetValueAsync() == "true" },
            { "import", async (reader, item) => item.IsImport = await reader.GetValueAsync() == "true" },
            { "id", async (reader, item) => item.Id = await reader.GetValueAsync() },
            { "name", async (reader, item) => item.Name = await reader.GetValueAsync() },
            { "page", async (reader, item) => item.Page = await reader.GetValueAsync() },
            { "publicationId", async (reader, item) => item.PublicationId = await reader.GetValueAsync() },
            { "type", async (reader, item) => item.Type = await reader.GetValueAsync() },
        };

        private static readonly Dictionary<string, Func<XmlReader, SelectionEntryGroup, Task>> selectionEntryGroupAttributes = new()
        {
            { "collective", async (reader, item) => item.IsCollective = await reader.GetValueAsync() == "true" },
            { "defaultSelectionEntryId", async (reader, item) => item.DefaultSelectionEntryId = await reader.GetValueAsync() },
            { "hidden", async (reader, item) => item.IsHidden = await reader.GetValueAsync() == "true" },
            { "import", async (reader, item) => item.IsImport = await reader.GetValueAsync() == "true" },
            { "id", async (reader, item) => item.Id = await reader.GetValueAsync() },
            { "name", async (reader, item) => item.Name = await reader.GetValueAsync() },
            { "page", async (reader, item) => item.Page = await reader.GetValueAsync() },
            { "publicationId", async (reader, item) => item.PublicationId = await reader.GetValueAsync() },
        };

        /// <summary>
        /// Deserialize the root level element.
        /// </summary>
        /// <param name="reader">The <see cref="TextReader"/> containing the definition to deserialize.</param>
        /// <param name="configurationType">Defines the type of configuration the deserialisation should handle.</param>
        /// <param name="level">How much of the definition to deserialize.</param>
        /// <returns>A <see cref="GameSystemConfiguration"/> containing the deserialized data.</returns>
        public static async Task<GameSystemConfiguration> DeserializeRootAsync(TextReader reader, ConfigurationType configurationType, ConfigurationLevel level)
        {
            var gameSystem = new GameSystemConfiguration { Type = configurationType };
            var settings = new XmlReaderSettings { Async = true };
            using var xmlReader = XmlReader.Create(reader, settings);
            while (await xmlReader.ReadAsync())
            {
                await xmlReader.MoveToContentAsync();
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        // Should only have a gameSystem at the root level
                        if (!Constants.ConfigurationTypeRoots.TryGetValue(configurationType, out var details)) throw new Exception($"Unhandled configuration type {configurationType}");
                        if (xmlReader.Name != details.Root) throw new Exception($"Invalid game system definition: expected a {details.Root} node, found {xmlReader.Name} instead");
                        await DeserializeGameSystemConfigurationAsync(xmlReader, gameSystem, level);
                        break;

                    default:
                        // Anything else is an error
                        throw new Exception($"Unexpected node type: {xmlReader.NodeType} ({xmlReader.Name})");
                }
            }

            return gameSystem;
        }

        /// <summary>
        /// Deserialize a catalogue link.
        /// </summary>
        /// <param name="xmlReader">The <see cref="XmlReader"/> containing the definition to deserialize.</param>
        /// <param name="parent">The <see cref="List{CatalogueLink}"/> to populate.</param>
        private static async Task DeserializeCatalogueLinkAsync(XmlReader xmlReader, List<CatalogueLink>? parent)
        {
            var name = xmlReader.Name;
            var isReading = !xmlReader.IsEmptyElement;
            var item = new CatalogueLink();
            await xmlReader.DeserializeAttributesAsync(item, catalogueLinkAttributes);
            if (parent == null) throw new Exception("CatalogueLinks has not been initialised");
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
        /// Deserialize a category entry.
        /// </summary>
        /// <param name="xmlReader">The <see cref="XmlReader"/> containing the definition to deserialize.</param>
        /// <param name="parent">The <see cref="GameSystemConfiguration"/> to populate.</param>
        private static async Task DeserializeCategoryEntryAsync(XmlReader xmlReader, GameSystemConfiguration parent)
        {
            var name = xmlReader.Name;
            var isReading = !xmlReader.IsEmptyElement;
            var item = new CategoryEntry();
            await xmlReader.DeserializeAttributesAsync(item, categoryEntryAttributes);
            if (parent.CategoryEntries == null) throw new Exception("CategoryEntries has not been initialised");
            parent.CategoryEntries.Add(item);

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

                            case "constraints":
                                item.Constraints ??= new List<Constraint>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "constraint",
                                    async (reader, _) => await CommonDeserialization.DeserializeConstraint(reader, item.Constraints));
                                break;

                            case "infoLinks":
                                item.InformationLinks ??= new List<InformationLink>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "infoLink",
                                    async (reader, _) => await DeserializeInformationLinkAsync(reader, item.InformationLinks));
                                break;

                            case "modifierGroups":
                                item.ModifierGroups ??= new List<ModifierGroup>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "modifierGroup",
                                    async (reader, _) => await CommonDeserialization.DeserializeModifierGroupAsync(reader, item.ModifierGroups));
                                break;

                            case "modifiers":
                                item.Modifiers ??= new List<Modifier>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "modifier",
                                    async (reader, _) => await CommonDeserialization.DeserializeModifierAsync(reader, item.Modifiers));
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
        /// Deserialize a category link.
        /// </summary>
        /// <param name="xmlReader">The <see cref="XmlReader"/> containing the definition to deserialize.</param>
        /// <param name="parent">The <see cref="GameSystemConfiguration"/> to populate.</param>
        private static async Task DeserializeCategoryLinkAsync(XmlReader xmlReader, List<CategoryLink>? parent)
        {
            var name = xmlReader.Name;
            var isReading = !xmlReader.IsEmptyElement;
            var item = new CategoryLink();
            await xmlReader.DeserializeAttributesAsync(item, categoryLinkAttributes);
            if (parent == null) throw new Exception("CategoryLinks has not been initialised");
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

                            case "constraints":
                                item.Constraints ??= new List<Constraint>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "constraint",
                                    async (reader, _) => await CommonDeserialization.DeserializeConstraint(reader, item.Constraints));
                                break;

                            case "modifiers":
                                item.Modifiers ??= new List<Modifier>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "modifier",
                                    async (reader, _) => await CommonDeserialization.DeserializeModifierAsync(reader, item.Modifiers));
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

        private static async Task DeserializeCostTypeAsync(XmlReader xmlReader, List<CostType> parent)
        {
            var name = xmlReader.Name;
            var isReading = !xmlReader.IsEmptyElement;
            var item = new CostType();
            await xmlReader.DeserializeAttributesAsync(item, costTypeAttributes);
            if (parent == null) throw new Exception("CostTypes has not been initialised");
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
        /// Deserialize an entry link.
        /// </summary>
        /// <param name="xmlReader">The <see cref="XmlReader"/> containing the definition to deserialize.</param>
        /// <param name="parent">The <see cref="GameSystemConfiguration"/> to populate.</param>
        private static async Task DeserializeEntryLinkAsync(XmlReader xmlReader, List<EntryLink>? parent)
        {
            var name = xmlReader.Name;
            var isReading = !xmlReader.IsEmptyElement;
            var item = new EntryLink();
            await xmlReader.DeserializeAttributesAsync(item, entryLinkAttributes);
            if (parent == null) throw new Exception("EntryLinks has not been initialised");
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

                            case "categoryLinks":
                                item.CategoryLinks ??= new List<CategoryLink>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "categoryLink",
                                    async (reader, _) => await DeserializeCategoryLinkAsync(reader, item.CategoryLinks));
                                break;

                            case "constraints":
                                item.Constraints ??= new List<Constraint>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "constraint",
                                    async (reader, _) => await CommonDeserialization.DeserializeConstraint(reader, item.Constraints));
                                break;

                            case "costs":
                                item.Costs ??= new List<ItemCost>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "cost",
                                    async (reader, _) => await reader.DeserializeSingleItemAsync(item.Costs, CommonDeserialization.CostAttributes));
                                break;

                            case "entryLinks":
                                item.EntryLinks ??= new List<EntryLink>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "entryLink",
                                    async (reader, _) => await DeserializeEntryLinkAsync(reader, item.EntryLinks));
                                break;

                            case "infoGroups":
                                item.InformationGroups ??= new List<InformationGroup>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "infoGroup",
                                    async (reader, _) => await DeserializeInformationGroupsAsync(reader, item.InformationGroups));
                                break;

                            case "infoLinks":
                                item.InformationLinks ??= new List<InformationLink>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "infoLink",
                                    async (reader, _) => await DeserializeInformationLinkAsync(reader, item.InformationLinks));
                                break;

                            case "modifierGroups":
                                item.ModifierGroups ??= new List<ModifierGroup>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "modifierGroup",
                                    async (reader, _) => await CommonDeserialization.DeserializeModifierGroupAsync(reader, item.ModifierGroups));
                                break;

                            case "modifiers":
                                item.Modifiers ??= new List<Modifier>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "modifier",
                                    async (reader, _) => await CommonDeserialization.DeserializeModifierAsync(reader, item.Modifiers));
                                break;

                            case "profiles":
                                item.Profiles ??= new List<Profile>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "profile",
                                    async (reader, _) => await CommonDeserialization.DeserializeProfilesAsync(reader, item.Profiles));
                                break;

                            case "selectionEntries":
                                item.SelectionEntries ??= new List<SelectionEntry>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "selectionEntry",
                                    async (reader, _) => await DeserializeSelectionEntryAsync(reader, item.SelectionEntries));
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
        /// Deserialize a force entry.
        /// </summary>
        /// <param name="xmlReader">The <see cref="XmlReader"/> containing the definition to deserialize.</param>
        /// <param name="parent">The <see cref="GameSystemConfiguration"/> to populate.</param>
        private static async Task DeserializeForceEntryAsync(XmlReader xmlReader, List<ForceEntry>? parent)
        {
            var name = xmlReader.Name;
            var isReading = !xmlReader.IsEmptyElement;
            var item = new ForceEntry();
            await xmlReader.DeserializeAttributesAsync(item, forceEntryAttributes);
            if (parent == null) throw new Exception("ForceEntries has not been initialised");
            parent.Add(item);

            while (isReading && await xmlReader.ReadAsync())
            {
                await xmlReader.MoveToContentAsync();
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (xmlReader.Name)
                        {
                            case "categoryLinks":
                                item.CategoryLinks ??= new List<CategoryLink>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "categoryLink",
                                    async (reader, _) => await DeserializeCategoryLinkAsync(reader, item.CategoryLinks));
                                break;

                            case "comment":
                                item.Comment = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            case "constraints":
                                item.Constraints ??= new List<Constraint>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "constraint",
                                    async (reader, _) => await CommonDeserialization.DeserializeConstraint(reader, item.Constraints));
                                break;

                            case "forceEntries":
                                item.ForceEntries ??= new List<ForceEntry>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "forceEntry",
                                    async (reader, _) => await DeserializeForceEntryAsync(reader, item.ForceEntries));
                                break;

                            case "modifiers":
                                item.Modifiers ??= new List<Modifier>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "modifier",
                                    async (reader, _) => await CommonDeserialization.DeserializeModifierAsync(reader, item.Modifiers));
                                break;

                            case "rules":
                                item.Rules ??= new List<Rule>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "rule",
                                    async (reader, _) => await CommonDeserialization.DeserializeRuleAsync(reader, item.Rules));
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

        private static async Task DeserializeGameSystemConfigurationAsync(XmlReader xmlReader, GameSystemConfiguration item, ConfigurationLevel level)
        {
            var name = xmlReader.Name;
            var isReading = !xmlReader.IsEmptyElement;
            await xmlReader.DeserializeAttributesAsync(item, gameSystemAttributes);

            while (isReading && await xmlReader.ReadAsync())
            {
                await xmlReader.MoveToContentAsync();
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (xmlReader.Name)
                        {
                            case "categoryEntries":
                                if (level == ConfigurationLevel.Root)
                                {
                                    await xmlReader.SkipAsync();
                                    continue;
                                }

                                item.CategoryEntries ??= new List<CategoryEntry>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "categoryEntry",
                                    DeserializeCategoryEntryAsync);
                                break;

                            case "catalogueLinks":
                                if (level == ConfigurationLevel.Root)
                                {
                                    await xmlReader.SkipAsync();
                                    continue;
                                }

                                item.CatalogueLinks ??= new List<CatalogueLink>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "catalogueLink",
                                    async (reader, _) => await DeserializeCatalogueLinkAsync(reader, item.CatalogueLinks));
                                break;

                            case "comment":
                                item.Comment = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            case "costTypes":
                                if (level == ConfigurationLevel.Root)
                                {
                                    await xmlReader.SkipAsync();
                                    continue;
                                }

                                item.CostTypes ??= new List<CostType>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "costType",
                                    async (reader, _) => await DeserializeCostTypeAsync(reader, item.CostTypes));
                                break;

                            case "entryLinks":
                                if (level == ConfigurationLevel.Root)
                                {
                                    await xmlReader.SkipAsync();
                                    continue;
                                }

                                item.EntryLinks ??= new List<EntryLink>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "entryLink",
                                    async (reader, _) => await DeserializeEntryLinkAsync(reader, item.EntryLinks));
                                break;

                            case "forceEntries":
                                if (level == ConfigurationLevel.Root)
                                {
                                    await xmlReader.SkipAsync();
                                    continue;
                                }

                                item.ForceEntries ??= new List<ForceEntry>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "forceEntry",
                                    async (reader, _) => await DeserializeForceEntryAsync(reader, item.ForceEntries));
                                break;

                            case "infoLinks":
                                if (level == ConfigurationLevel.Root)
                                {
                                    await xmlReader.SkipAsync();
                                    continue;
                                }

                                item.InformationLinks ??= new List<InformationLink>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "infoLink",
                                    async (reader, _) => await DeserializeInformationLinkAsync(reader, item.InformationLinks));
                                break;

                            case "profileTypes":
                                if (level == ConfigurationLevel.Root)
                                {
                                    await xmlReader.SkipAsync();
                                    continue;
                                }

                                item.ProfileTypes ??= new List<ProfileType>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "profileType",
                                    DeserializeProfileTypeAsync);
                                break;

                            case "publications":
                                if (level == ConfigurationLevel.Root)
                                {
                                    await xmlReader.SkipAsync();
                                    continue;
                                }

                                item.Publications ??= new List<Publication>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "publication",
                                    async (reader, _) => await CommonDeserialization.DeserializePublication(reader, item.Publications));
                                break;

                            case "readme":
                                item.ReadMe = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            case "rules":
                                if (level == ConfigurationLevel.Root)
                                {
                                    await xmlReader.SkipAsync();
                                    continue;
                                }

                                item.Rules ??= new List<Rule>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "rule",
                                    async (reader, _) => await CommonDeserialization.DeserializeRuleAsync(reader, item.Rules));
                                break;

                            case "selectionEntries":
                                if (level == ConfigurationLevel.Root)
                                {
                                    await xmlReader.SkipAsync();
                                    continue;
                                }

                                item.SelectionEntries ??= new List<SelectionEntry>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "selectionEntry",
                                    async (reader, _) => await DeserializeSelectionEntryAsync(reader, item.SelectionEntries));
                                break;

                            case "sharedInfoGroups":
                                if (level == ConfigurationLevel.Root)
                                {
                                    await xmlReader.SkipAsync();
                                    continue;
                                }

                                item.SharedInformationGroups ??= new List<InformationGroup>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "infoGroup",
                                    async (reader, _) => await DeserializeInformationGroupsAsync(reader, item.SharedInformationGroups));
                                break;

                            case "sharedProfiles":
                                if (level == ConfigurationLevel.Root)
                                {
                                    await xmlReader.SkipAsync();
                                    continue;
                                }

                                item.SharedProfiles ??= new List<Profile>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "profile",
                                    async (reader, _) => await CommonDeserialization.DeserializeProfilesAsync(reader, item.SharedProfiles));
                                break;

                            case "sharedRules":
                                if (level == ConfigurationLevel.Root)
                                {
                                    await xmlReader.SkipAsync();
                                    continue;
                                }

                                item.SharedRules ??= new List<Rule>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "rule",
                                    async (reader, _) => await CommonDeserialization.DeserializeRuleAsync(reader, item.SharedRules));
                                break;

                            case "sharedSelectionEntries":
                                if (level == ConfigurationLevel.Root)
                                {
                                    await xmlReader.SkipAsync();
                                    continue;
                                }

                                item.SharedSelectionEntries ??= new List<SelectionEntry>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "selectionEntry",
                                    async (reader, _) => await DeserializeSelectionEntryAsync(reader, item.SharedSelectionEntries));
                                break;

                            case "sharedSelectionEntryGroups":
                                if (level == ConfigurationLevel.Root)
                                {
                                    await xmlReader.SkipAsync();
                                    continue;
                                }

                                item.SharedSelectionEntryGroups ??= new List<SelectionEntryGroup>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "selectionEntryGroup",
                                    async (reader, _) => await DeserializeSelectionEntryGroupAsync(reader, item.SharedSelectionEntryGroups));
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
        /// Deserialize an information group.
        /// </summary>
        /// <param name="xmlReader">The <see cref="XmlReader"/> containing the definition to deserialize.</param>
        /// <param name="parent">The <see cref="List{InformationGroup}"/> to populate.</param>
        private static async Task DeserializeInformationGroupsAsync(XmlReader xmlReader, List<InformationGroup>? parent)
        {
            var name = xmlReader.Name;
            var isReading = !xmlReader.IsEmptyElement;
            var item = new InformationGroup();
            await xmlReader.DeserializeAttributesAsync(item, informationGroupAttributes);
            if (parent == null) throw new Exception("informationGroups has not been initialised");
            parent.Add(item);

            while (isReading && await xmlReader.ReadAsync())
            {
                await xmlReader.MoveToContentAsync();
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (xmlReader.Name)
                        {
                            case "infoLinks":
                                item.InformationLinks ??= new List<InformationLink>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "infoLink",
                                    async (reader, _) => await DeserializeInformationLinkAsync(reader, item.InformationLinks));
                                break;

                            case "profiles":
                                item.Profiles ??= new List<Profile>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "profile",
                                    async (reader, _) => await CommonDeserialization.DeserializeProfilesAsync(reader, item.Profiles));
                                break;

                            case "rules":
                                item.Rules ??= new List<Rule>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "rule",
                                    async (reader, _) => await CommonDeserialization.DeserializeRuleAsync(reader, item.Rules));
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
        /// Deserialize an information link.
        /// </summary>
        /// <param name="xmlReader">The <see cref="XmlReader"/> containing the definition to deserialize.</param>
        /// <param name="parent">The <see cref="List{InformationLink}"/> to populate.</param>
        private static async Task DeserializeInformationLinkAsync(XmlReader xmlReader, List<InformationLink>? parent)
        {
            var name = xmlReader.Name;
            var isReading = !xmlReader.IsEmptyElement;
            var item = new InformationLink();
            await xmlReader.DeserializeAttributesAsync(item, informationLinkAttributes);
            if (parent == null) throw new Exception("InformationLinks has not been initialised");
            parent.Add(item);

            while (isReading && await xmlReader.ReadAsync())
            {
                await xmlReader.MoveToContentAsync();
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (xmlReader.Name)
                        {
                            case "modifierGroups":
                                item.ModifierGroups ??= new List<ModifierGroup>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "modifierGroup",
                                    async (reader, _) => await CommonDeserialization.DeserializeModifierGroupAsync(reader, item.ModifierGroups));
                                break;

                            case "modifiers":
                                item.Modifiers ??= new List<Modifier>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "modifier",
                                    async (reader, _) => await CommonDeserialization.DeserializeModifierAsync(reader, item.Modifiers));
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
        /// Deserialize a profile type.
        /// </summary>
        /// <param name="xmlReader">The <see cref="XmlReader"/> containing the definition to deserialize.</param>
        /// <param name="parent">The <see cref="GameSystemConfiguration"/> to populate.</param>
        private static async Task DeserializeProfileTypeAsync(XmlReader xmlReader, GameSystemConfiguration parent)
        {
            var name = xmlReader.Name;
            var isReading = !xmlReader.IsEmptyElement;
            var item = new ProfileType();
            await xmlReader.DeserializeAttributesAsync(item, profileTypeAttributes);
            if (parent.ProfileTypes == null) throw new Exception("ProfileTypes has not been initialised");
            parent.ProfileTypes.Add(item);

            while (isReading && await xmlReader.ReadAsync())
            {
                await xmlReader.MoveToContentAsync();
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (xmlReader.Name)
                        {
                            case "characteristicTypes":
                                item.CharacteristicTypes ??= new List<CharacteristicType>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "characteristicType",
                                    async (reader, _) => await reader.DeserializeSingleItemAsync(item.CharacteristicTypes, characteristicTypeAttributes));
                                break;

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
        /// Deserialize a selection entry.
        /// </summary>
        /// <param name="xmlReader">The <see cref="XmlReader"/> containing the definition to deserialize.</param>
        /// <param name="parent">The <see cref="List{SelectionEntry}"/> to populate.</param>
        private static async Task DeserializeSelectionEntryAsync(XmlReader xmlReader, List<SelectionEntry> parent)
        {
            var name = xmlReader.Name;
            var isReading = !xmlReader.IsEmptyElement;
            var item = new SelectionEntry();
            await xmlReader.DeserializeAttributesAsync(item, selectionEntryAttributes);
            parent.Add(item);

            while (isReading && await xmlReader.ReadAsync())
            {
                await xmlReader.MoveToContentAsync();
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (xmlReader.Name)
                        {
                            case "categoryLinks":
                                item.CategoryLinks ??= new List<CategoryLink>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "categoryLink",
                                    async (reader, _) => await DeserializeCategoryLinkAsync(reader, item.CategoryLinks));
                                break;

                            case "comment":
                                item.Comment = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            case "constraints":
                                item.Constraints ??= new List<Constraint>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "constraint",
                                    async (reader, _) => await CommonDeserialization.DeserializeConstraint(reader, item.Constraints));
                                break;

                            case "costs":
                                item.Costs ??= new List<ItemCost>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "cost",
                                    async (reader, _) => await reader.DeserializeSingleItemAsync(item.Costs, CommonDeserialization.CostAttributes));
                                break;

                            case "entryLinks":
                                item.EntryLinks ??= new List<EntryLink>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "entryLink",
                                    async (reader, _) => await DeserializeEntryLinkAsync(reader, item.EntryLinks));
                                break;

                            case "infoGroups":
                                item.InformationGroups ??= new List<InformationGroup>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "infoGroup",
                                    async (reader, _) => await DeserializeInformationGroupsAsync(reader, item.InformationGroups));
                                break;

                            case "infoLinks":
                                item.InformationLinks ??= new List<InformationLink>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "infoLink",
                                    async (reader, _) => await DeserializeInformationLinkAsync(reader, item.InformationLinks));
                                break;

                            case "modifierGroups":
                                item.ModifierGroups ??= new List<ModifierGroup>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "modifierGroup",
                                    async (reader, _) => await CommonDeserialization.DeserializeModifierGroupAsync(reader, item.ModifierGroups));
                                break;

                            case "modifiers":
                                item.Modifiers ??= new List<Modifier>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "modifier",
                                    async (reader, _) => await CommonDeserialization.DeserializeModifierAsync(reader, item.Modifiers));
                                break;

                            case "profiles":
                                item.Profiles ??= new List<Profile>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "profile",
                                    async (reader, _) => await CommonDeserialization.DeserializeProfilesAsync(reader, item.Profiles));
                                break;

                            case "rules":
                                item.Rules ??= new List<Rule>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "rule",
                                    async (reader, _) => await CommonDeserialization.DeserializeRuleAsync(reader, item.Rules));
                                break;

                            case "selectionEntries":
                                item.SelectionEntries ??= new List<SelectionEntry>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "selectionEntry",
                                    async (reader, _) => await DeserializeSelectionEntryAsync(reader, item.SelectionEntries));
                                break;

                            case "selectionEntryGroups":
                                item.SelectionEntryGroups ??= new List<SelectionEntryGroup>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "selectionEntryGroup",
                                    async (reader, _) => await DeserializeSelectionEntryGroupAsync(reader, item.SelectionEntryGroups));
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
        /// Deserialize a selection entry group.
        /// </summary>
        /// <param name="xmlReader">The <see cref="XmlReader"/> containing the definition to deserialize.</param>
        /// <param name="parent">The <see cref="List{SelectionEntryGroup}"/> to populate.</param>
        private static async Task DeserializeSelectionEntryGroupAsync(XmlReader xmlReader, List<SelectionEntryGroup> parent)
        {
            var name = xmlReader.Name;
            var isReading = !xmlReader.IsEmptyElement;
            var item = new SelectionEntryGroup();
            await xmlReader.DeserializeAttributesAsync(item, selectionEntryGroupAttributes);
            parent.Add(item);

            while (isReading && await xmlReader.ReadAsync())
            {
                await xmlReader.MoveToContentAsync();
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (xmlReader.Name)
                        {
                            case "categoryLinks":
                                item.CategoryLinks ??= new List<CategoryLink>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "categoryLink",
                                    async (reader, _) => await DeserializeCategoryLinkAsync(reader, item.CategoryLinks));
                                break;

                            case "comment":
                                item.Comment = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            case "constraints":
                                item.Constraints ??= new List<Constraint>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "constraint",
                                    async (reader, _) => await CommonDeserialization.DeserializeConstraint(reader, item.Constraints));
                                break;

                            case "entryLinks":
                                item.EntryLinks ??= new List<EntryLink>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "entryLink",
                                    async (reader, _) => await DeserializeEntryLinkAsync(reader, item.EntryLinks));
                                break;

                            case "modifierGroups":
                                item.ModifierGroups ??= new List<ModifierGroup>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "modifierGroup",
                                    async (reader, _) => await CommonDeserialization.DeserializeModifierGroupAsync(reader, item.ModifierGroups));
                                break;

                            case "modifiers":
                                item.Modifiers ??= new List<Modifier>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "modifier",
                                    async (reader, _) => await CommonDeserialization.DeserializeModifierAsync(reader, item.Modifiers));
                                break;

                            case "profiles":
                                item.Profiles ??= new List<Profile>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "profile",
                                    async (reader, _) => await CommonDeserialization.DeserializeProfilesAsync(reader, item.Profiles));
                                break;

                            case "selectionEntries":
                                item.SelectionEntries ??= new List<SelectionEntry>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "selectionEntry",
                                    async (reader, _) => await DeserializeSelectionEntryAsync(reader, item.SelectionEntries));
                                break;

                            case "selectionEntryGroups":
                                item.SelectionEntryGroups ??= new List<SelectionEntryGroup>();
                                await xmlReader.DeserializeArrayAsync(
                                    item,
                                    "selectionEntryGroup",
                                    async (reader, _) => await DeserializeSelectionEntryGroupAsync(reader, item.SelectionEntryGroups));
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
    }
}