using System.Xml;

namespace Wasp.Core.Data.Xml
{
    /// <summary>
    /// Serializes a definition to an XML format.
    /// </summary>
    internal static class RosterSerialization
    {
        /// <summary>
        /// Serializes the root level element.
        /// </summary>
        /// <param name="roster">The roster to serialize.</param>
        /// <param name="writer">The writer to use.</param>
        public static async Task SerializeRootAsync(Roster roster, TextWriter writer)
        {
            var settings = new XmlWriterSettings
            {
                Async = true,
                Indent = true
            };

            using var xmlWriter = XmlWriter.Create(writer, settings);
            await xmlWriter.WriteStartDocumentAsync(true);
            await xmlWriter.WriteStartElementAsync(null, "roster", Constants.RosterXmlNamespace);
            await CommonSerialization.WriteAttributeAsync(xmlWriter, "id", roster.Id);
            await CommonSerialization.WriteAttributeAsync(xmlWriter, "name", roster.Name);
            await CommonSerialization.WriteAttributeAsync(xmlWriter, "battleScribeVersion", roster.BattleScribeVersion);
            await CommonSerialization.WriteAttributeAsync(xmlWriter, "gameSystemId", roster.GameSystemId);
            await CommonSerialization.WriteAttributeAsync(xmlWriter, "gameSystemName", roster.GameSystemName);
            await CommonSerialization.WriteAttributeAsync(xmlWriter, "gameSystemRevision", roster.GameSystemRevision);
            await xmlWriter.WriteAttributeStringAsync(null, "xmlns", null, Constants.RosterXmlNamespace);

            await CommonSerialization.WriteCostsAsync(xmlWriter, roster.Costs);
            await CommonSerialization.WriteCostsAsync(xmlWriter, roster.CostLimits, "costLimit");
            await WriteForcesAsync(xmlWriter, roster);

            await xmlWriter.WriteEndElementAsync();
            await xmlWriter.WriteEndDocumentAsync();
        }

        /// <summary>
        /// Writes the categories for an item.
        /// </summary>
        /// <param name="xmlWriter">The <see cref="XmlWriter"/> to use.</param>
        /// <param name="categories">The element containing the categories.</param>
        private static async Task WriteCategoriesAsync(XmlWriter xmlWriter, List<Category>? categories)
        {
            if (categories == null) return;

            await xmlWriter.WriteStartElementAsync(null, "categories", null);
            foreach (var category in categories)
            {
                await xmlWriter.WriteStartElementAsync(null, "category", null);
                await CommonSerialization.WriteAttributeAsync(xmlWriter, "id", category.Id);
                await CommonSerialization.WriteAttributeAsync(xmlWriter, "name", category.Name);
                await CommonSerialization.WriteAttributeAsync(xmlWriter, "entryId", category.EntryId);
                await CommonSerialization.WriteAttributeAsync(xmlWriter, "primary", category.IsPrimary);

                await xmlWriter.WriteEndElementAsync();
            }
            await xmlWriter.WriteEndElementAsync();
        }

        /// <summary>
        /// Writes the characteristics for an item.
        /// </summary>
        /// <param name="xmlWriter">The <see cref="XmlWriter"/> to use.</param>
        /// <param name="parent">The element containing the characteristics.</param>
        private static async Task WriteCharacteristicsAsync(XmlWriter xmlWriter, Profile? parent)
        {
            if (parent?.Characteristics == null) return;

            await xmlWriter.WriteStartElementAsync(null, "characteristics", null);
            foreach (var characteristic in parent.Characteristics)
            {
                await xmlWriter.WriteStartElementAsync(null, "characteristic", null);
                await CommonSerialization.WriteAttributeAsync(xmlWriter, "name", characteristic.Name);
                await CommonSerialization.WriteAttributeAsync(xmlWriter, "typeId", characteristic.TypeId);
                if (characteristic.Value != null)
                {
                    await xmlWriter.WriteStringAsync(characteristic.Value);
                }

                await xmlWriter.WriteEndElementAsync();
            }
            await xmlWriter.WriteEndElementAsync();
        }

        /// <summary>
        /// Writes the forces for a roster.
        /// </summary>
        /// <param name="xmlWriter">The <see cref="XmlWriter"/> to use.</param>
        /// <param name="roster">The roster containing the forces.</param>
        private static async Task WriteForcesAsync(XmlWriter xmlWriter, Roster? roster)
        {
            if (roster?.Forces == null) return;

            await xmlWriter.WriteStartElementAsync(null, "forces", null);
            foreach (var force in roster.Forces)
            {
                await xmlWriter.WriteStartElementAsync(null, "force", null);
                await CommonSerialization.WriteAttributeAsync(xmlWriter, "id", force.Id);
                await CommonSerialization.WriteAttributeAsync(xmlWriter, "name", force.Name);
                await CommonSerialization.WriteAttributeAsync(xmlWriter, "entryId", force.EntryId);
                await CommonSerialization.WriteAttributeAsync(xmlWriter, "catalogueId", force.CatalogueId);
                await CommonSerialization.WriteAttributeAsync(xmlWriter, "catalogueRevision", force.CatalogueRevision);
                await CommonSerialization.WriteAttributeAsync(xmlWriter, "catalogueName", force.CatalogueName);

                await CommonSerialization.WriteRulesAsync(xmlWriter, force.Rules);
                await WriteSelectionsAsync(xmlWriter, force.Selections);
                await CommonSerialization.WritePublicationsAsync(xmlWriter, force.Publications);
                await WriteCategoriesAsync(xmlWriter, force.Categories);

                await xmlWriter.WriteEndElementAsync();
            }
            await xmlWriter.WriteEndElementAsync();
        }

        /// <summary>
        /// Writes the profiles for an item.
        /// </summary>
        /// <param name="xmlWriter">The <see cref="XmlWriter"/> to use.</param>
        /// <param name="parent">The element containing the profiles.</param>
        private static async Task WriteProfilesAsync(XmlWriter xmlWriter, Selection? parent)
        {
            if (parent?.Profiles == null) return;

            await xmlWriter.WriteStartElementAsync(null, "profiles", null);
            foreach (var profile in parent.Profiles)
            {
                await xmlWriter.WriteStartElementAsync(null, "profile", null);
                await CommonSerialization.WriteAttributeAsync(xmlWriter, "id", profile.Id);
                await CommonSerialization.WriteAttributeAsync(xmlWriter, "name", profile.Name);
                await CommonSerialization.WriteAttributeAsync(xmlWriter, "publicationId", profile.PublicationId);
                await CommonSerialization.WriteAttributeAsync(xmlWriter, "page", profile.Page);
                await CommonSerialization.WriteAttributeAsync(xmlWriter, "hidden", profile.IsHidden);
                await CommonSerialization.WriteAttributeAsync(xmlWriter, "typeId", profile.TypeId);
                await CommonSerialization.WriteAttributeAsync(xmlWriter, "typeName", profile.TypeName);

                await WriteCharacteristicsAsync(xmlWriter, profile);

                await xmlWriter.WriteEndElementAsync();
            }
            await xmlWriter.WriteEndElementAsync();
        }

        /// <summary>
        /// Writes the selections for an item.
        /// </summary>
        /// <param name="xmlWriter">The <see cref="XmlWriter"/> to use.</param>
        /// <param name="parent">The element containing the selections.</param>
        private static async Task WriteSelectionsAsync(XmlWriter xmlWriter, List<Selection>? parent)
        {
            if (parent == null) return;

            await xmlWriter.WriteStartElementAsync(null, "selections", null);
            foreach (var selection in parent)
            {
                await xmlWriter.WriteStartElementAsync(null, "selection", null);
                await CommonSerialization.WriteAttributeAsync(xmlWriter, "id", selection.Id);
                await CommonSerialization.WriteAttributeAsync(xmlWriter, "name", selection.Name);
                await CommonSerialization.WriteAttributeAsync(xmlWriter, "entryId", selection.EntryId);
                await CommonSerialization.WriteAttributeAsync(xmlWriter, "entryGroupId", selection.EntryGroupId);
                await CommonSerialization.WriteAttributeAsync(xmlWriter, "number", selection.Number);
                await CommonSerialization.WriteAttributeAsync(xmlWriter, "type", selection.Type);

                if (selection.CustomNotes != null) await xmlWriter.WriteElementStringAsync(null, "customNotes", null, selection.CustomNotes);

                await CommonSerialization.WriteRulesAsync(xmlWriter, selection.Rules);
                await WriteProfilesAsync(xmlWriter, selection);
                await WriteSelectionsAsync(xmlWriter, selection.Selections);
                await CommonSerialization.WriteCostsAsync(xmlWriter, selection.Costs);
                await WriteCategoriesAsync(xmlWriter, selection.Categories);

                await xmlWriter.WriteEndElementAsync();
            }
            await xmlWriter.WriteEndElementAsync();
        }
    }
}