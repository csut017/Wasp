using System.Xml;

namespace Wasp.Core.Data.Xml
{
    /// <summary>
    /// Serializes a definition to an XML format.
    /// </summary>
    internal static class ConfigurationSerialization
    {
        /// <summary>
        /// Serializes the root level element.
        /// </summary>
        /// <param name="configuration">The configuration to serialize.</param>
        /// <param name="writer">The writer to use.</param>
        public static async Task SerializeRootAsync(GameSystemConfiguration configuration, TextWriter writer)
        {
            var settings = new XmlWriterSettings
            {
                Async = true,
                Indent = true
            };

            using var xmlWriter = XmlWriter.Create(writer, settings);
            await xmlWriter.WriteStartDocumentAsync(true);
            await xmlWriter.WriteStartElementAsync(null, "catalogue", Constants.CatalogueXmlNamespace);
            await WriteConfigurationEntryAsync(xmlWriter, configuration, async (_, _) =>
            {
                await CommonSerialization.WriteAttributeAsync(xmlWriter, "revision", configuration.Revision);
                await CommonSerialization.WriteAttributeAsync(xmlWriter, "battleScribeVersion", configuration.BattleScribeVersion);
                await CommonSerialization.WriteAttributeAsync(xmlWriter, "authorName", configuration.AuthorName);
                await CommonSerialization.WriteAttributeAsync(xmlWriter, "authorContact", configuration.AuthorContact);
                await CommonSerialization.WriteAttributeAsync(xmlWriter, "authorUrl", configuration.AuthorUrl);
                await CommonSerialization.WriteAttributeAsync(xmlWriter, "library", configuration.IsLibrary);
                await CommonSerialization.WriteAttributeAsync(xmlWriter, "gameSystemId", configuration.GameSystemId);
                await CommonSerialization.WriteAttributeAsync(xmlWriter, "gameSystemRevision", configuration.GameSystemRevision);
                await xmlWriter.WriteAttributeStringAsync(null, "xmlns", null, Constants.CatalogueXmlNamespace);
            });

            await CommonSerialization.WritePublicationsAsync(xmlWriter, configuration.Publications);
            await WriteProfileTypesAsync(xmlWriter, configuration.ProfileTypes);
            await WriteCategoryEntriesAsync(xmlWriter, configuration.CategoryEntries);
            await WriteEntryLinksAsync(xmlWriter, configuration.EntryLinks);
            await CommonSerialization.WriteRulesAsync(xmlWriter, configuration.Rules, "rules", "rule");
            await WriteSelectionEntriesAsync(xmlWriter, configuration.SharedSelectionEntries, "sharedSelectionEntries", "selectionEntry");
            await WriteSelectionEntryGroupsAsync(xmlWriter, configuration.SharedSelectionEntryGroups, "sharedSelectionEntryGroups", "selectionEntryGroup");
            await CommonSerialization.WriteRulesAsync(xmlWriter, configuration.SharedRules, "sharedRules", "rule");
            await CommonSerialization.WriteProfilesAsync(xmlWriter, configuration.SharedProfiles, "sharedProfiles", "profile");

            await xmlWriter.WriteEndElementAsync();
            await xmlWriter.WriteEndDocumentAsync();
        }

        /// <summary>
        /// Writes the category entries.
        /// </summary>
        /// <param name="xmlWriter">The <see cref="XmlWriter"/> to use.</param>
        /// <param name="parent">The parent item containing the data to write.</param>
        private static async Task WriteCategoryEntriesAsync(XmlWriter xmlWriter, List<CategoryEntry>? parent)
        {
            if (parent == null) return;

            await xmlWriter.WriteStartElementAsync(null, "categoryEntries", null);
            foreach (var item in parent)
            {
                await xmlWriter.WriteStartElementAsync(null, "categoryEntry", null);
                await WriteConfigurationEntryAsync(xmlWriter, item, async (_, _) =>
                {
                    await CommonSerialization.WriteAttributeAsync(xmlWriter, "publicationId", item.PublicationId);
                    await CommonSerialization.WriteAttributeAsync(xmlWriter, "page", item.Page);
                    await CommonSerialization.WriteAttributeAsync(xmlWriter, "hidden", item.IsHidden);
                });
                await WriteInformationLinksAsync(xmlWriter, item.InformationLinks);
                await CommonSerialization.WriteModifiersAsync(xmlWriter, item.Modifiers);
                await CommonSerialization.WriteConstraintsAsync(xmlWriter, item.Constraints, "constraints", "constraint");

                await xmlWriter.WriteEndElementAsync();
            }
            await xmlWriter.WriteEndElementAsync();
            await xmlWriter.FlushAsync();
        }

        /// <summary>
        /// Writes the category links.
        /// </summary>
        /// <param name="xmlWriter">The <see cref="XmlWriter"/> to use.</param>
        /// <param name="parent">The parent item containing the data to write.</param>
        private static async Task WriteCategoryLinksAsync(XmlWriter xmlWriter, List<CategoryLink>? parent)
        {
            if (parent == null) return;

            await xmlWriter.WriteStartElementAsync(null, "categoryLinks", null);
            foreach (var item in parent)
            {
                await xmlWriter.WriteStartElementAsync(null, "categoryLink", null);
                await WriteConfigurationEntryAsync(xmlWriter, item, async (_, _) =>
                {
                    await CommonSerialization.WriteAttributeAsync(xmlWriter, "hidden", item.IsHidden);
                    await CommonSerialization.WriteAttributeAsync(xmlWriter, "targetId", item.TargetId);
                    await CommonSerialization.WriteAttributeAsync(xmlWriter, "primary", item.IsPrimary);
                });

                await xmlWriter.WriteEndElementAsync();
            }
            await xmlWriter.WriteEndElementAsync();
            await xmlWriter.FlushAsync();
        }

        /// <summary>
        /// Writes the characteristic types.
        /// </summary>
        /// <param name="xmlWriter">The <see cref="XmlWriter"/> to use.</param>
        /// <param name="parent">The parent item containing the data to write.</param>
        private static async Task WriteCharacteristicTypesAsync(XmlWriter xmlWriter, List<CharacteristicType>? parent)
        {
            if (parent == null) return;

            await xmlWriter.WriteStartElementAsync(null, "characteristicTypes", null);
            foreach (var item in parent)
            {
                await xmlWriter.WriteStartElementAsync(null, "characteristicType", null);
                await WriteConfigurationEntryAsync(xmlWriter, item);
                await xmlWriter.WriteEndElementAsync();
            }
            await xmlWriter.WriteEndElementAsync();
            await xmlWriter.FlushAsync();
        }

        private static async Task WriteConfigurationEntryAsync<TItem>(XmlWriter xmlWriter, TItem item, Action<XmlWriter, TItem>? writeAttributes = null)
                                                    where TItem : ConfigurationEntry
        {
            await CommonSerialization.WriteAttributeAsync(xmlWriter, "id", item.Id);
            await CommonSerialization.WriteAttributeAsync(xmlWriter, "name", item.Name);
            writeAttributes?.Invoke(xmlWriter, item);
            await CommonSerialization.WriteComment(xmlWriter, item);
        }

        /// <summary>
        /// Writes the entry links.
        /// </summary>
        /// <param name="xmlWriter">The <see cref="XmlWriter"/> to use.</param>
        /// <param name="parent">The parent item containing the data to write.</param>
        private static async Task WriteEntryLinksAsync(XmlWriter xmlWriter, List<EntryLink>? parent)
        {
            if (parent == null) return;

            await xmlWriter.WriteStartElementAsync(null, "entryLinks", null);
            foreach (var item in parent)
            {
                await xmlWriter.WriteStartElementAsync(null, "entryLink", null);
                await WriteConfigurationEntryAsync(xmlWriter, item, async (_, _) =>
                {
                    await CommonSerialization.WriteAttributeAsync(xmlWriter, "publicationId", item.PublicationId);
                    await CommonSerialization.WriteAttributeAsync(xmlWriter, "page", item.Page);
                    await CommonSerialization.WriteAttributeAsync(xmlWriter, "hidden", item.IsHidden);
                    await CommonSerialization.WriteAttributeAsync(xmlWriter, "collective", item.IsCollective);
                    await CommonSerialization.WriteAttributeAsync(xmlWriter, "import", item.IsImport);
                    await CommonSerialization.WriteAttributeAsync(xmlWriter, "targetId", item.TargetId);
                    await CommonSerialization.WriteAttributeAsync(xmlWriter, "type", item.Type);
                });
                await WriteModifierGroupsAsync(xmlWriter, item.ModifierGroups);
                await CommonSerialization.WriteModifiersAsync(xmlWriter, item.Modifiers);
                await CommonSerialization.WriteConstraintsAsync(xmlWriter, item.Constraints, "constraints", "constraint");
                await WriteEntryLinksAsync(xmlWriter, item.EntryLinks);
                await WriteCategoryLinksAsync(xmlWriter, item.CategoryLinks);
                await CommonSerialization.WriteCostsAsync(xmlWriter, item.Costs);

                await xmlWriter.WriteEndElementAsync();
            }
            await xmlWriter.WriteEndElementAsync();
            await xmlWriter.FlushAsync();
        }

        /// <summary>
        /// Writes the information links.
        /// </summary>
        /// <param name="xmlWriter">The <see cref="XmlWriter"/> to use.</param>
        /// <param name="parent">The parent item containing the data to write.</param>
        private static async Task WriteInformationLinksAsync(XmlWriter xmlWriter, List<InformationLink>? parent)
        {
            if (parent == null) return;

            await xmlWriter.WriteStartElementAsync(null, "infoLinks", null);
            foreach (var item in parent)
            {
                await xmlWriter.WriteStartElementAsync(null, "infoLink", null);
                await WriteConfigurationEntryAsync(xmlWriter, item, async (_, _) =>
                {
                    await CommonSerialization.WriteAttributeAsync(xmlWriter, "publicationId", item.PublicationId);
                    await CommonSerialization.WriteAttributeAsync(xmlWriter, "page", item.Page);
                    await CommonSerialization.WriteAttributeAsync(xmlWriter, "hidden", item.IsHidden);
                    await CommonSerialization.WriteAttributeAsync(xmlWriter, "targetId", item.TargetId);
                    await CommonSerialization.WriteAttributeAsync(xmlWriter, "type", item.Type);
                });
                await CommonSerialization.WriteModifiersAsync(xmlWriter, item.Modifiers);
                await xmlWriter.WriteEndElementAsync();
            }
            await xmlWriter.WriteEndElementAsync();
            await xmlWriter.FlushAsync();
        }

        /// <summary>
        /// Writes the modifier groups.
        /// </summary>
        /// <param name="xmlWriter">The <see cref="XmlWriter"/> to use.</param>
        /// <param name="parent">The parent item containing the data to write.</param>
        private static async Task WriteModifierGroupsAsync(XmlWriter xmlWriter, List<ModifierGroup>? parent)
        {
            if (parent == null) return;

            await xmlWriter.WriteStartElementAsync(null, "modifierGroups", null);
            foreach (var item in parent)
            {
                await xmlWriter.WriteStartElementAsync(null, "modifierGroup", null);
                await CommonSerialization.WriteModifiersAsync(xmlWriter, item.Modifiers);

                await xmlWriter.WriteEndElementAsync();
            }
            await xmlWriter.WriteEndElementAsync();
            await xmlWriter.FlushAsync();
        }

        /// <summary>
        /// Writes the profile types.
        /// </summary>
        /// <param name="xmlWriter">The <see cref="XmlWriter"/> to use.</param>
        /// <param name="parent">The parent item containing the data to write.</param>
        private static async Task WriteProfileTypesAsync(XmlWriter xmlWriter, List<ProfileType>? parent)
        {
            if (parent == null) return;

            await xmlWriter.WriteStartElementAsync(null, "profileTypes", null);
            foreach (var item in parent)
            {
                await xmlWriter.WriteStartElementAsync(null, "profileType", null);
                await WriteConfigurationEntryAsync(xmlWriter, item);
                await WriteCharacteristicTypesAsync(xmlWriter, item.CharacteristicTypes);
                await xmlWriter.WriteEndElementAsync();
            }
            await xmlWriter.WriteEndElementAsync();
            await xmlWriter.FlushAsync();
        }

        /// <summary>
        /// Writes the selection entries.
        /// </summary>
        /// <param name="xmlWriter">The <see cref="XmlWriter"/> to use.</param>
        /// <param name="parent">The parent item containing the data to write.</param>
        /// <param name="arrayName">The name of the list.</param>
        /// <param name="itemName">The name of each item.</param>
        private static async Task WriteSelectionEntriesAsync(XmlWriter xmlWriter, List<SelectionEntry>? parent, string arrayName, string itemName)
        {
            if (parent == null) return;

            await xmlWriter.WriteStartElementAsync(null, arrayName, null);
            foreach (var item in parent)
            {
                await xmlWriter.WriteStartElementAsync(null, itemName, null);
                await WriteConfigurationEntryAsync(xmlWriter, item, async (_, _) =>
                {
                    await CommonSerialization.WriteAttributeAsync(xmlWriter, "publicationId", item.PublicationId);
                    await CommonSerialization.WriteAttributeAsync(xmlWriter, "page", item.Page);
                    await CommonSerialization.WriteAttributeAsync(xmlWriter, "hidden", item.IsHidden);
                    await CommonSerialization.WriteAttributeAsync(xmlWriter, "collective", item.IsCollective);
                    await CommonSerialization.WriteAttributeAsync(xmlWriter, "import", item.IsImport);
                    await CommonSerialization.WriteAttributeAsync(xmlWriter, "type", item.Type);
                });
                await CommonSerialization.WriteModifiersAsync(xmlWriter, item.Modifiers);
                await CommonSerialization.WriteConstraintsAsync(xmlWriter, item.Constraints, "constraints", "constraint");
                await CommonSerialization.WriteProfilesAsync(xmlWriter, item.Profiles, "profiles", "profile");
                await CommonSerialization.WriteRulesAsync(xmlWriter, item.Rules, "rules", "rule");
                await WriteInformationLinksAsync(xmlWriter, item.InformationLinks);
                await WriteCategoryLinksAsync(xmlWriter, item.CategoryLinks);
                await WriteSelectionEntriesAsync(xmlWriter, item.SelectionEntries, "selectionEntries", "selectionEntry");
                await WriteSelectionEntryGroupsAsync(xmlWriter, item.SelectionEntryGroups, "selectionEntryGroups", "selectionEntryGroup");
                await WriteEntryLinksAsync(xmlWriter, item.EntryLinks);
                await CommonSerialization.WriteCostsAsync(xmlWriter, item.Costs);

                await xmlWriter.WriteEndElementAsync();
            }
            await xmlWriter.WriteEndElementAsync();
            await xmlWriter.FlushAsync();
        }

        /// <summary>
        /// Writes the selection entry groups.
        /// </summary>
        /// <param name="xmlWriter">The <see cref="XmlWriter"/> to use.</param>
        /// <param name="parent">The parent item containing the data to write.</param>
        /// <param name="arrayName">The name of the list.</param>
        /// <param name="itemName">The name of each item.</param>
        private static async Task WriteSelectionEntryGroupsAsync(XmlWriter xmlWriter, List<SelectionEntryGroup>? parent, string arrayName, string itemName)
        {
            if (parent == null) return;

            await xmlWriter.WriteStartElementAsync(null, arrayName, null);
            foreach (var item in parent)
            {
                await xmlWriter.WriteStartElementAsync(null, itemName, null);
                await WriteConfigurationEntryAsync(xmlWriter, item, async (_, _) =>
                {
                    await CommonSerialization.WriteAttributeAsync(xmlWriter, "publicationId", item.PublicationId);
                    await CommonSerialization.WriteAttributeAsync(xmlWriter, "page", item.Page);
                    await CommonSerialization.WriteAttributeAsync(xmlWriter, "hidden", item.IsHidden);
                    await CommonSerialization.WriteAttributeAsync(xmlWriter, "collective", item.IsCollective);
                    await CommonSerialization.WriteAttributeAsync(xmlWriter, "import", item.IsImport);
                    await CommonSerialization.WriteAttributeAsync(xmlWriter, "defaultSelectionEntryId", item.DefaultSelectionEntryId);
                });
                await CommonSerialization.WriteModifiersAsync(xmlWriter, item.Modifiers);
                await CommonSerialization.WriteConstraintsAsync(xmlWriter, item.Constraints, "constraints", "constraint");
                await WriteSelectionEntriesAsync(xmlWriter, item.SelectionEntries, "selectionEntries", "selectionEntry");
                await WriteSelectionEntryGroupsAsync(xmlWriter, item.SelectionEntryGroups, "selectionEntryGroups", "selectionEntryGroup");
                await WriteEntryLinksAsync(xmlWriter, item.EntryLinks);

                await xmlWriter.WriteEndElementAsync();
            }
            await xmlWriter.WriteEndElementAsync();
            await xmlWriter.FlushAsync();
        }
    }
}