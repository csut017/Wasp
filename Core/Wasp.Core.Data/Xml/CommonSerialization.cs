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
        /// Writes the characteristics for an item.
        /// </summary>
        /// <param name="xmlWriter">The <see cref="XmlWriter"/> to use.</param>
        /// <param name="parent">The element containing the characteristics.</param>
        public static async Task WriteCharacteristicsAsync(XmlWriter xmlWriter, List<Characteristic>? parent)
        {
            if (parent == null) return;

            await xmlWriter.WriteStartElementAsync(null, "characteristics", null);
            foreach (var characteristic in parent)
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
            await xmlWriter.FlushAsync();
        }

        /// <summary>
        /// Writes the comment for an entry.
        /// </summary>
        /// <param name="xmlWriter">The <see cref="XmlWriter"/> to use.</param>
        /// <param name="configuration">The element containing the comment.</param>
        public static async Task WriteCommentAsync(XmlWriter xmlWriter, ConfigurationEntry configuration)
        {
            if (configuration.Comment == null) return;
            await xmlWriter.WriteElementStringAsync(null, "comment", null, configuration.Comment);
        }

        /// <summary>
        /// Writes the condition groups.
        /// </summary>
        /// <param name="xmlWriter">The <see cref="XmlWriter"/> to use.</param>
        /// <param name="parent">The parent item containing the data to write.</param>
        public static async Task WriteConditionGroupsAsync(XmlWriter xmlWriter, List<ConditionGroup>? parent)
        {
            if (parent == null) return;

            await xmlWriter.WriteStartElementAsync(null, "conditionGroups", null);
            foreach (var item in parent)
            {
                await xmlWriter.WriteStartElementAsync(null, "conditionGroup", null);
                await WriteAttributeAsync(xmlWriter, "type", item.Type);
                await WriteCommentAsync(xmlWriter, item);
                await WriteConstraintsAsync(xmlWriter, item.Conditions, "conditions", "condition");
                await WriteConditionGroupsAsync(xmlWriter, item.ConditionGroups);
                await xmlWriter.WriteEndElementAsync();
            }
            await xmlWriter.WriteEndElementAsync();
            await xmlWriter.FlushAsync();
        }

        /// <summary>
        /// Writes the conditions.
        /// </summary>
        /// <param name="xmlWriter">The <see cref="XmlWriter"/> to use.</param>
        /// <param name="parent">The parent item containing the data to write.</param>
        /// <param name="arrayName">The name of the list.</param>
        /// <param name="itemName">The name of each item.</param>
        public static async Task WriteConstraintsAsync(XmlWriter xmlWriter, List<Constraint>? parent, string arrayName, string itemName)
        {
            if (parent == null) return;

            await xmlWriter.WriteStartElementAsync(null, arrayName, null);
            foreach (var item in parent)
            {
                await xmlWriter.WriteStartElementAsync(null, itemName, null);
                await WriteAttributeAsync(xmlWriter, "field", item.Field);
                await WriteAttributeAsync(xmlWriter, "scope", item.Scope);
                await WriteAttributeAsync(xmlWriter, "value", item.Value);
                await WriteAttributeAsync(xmlWriter, "percentValue", item.IsPercentage);
                await WriteAttributeAsync(xmlWriter, "shared", item.IsShared);
                await WriteAttributeAsync(xmlWriter, "includeChildSelections", item.IncludeChildSelections);
                await WriteAttributeAsync(xmlWriter, "includeChildForces", item.IncludeChildForces);
                await WriteAttributeAsync(xmlWriter, "childId", item.ChildId);
                await WriteAttributeAsync(xmlWriter, "repeats", item.NumberOfRepeats);
                await WriteAttributeAsync(xmlWriter, "roundUp", item.ShouldRoundUp);
                await WriteAttributeAsync(xmlWriter, "id", item.Id);
                await WriteAttributeAsync(xmlWriter, "type", item.Type);
                await WriteCommentAsync(xmlWriter, item);
                await xmlWriter.WriteEndElementAsync();
            }
            await xmlWriter.WriteEndElementAsync();
            await xmlWriter.FlushAsync();
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
                await WriteAttributeAsync(xmlWriter, "name", cost.Name);
                await WriteAttributeAsync(xmlWriter, "typeId", cost.TypeId);
                await WriteAttributeAsync(xmlWriter, "value", cost.Value);
                await xmlWriter.WriteEndElementAsync();
            }
            await xmlWriter.WriteEndElementAsync();
            await xmlWriter.FlushAsync();
        }

        /// <summary>
        /// Writes the modifiers.
        /// </summary>
        /// <param name="xmlWriter">The <see cref="XmlWriter"/> to use.</param>
        /// <param name="parent">The parent item containing the data to write.</param>
        public static async Task WriteModifiersAsync(XmlWriter xmlWriter, List<Modifier>? parent)
        {
            if (parent == null) return;

            await xmlWriter.WriteStartElementAsync(null, "modifiers", null);
            foreach (var item in parent)
            {
                await xmlWriter.WriteStartElementAsync(null, "modifier", null);
                await WriteAttributeAsync(xmlWriter, "type", item.Type);
                await WriteAttributeAsync(xmlWriter, "field", item.Field);
                await WriteAttributeAsync(xmlWriter, "value", item.Value);
                await WriteCommentAsync(xmlWriter, item);
                await WriteConditionGroupsAsync(xmlWriter, item.ConditionGroups);
                await WriteConstraintsAsync(xmlWriter, item.Repeats, "repeats", "repeat");
                await WriteConstraintsAsync(xmlWriter, item.Conditions, "conditions", "condition");
                await xmlWriter.WriteEndElementAsync();
            }
            await xmlWriter.WriteEndElementAsync();
            await xmlWriter.FlushAsync();
        }

        /// <summary>
        /// Writes the profiles for an item.
        /// </summary>
        /// <param name="xmlWriter">The <see cref="XmlWriter"/> to use.</param>
        /// <param name="parent">The element containing the profiles.</param>
        /// <param name="arrayName">The name of the list.</param>
        /// <param name="itemName">The name of each item.</param>
        public static async Task WriteProfilesAsync(XmlWriter xmlWriter, List<Profile>? parent, string arrayName, string itemName)
        {
            if (parent == null) return;

            await xmlWriter.WriteStartElementAsync(null, arrayName, null);
            foreach (var profile in parent)
            {
                await xmlWriter.WriteStartElementAsync(null, itemName, null);
                await WriteAttributeAsync(xmlWriter, "id", profile.Id);
                await WriteAttributeAsync(xmlWriter, "name", profile.Name);
                await WriteAttributeAsync(xmlWriter, "publicationId", profile.PublicationId);
                await WriteAttributeAsync(xmlWriter, "page", profile.Page);
                await WriteAttributeAsync(xmlWriter, "hidden", profile.IsHidden);
                await WriteAttributeAsync(xmlWriter, "typeId", profile.TypeId);
                await WriteAttributeAsync(xmlWriter, "typeName", profile.TypeName);

                await WriteModifiersAsync(xmlWriter, profile.Modifiers);
                await WriteCharacteristicsAsync(xmlWriter, profile.Characteristics);

                await xmlWriter.WriteEndElementAsync();
            }
            await xmlWriter.WriteEndElementAsync();
            await xmlWriter.FlushAsync();
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
                await WriteAttributeAsync(xmlWriter, "publicationDate", publication.PublicationDate);
                await WriteAttributeAsync(xmlWriter, "publisherUrl", publication.PublisherUrl);
                await WriteCommentAsync(xmlWriter, publication);
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
        /// <param name="arrayName">The name of the list.</param>
        /// <param name="itemName">The name of each item.</param>
        public static async Task WriteRulesAsync(XmlWriter xmlWriter, List<Rule>? parent, string arrayName, string itemName)
        {
            if (parent == null) return;

            await xmlWriter.WriteStartElementAsync(null, arrayName, null);
            foreach (var rule in parent)
            {
                await xmlWriter.WriteStartElementAsync(null, itemName, null);
                await WriteAttributeAsync(xmlWriter, "id", rule.Id);
                await WriteAttributeAsync(xmlWriter, "name", rule.Name);
                await WriteAttributeAsync(xmlWriter, "publicationId", rule.PublicationId);
                await WriteAttributeAsync(xmlWriter, "page", rule.Page);
                await WriteAttributeAsync(xmlWriter, "hidden", rule.IsHidden);
                await WriteModifiersAsync(xmlWriter, rule.Modifiers);
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