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
            var isReading = !xmlReader.IsEmptyElement;
            var categoryEntry = new CategoryEntry();
            await xmlReader.DeserializeAttributesAsync(categoryEntry, categoryEntryAttributes, "categoryEntry");
            parent.CategoryEntries ??= new List<CategoryEntry>();
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
                                    "constraints",
                                    "constraint",
                                    async (reader, _) => await reader.DeserializeSingleItemAsync(categoryEntry.Constraints, "constraint", constraintAttributes));
                                break;

                            case "modifiers":
                                categoryEntry.Modifiers ??= new List<Modifier>();
                                await xmlReader.DeserializeArrayAsync(
                                    categoryEntry,
                                    "modifiers",
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
                        if (xmlReader.Name != "categoryEntry") throw new Exception($"Unexpected end node: expected categoryEntry, found {xmlReader.Name}");
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
            await xmlReader.DeserializeAttributesAsync(conditionGroup, conditionGroupAttributes, "conditionGroup");
            parent.ConditionGroups ??= new List<ConditionGroup>();
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
                                    "conditionGroups",
                                    "conditionGroup",
                                    DeserializeConditionGroup);
                                break;

                            case "conditions":
                                conditionGroup.Conditions ??= new List<Constraint>();
                                await xmlReader.DeserializeArrayAsync(
                                    conditionGroup,
                                    "conditions",
                                    "condition",
                                    async (reader, _) => await reader.DeserializeSingleItemAsync(conditionGroup.Conditions, "condition", constraintAttributes));
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
            var isReading = !xmlReader.IsEmptyElement;
            var forceEntry = new ForceEntry();
            await xmlReader.DeserializeAttributesAsync(forceEntry, forceEntryAttributes, "forceEntry");
            parent.ForceEntries ??= new List<ForceEntry>();
            parent.ForceEntries.Add(forceEntry);

            while (isReading && await xmlReader.ReadAsync())
            {
                await xmlReader.MoveToContentAsync();
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (xmlReader.Name)
                        {
                            case "constraints":
                                forceEntry.Constraints ??= new List<Constraint>();
                                await xmlReader.DeserializeArrayAsync(
                                    forceEntry,
                                    "constraints",
                                    "constraint",
                                    async (reader, _) => await reader.DeserializeSingleItemAsync(forceEntry.Constraints, "constraint", constraintAttributes));
                                break;

                            case "forceEntries":
                                parent.ForceEntries ??= new List<ForceEntry>();
                                await xmlReader.DeserializeArrayAsync(
                                    parent,
                                    "forceEntries",
                                    "forceEntry",
                                    DeserializeForceEntry);
                                break;

                            case "modifiers":
                                forceEntry.Modifiers ??= new List<Modifier>();
                                await xmlReader.DeserializeArrayAsync(
                                    forceEntry,
                                    "modifiers",
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
                        if (xmlReader.Name != "categoryEntry") throw new Exception($"Unexpected end node: expected categoryEntry, found {xmlReader.Name}");
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
            var isReading = !xmlReader.IsEmptyElement;
            await xmlReader.DeserializeAttributesAsync(gameSystem, gameSystemAttributes, "gameSystem");

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
                                    "categoryEntries",
                                    "categoryEntry",
                                    DeserializeCategoryEntry);
                                break;

                            case "costTypes":
                                gameSystem.CostTypes ??= new List<CostType>();
                                await xmlReader.DeserializeArrayAsync(
                                    gameSystem,
                                    "costTypes",
                                    "costType",
                                    async (reader, _) => await reader.DeserializeSingleItemAsync(gameSystem.CostTypes, "costType", costTypeAttributes));
                                break;

                            case "forceEntries":
                                gameSystem.ForceEntries ??= new List<ForceEntry>();
                                await xmlReader.DeserializeArrayAsync(
                                    gameSystem,
                                    "forceEntries",
                                    "forceEntry",
                                    DeserializeForceEntry);
                                break;

                            case "profileTypes":
                                gameSystem.ProfileTypes ??= new List<ProfileType>();
                                await xmlReader.DeserializeArrayAsync(
                                    gameSystem,
                                    "profileTypes",
                                    "profileType",
                                    DeserializeProfileType);
                                break;

                            case "publications":
                                gameSystem.Publications ??= new List<Publication>();
                                await xmlReader.DeserializeArrayAsync(
                                    gameSystem,
                                    "publications",
                                    "publication",
                                    async (reader, _) => await reader.DeserializeSingleItemAsync(gameSystem.Publications, "publication", CommonDeserialization.PublicationAttributes));
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
                        if (xmlReader.Name != "gameSystem") throw new Exception($"Unexpected end node: expected gameSystem, found {xmlReader.Name}");
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
            var isReading = !xmlReader.IsEmptyElement;
            var modifier = new Modifier();
            await xmlReader.DeserializeAttributesAsync(modifier, modifierAttributes, "modifier");
            parent.Modifiers ??= new List<Modifier>();
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
                                    "conditionGroups",
                                    "conditionGroup",
                                    DeserializeConditionGroup);
                                break;

                            case "conditions":
                                modifier.Conditions ??= new List<Constraint>();
                                await xmlReader.DeserializeArrayAsync(
                                    modifier,
                                    "conditions",
                                    "condition",
                                    async (reader, _) => await reader.DeserializeSingleItemAsync(modifier.Conditions, "condition", constraintAttributes));
                                break;

                            case "repeats":
                                modifier.Repeats ??= new List<Constraint>();
                                await xmlReader.DeserializeArrayAsync(
                                    modifier,
                                    "repeats",
                                    "repeat",
                                    async (reader, _) => await reader.DeserializeSingleItemAsync(modifier.Repeats, "repeat", constraintAttributes));
                                break;

                            default:
                                // Anything else is an error
                                throw new Exception($"Unexpected element: {xmlReader.Name}");
                        }
                        break;

                    case XmlNodeType.EndElement:
                        // Make sure we've got the correct end element
                        if (xmlReader.Name != "modifier") throw new Exception($"Unexpected end node: expected modifier, found {xmlReader.Name}");
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
            var isReading = !xmlReader.IsEmptyElement;
            var profileType = new ProfileType();
            await xmlReader.DeserializeAttributesAsync(profileType, profileTypeAttributes, "profileType");
            parent.ProfileTypes ??= new List<ProfileType>();
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
                                    "characteristicTypes",
                                    "characteristicType",
                                    async (reader, _) => await reader.DeserializeSingleItemAsync(profileType.CharacteristicTypes, "characteristicType", characteristicTypeAttributes));
                                break;

                            default:
                                // Anything else is an error
                                throw new Exception($"Unexpected element: {xmlReader.Name}");
                        }
                        break;

                    case XmlNodeType.EndElement:
                        // Make sure we've got the correct end element
                        if (xmlReader.Name != "profileType") throw new Exception($"Unexpected end node: expected profileType, found {xmlReader.Name}");
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