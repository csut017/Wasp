using System.Xml;

namespace Wasp.Core.Data.Xml
{
    internal class GameSystemDeserialization
    {
        private static readonly Dictionary<string, Func<XmlReader, CategoryEntry, Task>> categoryEntryAttributes = new()
        {
            { "hidden", async (reader, categoryEntry) => categoryEntry.IsHidden = await reader.GetValueAsync() == "true" },
            { "id", async (reader, categoryEntry) => categoryEntry.Id = await reader.GetValueAsync() },
            { "name", async (reader, categoryEntry) => categoryEntry.Name = await reader.GetValueAsync() },
            { "page", async (reader, categoryEntry) => categoryEntry.Page = await reader.GetValueAsync() },
            { "publicationId", async (reader, categoryEntry) => categoryEntry.PublicationId = await reader.GetValueAsync() },
        };

        private static readonly Dictionary<string, Func<XmlReader, CategoryLink, Task>> categoryLinkAttributes = new()
        {
            { "hidden", async (reader, categoryLink) => categoryLink.IsHidden = await reader.GetValueAsync() == "true" },
            { "id", async (reader, categoryLink) => categoryLink.Id = await reader.GetValueAsync() },
            { "name", async (reader, categoryLink) => categoryLink.Name = await reader.GetValueAsync() },
            { "primary", async (reader, categoryLink) => categoryLink.IsPrimary = await reader.GetValueAsync() == "true" },
            { "targetId", async (reader, categoryLink) => categoryLink.TargetId = await reader.GetValueAsync() },
        };

        private static readonly Dictionary<string, Func<XmlReader, CharacteristicType, Task>> characteristicTypeAttributes = new()
        {
            { "id", async (reader, characteristicType) => characteristicType.Id = await reader.GetValueAsync() },
            { "name", async (reader, characteristicType) => characteristicType.Name = await reader.GetValueAsync() },
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
            { "percentValue", async (reader, categoryEntry) => categoryEntry.PercentValue = await reader.GetValueAsync() },
            { "repeats", async (reader, categoryEntry) => categoryEntry.RepeatsValue = await reader.GetValueAsync() },
            { "roundUp", async (reader, categoryEntry) => categoryEntry.ShouldRoundUp = await reader.GetValueAsync() == "true" },
            { "scope", async (reader, categoryEntry) => categoryEntry.Scope = await reader.GetValueAsync() },
            { "shared", async (reader, categoryEntry) => categoryEntry.IsShared = await reader.GetValueAsync() == "true" },
            { "type", async (reader, categoryEntry) => categoryEntry.Type = await reader.GetValueAsync() },
            { "value", async (reader, categoryEntry) => categoryEntry.AbsoluteValue = await reader.GetValueAsync() },
        };

        private static readonly Dictionary<string, Func<XmlReader, CostType, Task>> costTypeAttributes = new()
        {
            { "defaultCostLimit", async (reader, cost) => cost.DefaultCostLimit = await reader.GetValueAsync() },
            { "hidden", async (reader, profile) => profile.IsHidden = await reader.GetValueAsync() == "true" },
            { "id", async (reader, cost) => cost.Id = await reader.GetValueAsync() },
            { "name", async (reader, cost) => cost.Name = await reader.GetValueAsync() },
        };

        private static readonly Dictionary<string, Func<XmlReader, EntryLink, Task>> entryLinkAttributes = new()
        {
            { "collective", async (reader, entryLink) => entryLink.IsCollective = await reader.GetValueAsync() == "true" },
            { "hidden", async (reader, entryLink) => entryLink.IsHidden = await reader.GetValueAsync() == "true" },
            { "import", async (reader, entryLink) => entryLink.IsImport = await reader.GetValueAsync() == "true" },
            { "id", async (reader, entryLink) => entryLink.Id = await reader.GetValueAsync() },
            { "name", async (reader, entryLink) => entryLink.Name = await reader.GetValueAsync() },
            { "targetId", async (reader, entryLink) => entryLink.TargetId = await reader.GetValueAsync() },
            { "type", async (reader, entryLink) => entryLink.Type = await reader.GetValueAsync() },
        };

        private static readonly Dictionary<string, Func<XmlReader, ForceEntry, Task>> forceEntryAttributes = new()
        {
            { "hidden", async (reader, forceEntry) => forceEntry.IsHidden = await reader.GetValueAsync() == "true" },
            { "id", async (reader, forceEntry) => forceEntry.Id = await reader.GetValueAsync() },
            { "name", async (reader, forceEntry) => forceEntry.Name = await reader.GetValueAsync() },
        };

        private static readonly Dictionary<string, Func<XmlReader, GameSystem, Task>> gameSystemAttributes = new()
        {
            { "battleScribeVersion", async (reader, gameSystem) => gameSystem.BattleScribeVersion = await reader.GetValueAsync() },
            { "authorContact", async (reader, gameSystem) => gameSystem.AuthorContact = await reader.GetValueAsync() },
            { "authorName", async (reader, gameSystem) => gameSystem.AuthorName = await reader.GetValueAsync() },
            { "authorUrl", async (reader, gameSystem) => gameSystem.AuthorUrl = await reader.GetValueAsync() },
            { "id", async (reader, gameSystem) => gameSystem.Id = await reader.GetValueAsync() },
            { "name", async (reader, gameSystem) => gameSystem.Name = await reader.GetValueAsync() },
            { "revision", async (reader, gameSystem) => gameSystem.Revision = await reader.GetValueAsync() },
        };

        private static readonly Dictionary<string, Func<XmlReader, Modifier, Task>> modifierAttributes = new()
        {
            { "field", async (reader, modifier) => modifier.Field = await reader.GetValueAsync() },
            { "type", async (reader, modifier) => modifier.Type = await reader.GetValueAsync() },
            { "value", async (reader, modifier) => modifier.Value = await reader.GetValueAsync() },
        };

        private static readonly Dictionary<string, Func<XmlReader, ProfileType, Task>> profileTypeAttributes = new()
        {
            { "id", async (reader, profileType) => profileType.Id = await reader.GetValueAsync() },
            { "name", async (reader, profileType) => profileType.Name = await reader.GetValueAsync() },
        };

        /// <summary>
        /// Deserialize the root level element.
        /// </summary>
        /// <param name="reader">The <see cref="TextReader"/> containing the definition to deserialize.</param>
        /// <returns>A <see cref="GameSystem"/> containing the deserialized data.</returns>
        public static async Task<GameSystem> DeserializeRootAsync(TextReader reader)
        {
            var gameSystem = new GameSystem();
            var settings = new XmlReaderSettings { Async = true };
            using var xmlReader = XmlReader.Create(reader, settings);
            while (await xmlReader.ReadAsync())
            {
                await xmlReader.MoveToContentAsync();
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        // Should only have a gameSystem at the root level
                        if (xmlReader.Name != "gameSystem") throw new Exception($"Invalid game system definition: expected a gameSystem node, found {xmlReader.Name} instead");
                        await DeserializeGameSystemAsync(xmlReader, gameSystem);
                        break;

                    default:
                        // Anything else is an error
                        throw new Exception($"Unexpected node type: {xmlReader.NodeType} ({xmlReader.Name})");
                }
            }

            return gameSystem;
        }

        /// <summary>
        /// Deserialize a category entry.
        /// </summary>
        /// <param name="xmlReader">The <see cref="XmlReader"/> containing the definition to deserialize.</param>
        /// <param name="parent">The <see cref="GameSystem"/> to populate.</param>
        private static async Task DeserializeCategoryEntry(XmlReader xmlReader, GameSystem parent)
        {
            var name = xmlReader.Name;
            var isReading = !xmlReader.IsEmptyElement;
            var categoryEntry = new CategoryEntry();
            await xmlReader.DeserializeAttributesAsync(categoryEntry, categoryEntryAttributes);
            if (parent.CategoryEntries == null) throw new Exception("CategoryEntries has not been initialised");
            parent.CategoryEntries.Add(categoryEntry);

            while (isReading && await xmlReader.ReadAsync())
            {
                await xmlReader.MoveToContentAsync();
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (xmlReader.Name)
                        {
                            case "comment":
                                categoryEntry.Comment = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            case "constraints":
                                categoryEntry.Constraints ??= new List<Constraint>();
                                await xmlReader.DeserializeArrayAsync(
                                    categoryEntry,
                                    "constraint",
                                    async (reader, _) => await reader.DeserializeSingleItemAsync(categoryEntry.Constraints, constraintAttributes));
                                break;

                            case "modifiers":
                                categoryEntry.Modifiers ??= new List<Modifier>();
                                await xmlReader.DeserializeArrayAsync(
                                    categoryEntry,
                                    "modifier",
                                    DeserializeModifier);
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
        /// Deserialize a category link.
        /// </summary>
        /// <param name="xmlReader">The <see cref="XmlReader"/> containing the definition to deserialize.</param>
        /// <param name="parent">The <see cref="GameSystem"/> to populate.</param>
        private static async Task DeserializeCategoryLink(XmlReader xmlReader, ICategoryLinksParent parent)
        {
            var name = xmlReader.Name;
            var isReading = !xmlReader.IsEmptyElement;
            var categoryLink = new CategoryLink();
            await xmlReader.DeserializeAttributesAsync(categoryLink, categoryLinkAttributes);
            if (parent.CategoryLinks == null) throw new Exception("CategoryLinks has not been initialised");
            parent.CategoryLinks.Add(categoryLink);

            while (isReading && await xmlReader.ReadAsync())
            {
                await xmlReader.MoveToContentAsync();
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (xmlReader.Name)
                        {
                            case "comment":
                                categoryLink.Comment = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            case "constraints":
                                categoryLink.Constraints ??= new List<Constraint>();
                                await xmlReader.DeserializeArrayAsync(
                                    categoryLink,
                                    "constraint",
                                    async (reader, _) => await reader.DeserializeSingleItemAsync(categoryLink.Constraints, constraintAttributes));
                                break;

                            case "modifiers":
                                categoryLink.Modifiers ??= new List<Modifier>();
                                await xmlReader.DeserializeArrayAsync(
                                    categoryLink,
                                    "modifier",
                                    DeserializeModifier);
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
        /// Deserialize a condition group.
        /// </summary>
        /// <param name="xmlReader">The <see cref="XmlReader"/> containing the definition to deserialize.</param>
        /// <param name="parent">The <see cref="GameSystem"/> to populate.</param>
        private static async Task DeserializeConditionGroup(XmlReader xmlReader, IConditionGroupsParent parent)
        {
            var name = xmlReader.Name;
            var isReading = !xmlReader.IsEmptyElement;
            var conditionGroup = new ConditionGroup();
            await xmlReader.DeserializeAttributesAsync(conditionGroup, conditionGroupAttributes);
            if (parent.ConditionGroups == null) throw new Exception("ConditionGroups has not been initialised");
            parent.ConditionGroups.Add(conditionGroup);

            while (isReading && await xmlReader.ReadAsync())
            {
                await xmlReader.MoveToContentAsync();
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (xmlReader.Name)
                        {
                            case "conditionGroups":
                                conditionGroup.ConditionGroups ??= new List<ConditionGroup>();
                                await xmlReader.DeserializeArrayAsync(
                                    conditionGroup,
                                    "conditionGroup",
                                    DeserializeConditionGroup);
                                break;

                            case "conditions":
                                conditionGroup.Conditions ??= new List<Constraint>();
                                await xmlReader.DeserializeArrayAsync(
                                    conditionGroup,
                                    "condition",
                                    async (reader, _) => await reader.DeserializeSingleItemAsync(conditionGroup.Conditions, constraintAttributes));
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
        /// Deserialize an entry link.
        /// </summary>
        /// <param name="xmlReader">The <see cref="XmlReader"/> containing the definition to deserialize.</param>
        /// <param name="parent">The <see cref="GameSystem"/> to populate.</param>
        private static async Task DeserializeEntryLink(XmlReader xmlReader, GameSystem parent)
        {
            var name = xmlReader.Name;
            var isReading = !xmlReader.IsEmptyElement;
            var entryLink = new EntryLink();
            await xmlReader.DeserializeAttributesAsync(entryLink, entryLinkAttributes);
            if (parent.EntryLinks == null) throw new Exception("EntryLinks has not been initialised");
            parent.EntryLinks.Add(entryLink);

            while (isReading && await xmlReader.ReadAsync())
            {
                await xmlReader.MoveToContentAsync();
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (xmlReader.Name)
                        {
                            case "comment":
                                entryLink.Comment = await xmlReader.ReadElementContentAsStringAsync();
                                break;

                            case "categoryLinks":
                                entryLink.CategoryLinks ??= new List<CategoryLink>();
                                await xmlReader.DeserializeArrayAsync(
                                    entryLink,
                                    "categoryLink",
                                    DeserializeCategoryLink);
                                break;

                            case "constraints":
                                entryLink.Constraints ??= new List<Constraint>();
                                await xmlReader.DeserializeArrayAsync(
                                    entryLink,
                                    "constraint",
                                    async (reader, _) => await reader.DeserializeSingleItemAsync(entryLink.Constraints, constraintAttributes));
                                break;

                            case "modifiers":
                                entryLink.Modifiers ??= new List<Modifier>();
                                await xmlReader.DeserializeArrayAsync(
                                    entryLink,
                                    "modifier",
                                    DeserializeModifier);
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
        /// Deserialize a force entry.
        /// </summary>
        /// <param name="xmlReader">The <see cref="XmlReader"/> containing the definition to deserialize.</param>
        /// <param name="parent">The <see cref="GameSystem"/> to populate.</param>
        private static async Task DeserializeForceEntry(XmlReader xmlReader, IForceEntriesParent parent)
        {
            var name = xmlReader.Name;
            var isReading = !xmlReader.IsEmptyElement;
            var forceEntry = new ForceEntry();
            await xmlReader.DeserializeAttributesAsync(forceEntry, forceEntryAttributes);
            if (parent.ForceEntries == null) throw new Exception("ForceEntries has not been initialised");
            parent.ForceEntries.Add(forceEntry);

            while (isReading && await xmlReader.ReadAsync())
            {
                await xmlReader.MoveToContentAsync();
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (xmlReader.Name)
                        {
                            case "categoryLinks":
                                forceEntry.CategoryLinks ??= new List<CategoryLink>();
                                await xmlReader.DeserializeArrayAsync(
                                    forceEntry,
                                    "categoryLink",
                                    DeserializeCategoryLink);
                                break;

                            case "constraints":
                                forceEntry.Constraints ??= new List<Constraint>();
                                await xmlReader.DeserializeArrayAsync(
                                    forceEntry,
                                    "constraint",
                                    async (reader, _) => await reader.DeserializeSingleItemAsync(forceEntry.Constraints, constraintAttributes));
                                break;

                            case "forceEntries":
                                parent.ForceEntries ??= new List<ForceEntry>();
                                await xmlReader.DeserializeArrayAsync(
                                    parent,
                                    "forceEntry",
                                    DeserializeForceEntry);
                                break;

                            case "modifiers":
                                forceEntry.Modifiers ??= new List<Modifier>();
                                await xmlReader.DeserializeArrayAsync(
                                    forceEntry,
                                    "modifier",
                                    DeserializeModifier);
                                break;

                            case "rules":
                                await xmlReader.DeserializeArrayAsync(
                                    forceEntry,
                                    "rule",
                                    CommonDeserialization.DeserializeRuleAsync);
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

        private static async Task DeserializeGameSystemAsync(XmlReader xmlReader, GameSystem gameSystem)
        {
            var name = xmlReader.Name;
            var isReading = !xmlReader.IsEmptyElement;
            await xmlReader.DeserializeAttributesAsync(gameSystem, gameSystemAttributes);

            while (isReading && await xmlReader.ReadAsync())
            {
                await xmlReader.MoveToContentAsync();
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (xmlReader.Name)
                        {
                            case "categoryEntries":
                                gameSystem.CategoryEntries ??= new List<CategoryEntry>();
                                await xmlReader.DeserializeArrayAsync(
                                    gameSystem,
                                    "categoryEntry",
                                    DeserializeCategoryEntry);
                                break;

                            case "costTypes":
                                gameSystem.CostTypes ??= new List<CostType>();
                                await xmlReader.DeserializeArrayAsync(
                                    gameSystem,
                                    "costType",
                                    async (reader, _) => await reader.DeserializeSingleItemAsync(gameSystem.CostTypes, costTypeAttributes));
                                break;

                            case "entryLinks":
                                gameSystem.EntryLinks ??= new List<EntryLink>();
                                await xmlReader.DeserializeArrayAsync(
                                    gameSystem,
                                    "entryLink",
                                    DeserializeEntryLink);
                                break;

                            case "forceEntries":
                                gameSystem.ForceEntries ??= new List<ForceEntry>();
                                await xmlReader.DeserializeArrayAsync(
                                    gameSystem,
                                    "forceEntry",
                                    DeserializeForceEntry);
                                break;

                            case "profileTypes":
                                gameSystem.ProfileTypes ??= new List<ProfileType>();
                                await xmlReader.DeserializeArrayAsync(
                                    gameSystem,
                                    "profileType",
                                    DeserializeProfileType);
                                break;

                            case "publications":
                                gameSystem.Publications ??= new List<Publication>();
                                await xmlReader.DeserializeArrayAsync(
                                    gameSystem,
                                    "publication",
                                    async (reader, _) => await reader.DeserializeSingleItemAsync(gameSystem.Publications, CommonDeserialization.PublicationAttributes));
                                break;

                            case "readme":
                                gameSystem.ReadMe = await xmlReader.ReadElementContentAsStringAsync();
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
        /// Deserialize a modifier.
        /// </summary>
        /// <param name="xmlReader">The <see cref="XmlReader"/> containing the definition to deserialize.</param>
        /// <param name="parent">The <see cref="GameSystem"/> to populate.</param>
        private static async Task DeserializeModifier(XmlReader xmlReader, IModifiersParent parent)
        {
            var name = xmlReader.Name;
            var isReading = !xmlReader.IsEmptyElement;
            var modifier = new Modifier();
            await xmlReader.DeserializeAttributesAsync(modifier, modifierAttributes);
            if (parent.Modifiers == null) throw new Exception("Modifiers has not been initialised");
            parent.Modifiers.Add(modifier);

            while (isReading && await xmlReader.ReadAsync())
            {
                await xmlReader.MoveToContentAsync();
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (xmlReader.Name)
                        {
                            case "conditionGroups":
                                modifier.ConditionGroups ??= new List<ConditionGroup>();
                                await xmlReader.DeserializeArrayAsync(
                                    modifier,
                                    "conditionGroup",
                                    DeserializeConditionGroup);
                                break;

                            case "conditions":
                                modifier.Conditions ??= new List<Constraint>();
                                await xmlReader.DeserializeArrayAsync(
                                    modifier,
                                    "condition",
                                    async (reader, _) => await reader.DeserializeSingleItemAsync(modifier.Conditions, constraintAttributes));
                                break;

                            case "repeats":
                                modifier.Repeats ??= new List<Constraint>();
                                await xmlReader.DeserializeArrayAsync(
                                    modifier,
                                    "repeat",
                                    async (reader, _) => await reader.DeserializeSingleItemAsync(modifier.Repeats, constraintAttributes));
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
        /// Deserialize a profile type.
        /// </summary>
        /// <param name="xmlReader">The <see cref="XmlReader"/> containing the definition to deserialize.</param>
        /// <param name="parent">The <see cref="GameSystem"/> to populate.</param>
        private static async Task DeserializeProfileType(XmlReader xmlReader, GameSystem parent)
        {
            var name = xmlReader.Name;
            var isReading = !xmlReader.IsEmptyElement;
            var profileType = new ProfileType();
            await xmlReader.DeserializeAttributesAsync(profileType, profileTypeAttributes);
            if (parent.ProfileTypes == null) throw new Exception("ProfileTypes has not been initialised");
            parent.ProfileTypes.Add(profileType);

            while (isReading && await xmlReader.ReadAsync())
            {
                await xmlReader.MoveToContentAsync();
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (xmlReader.Name)
                        {
                            case "characteristicTypes":
                                profileType.CharacteristicTypes ??= new List<CharacteristicType>();
                                await xmlReader.DeserializeArrayAsync(
                                    profileType,
                                    "characteristicType",
                                    async (reader, _) => await reader.DeserializeSingleItemAsync(profileType.CharacteristicTypes, characteristicTypeAttributes));
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