using System.Xml;

namespace Wasp.Core.Data.Xml
{
    internal static class CommonSerialization
    {
        /// <summary>
        /// Conditionally writes an attribute.
        /// </summary>
        /// <param name="xmlWriter">The <see cref="XmlWriter"/> to use.</param>
        /// <param name="name">The name of the attribute.</param>
        /// <param name="value">The value of the attribute.</param>
        public static async Task WriteAttributeAsync(XmlWriter xmlWriter, string name, bool? value)
        {
            if (value == null) return;

            await xmlWriter.WriteAttributeStringAsync(null, name, null, value.Value ? "true" : "false");
        }

        /// <summary>
        /// Conditionally writes an attribute.
        /// </summary>
        /// <param name="xmlWriter">The <see cref="XmlWriter"/> to use.</param>
        /// <param name="name">The name of the attribute.</param>
        /// <param name="value">The value of the attribute.</param>
        public static async Task WriteAttributeAsync(XmlWriter xmlWriter, string name, string? value)
        {
            if (value == null) return;

            await xmlWriter.WriteAttributeStringAsync(null, name, null, value);
        }

        /// <summary>
        /// Writes the comment for an entry.
        /// </summary>
        /// <param name="xmlWriter">The <see cref="XmlWriter"/> to use.</param>
        /// <param name="configuration">The element containing the comment.</param>
        public static async Task WriteComment(XmlWriter xmlWriter, ConfigurationEntry configuration)
        {
            if (configuration.Comment == null) return;
            await xmlWriter.WriteElementStringAsync(null, "comment", null, configuration.Comment);
        }

        /// <summary>
        /// Writes the costs for an item.
        /// </summary>
        /// <param name="xmlWriter">The <see cref="XmlWriter"/> to use.</param>
        /// <param name="costs">The element containing the costs.</param>
        /// <param name="itemName">The name of the item.</param>
        public static async Task WriteCostsAsync(XmlWriter xmlWriter, List<ItemCost>? costs, string itemName = "cost")
        {
            if (costs == null) return;

            await xmlWriter.WriteStartElementAsync(null, itemName + "s", null);
            foreach (var cost in costs)
            {
                await xmlWriter.WriteStartElementAsync(null, itemName, null);
                await CommonSerialization.WriteAttributeAsync(xmlWriter, "name", cost.Name);
                await CommonSerialization.WriteAttributeAsync(xmlWriter, "typeId", cost.TypeId);
                await CommonSerialization.WriteAttributeAsync(xmlWriter, "value", cost.Value);
                await xmlWriter.WriteEndElementAsync();
            }
            await xmlWriter.WriteEndElementAsync();
        }

        /// <summary>
        /// Writes the publications for an item.
        /// </summary>
        /// <param name="xmlWriter">The <see cref="XmlWriter"/> to use.</param>
        /// <param name="parent">The element containing the publications.</param>
        public static async Task WritePublicationsAsync(XmlWriter xmlWriter, List<Publication>? parent)
        {
            if (parent == null) return;

            await xmlWriter.WriteStartElementAsync(null, "publications", null);
            foreach (var publication in parent)
            {
                await xmlWriter.WriteStartElementAsync(null, "publication", null);
                await WriteAttributeAsync(xmlWriter, "id", publication.Id);
                await WriteAttributeAsync(xmlWriter, "name", publication.FullName);
                await WriteAttributeAsync(xmlWriter, "shortName", publication.ShortName);
                await WriteAttributeAsync(xmlWriter, "publisher", publication.PublisherName);
                await WriteComment(xmlWriter, publication);
                await xmlWriter.WriteEndElementAsync();
            }
            await xmlWriter.WriteEndElementAsync();
            await xmlWriter.FlushAsync();
        }

        /// <summary>
        /// Writes the rules for an item.
        /// </summary>
        /// <param name="xmlWriter">The <see cref="XmlWriter"/> to use.</param>
        /// <param name="parent">The element containing the rules.</param>
        public static async Task WriteRulesAsync(XmlWriter xmlWriter, List<Rule>? parent)
        {
            if (parent == null) return;

            await xmlWriter.WriteStartElementAsync(null, "rules", null);
            foreach (var rule in parent)
            {
                await xmlWriter.WriteStartElementAsync(null, "rule", null);
                await CommonSerialization.WriteAttributeAsync(xmlWriter, "id", rule.Id);
                await CommonSerialization.WriteAttributeAsync(xmlWriter, "name", rule.Name);
                await CommonSerialization.WriteAttributeAsync(xmlWriter, "publicationId", rule.PublicationId);
                await CommonSerialization.WriteAttributeAsync(xmlWriter, "page", rule.Page);
                await CommonSerialization.WriteAttributeAsync(xmlWriter, "hidden", rule.IsHidden);
                if (rule.Description != null)
                {
                    await xmlWriter.WriteElementStringAsync(null, "description", null, rule.Description);
                }

                await xmlWriter.WriteEndElementAsync();
            }
            await xmlWriter.WriteEndElementAsync();
        }
    }
}