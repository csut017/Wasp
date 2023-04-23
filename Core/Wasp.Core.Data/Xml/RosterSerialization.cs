using System.Xml;

namespace Wasp.Core.Data.Xml
{
    /// <summary>
    /// Deserializes a definition from an XML format.
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
            await xmlWriter.WriteStartElementAsync(null, "roster", Constants.XmlNamespace);
            await WriteAttributeAsync(xmlWriter, "id", roster.Id);
            await WriteAttributeAsync(xmlWriter, "name", roster.Name);
            await WriteAttributeAsync(xmlWriter, "battleScribeVersion", roster.BattleScribeVersion);
            await WriteAttributeAsync(xmlWriter, "gameSystemId", roster.GameSystemId);
            await WriteAttributeAsync(xmlWriter, "gameSystemName", roster.GameSystemName);
            await WriteAttributeAsync(xmlWriter, "gameSystemRevision", roster.GameSystemRevision);
            await xmlWriter.WriteAttributeStringAsync(null, "xmlns", null, Constants.XmlNamespace);

            await WriteCostsAsync(xmlWriter, roster.Costs);
            await WriteCostsAsync(xmlWriter, roster.CostLimits, "costLimit");
            await WriteForcesAsync(xmlWriter, roster);

            await xmlWriter.WriteEndElementAsync();
            await xmlWriter.WriteEndDocumentAsync();
        }

        /// <summary>
        /// Conditionally writes an attribute.
        /// </summary>
        /// <param name="xmlWriter">The <see cref="XmlWriter"/> to use.</param>
        /// <param name="name">The name of the attribute.</param>
        /// <param name="value">The value of the attribute.</param>
        private static async Task WriteAttributeAsync(XmlWriter xmlWriter, string name, string? value)
        {
            if (value == null) return;

            await xmlWriter.WriteAttributeStringAsync(null, name, null, value);
        }

        /// <summary>
        /// Conditionally writes an attribute.
        /// </summary>
        /// <param name="xmlWriter">The <see cref="XmlWriter"/> to use.</param>
        /// <param name="name">The name of the attribute.</param>
        /// <param name="value">The value of the attribute.</param>
        private static async Task WriteAttributeAsync(XmlWriter xmlWriter, string name, bool? value)
        {
            if (value == null) return;

            await xmlWriter.WriteAttributeStringAsync(null, name, null, value.Value ? "true" : "false");
        }

        /// <summary>
        /// Writes the categories for an item.
        /// </summary>
        /// <param name="xmlWriter">The <see cref="XmlWriter"/> to use.</param>
        /// <param name="parent">The element containing the categories.</param>
        private static async Task WriteCategoriesAsync(XmlWriter xmlWriter, ICategoriesParent? parent)
        {
            if (parent?.Categories == null) return;

            await xmlWriter.WriteStartElementAsync(null, "categories", null);
            foreach (var category in parent.Categories)
            {
                await xmlWriter.WriteStartElementAsync(null, "category", null);
                await WriteAttributeAsync(xmlWriter, "id", category.Id);
                await WriteAttributeAsync(xmlWriter, "name", category.Name);
                await WriteAttributeAsync(xmlWriter, "entryId", category.EntryId);
                await WriteAttributeAsync(xmlWriter, "primary", category.IsPrimary);

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
                await WriteAttributeAsync(xmlWriter, "name", characteristic.Name);
                await WriteAttributeAsync(xmlWriter, "typeId", characteristic.TypeId);
                if (characteristic.Value != null)
                {
                    await xmlWriter.WriteStringAsync(characteristic.Value);
                }

                await xmlWriter.WriteEndElementAsync();
            }
            await xmlWriter.WriteEndElementAsync();
        }

        /// <summary>
        /// Writes the costs for an item.
        /// </summary>
        /// <param name="xmlWriter">The <see cref="XmlWriter"/> to use.</param>
        /// <param name="costs">The element containing the costs.</param>
        /// <param name="itemName">The name of the item.</param>
        private static async Task WriteCostsAsync(XmlWriter xmlWriter, List<ItemCost>? costs, string itemName = "cost")
        {
            if (costs == null) return;

            await xmlWriter.WriteStartElementAsync(null, itemName + "s", null);
            foreach (var cost in costs)
            {
                await xmlWriter.WriteStartElementAsync(null, itemName, null);
                await WriteAttributeAsync(xmlWriter, "name", cost.Name);
                await WriteAttributeAsync(xmlWriter, "typeId", cost.TypeId);
                await WriteAttributeAsync(xmlWriter, "value", cost.Value);
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
                await WriteAttributeAsync(xmlWriter, "id", force.Id);
                await WriteAttributeAsync(xmlWriter, "name", force.Name);
                await WriteAttributeAsync(xmlWriter, "entryId", force.EntryId);
                await WriteAttributeAsync(xmlWriter, "catalogueId", force.CatalogueId);
                await WriteAttributeAsync(xmlWriter, "catalogueRevision", force.CatalogueRevision);
                await WriteAttributeAsync(xmlWriter, "catalogueName", force.CatalogueName);

                await WriteRulesAsync(xmlWriter, force);
                await WriteSelectionsAsync(xmlWriter, force);
                await WritePublicationsAsync(xmlWriter, force);
                await WriteCategoriesAsync(xmlWriter, force);

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
                await WriteAttributeAsync(xmlWriter, "id", profile.Id);
                await WriteAttributeAsync(xmlWriter, "name", profile.Name);
                await WriteAttributeAsync(xmlWriter, "publicationId", profile.PublicationId);
                await WriteAttributeAsync(xmlWriter, "page", profile.Page);
                await WriteAttributeAsync(xmlWriter, "hidden", profile.IsHidden);
                await WriteAttributeAsync(xmlWriter, "typeId", profile.TypeId);
                await WriteAttributeAsync(xmlWriter, "typeName", profile.TypeName);

                await WriteCharacteristicsAsync(xmlWriter, profile);

                await xmlWriter.WriteEndElementAsync();
            }
            await xmlWriter.WriteEndElementAsync();
        }

        /// <summary>
        /// Writes the publications for an item.
        /// </summary>
        /// <param name="xmlWriter">The <see cref="XmlWriter"/> to use.</param>
        /// <param name="parent">The element containing the publications.</param>
        private static async Task WritePublicationsAsync(XmlWriter xmlWriter, Force? parent)
        {
            if (parent?.Publications == null) return;

            await xmlWriter.WriteStartElementAsync(null, "publications", null);
            foreach (var publication in parent.Publications)
            {
                await xmlWriter.WriteStartElementAsync(null, "publication", null);
                await WriteAttributeAsync(xmlWriter, "id", publication.Id);
                await WriteAttributeAsync(xmlWriter, "name", publication.FullName);
                await xmlWriter.WriteEndElementAsync();
            }
            await xmlWriter.WriteEndElementAsync();
        }

        /// <summary>
        /// Writes the rules for an item.
        /// </summary>
        /// <param name="xmlWriter">The <see cref="XmlWriter"/> to use.</param>
        /// <param name="parent">The element containing the rules.</param>
        private static async Task WriteRulesAsync(XmlWriter xmlWriter, IRulesParent? parent)
        {
            if (parent?.Rules == null) return;

            await xmlWriter.WriteStartElementAsync(null, "rules", null);
            foreach (var rule in parent.Rules)
            {
                await xmlWriter.WriteStartElementAsync(null, "rule", null);
                await WriteAttributeAsync(xmlWriter, "id", rule.Id);
                await WriteAttributeAsync(xmlWriter, "name", rule.Name);
                await WriteAttributeAsync(xmlWriter, "publicationId", rule.PublicationId);
                await WriteAttributeAsync(xmlWriter, "page", rule.Page);
                await WriteAttributeAsync(xmlWriter, "hidden", rule.IsHidden);
                if (rule.Description != null)
                {
                    await xmlWriter.WriteElementStringAsync(null, "description", null, rule.Description);
                }

                await xmlWriter.WriteEndElementAsync();
            }
            await xmlWriter.WriteEndElementAsync();
        }

        /// <summary>
        /// Writes the selections for an item.
        /// </summary>
        /// <param name="xmlWriter">The <see cref="XmlWriter"/> to use.</param>
        /// <param name="parent">The element containing the selections.</param>
        private static async Task WriteSelectionsAsync(XmlWriter xmlWriter, ISelectionParent? parent)
        {
            if (parent?.Selections == null) return;

            await xmlWriter.WriteStartElementAsync(null, "selections", null);
            foreach (var selection in parent.Selections)
            {
                await xmlWriter.WriteStartElementAsync(null, "selection", null);
                await WriteAttributeAsync(xmlWriter, "id", selection.Id);
                await WriteAttributeAsync(xmlWriter, "name", selection.Name);
                await WriteAttributeAsync(xmlWriter, "entryId", selection.EntryId);
                await WriteAttributeAsync(xmlWriter, "entryGroupId", selection.EntryGroupId);
                await WriteAttributeAsync(xmlWriter, "number", selection.Number);
                await WriteAttributeAsync(xmlWriter, "type", selection.Type);

                if (selection.CustomNotes != null) await xmlWriter.WriteElementStringAsync(null, "customNotes", null, selection.CustomNotes);

                await WriteRulesAsync(xmlWriter, selection);
                await WriteProfilesAsync(xmlWriter, selection);
                await WriteSelectionsAsync(xmlWriter, selection);
                await WriteCostsAsync(xmlWriter, selection.Costs);
                await WriteCategoriesAsync(xmlWriter, selection);

                await xmlWriter.WriteEndElementAsync();
            }
            await xmlWriter.WriteEndElementAsync();
        }
    }
}