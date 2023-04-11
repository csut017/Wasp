using System.Xml.Serialization;

namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines a category.
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Gets or sets the entry id.
        /// </summary>
        [XmlAttribute("entryId", Namespace = BattleScribeConstants.XmlNamespace)]
        public string? EntryId { get; set; }

        /// <summary>
        /// Gets or sets the id of the category.
        /// </summary>
        [XmlAttribute("id", Namespace = BattleScribeConstants.XmlNamespace)]
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the category.
        /// </summary>
        [XmlAttribute("name", Namespace = BattleScribeConstants.XmlNamespace)]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether this is the primary category.
        /// </summary>
        [XmlAttribute("primary", Namespace = BattleScribeConstants.XmlNamespace)]
        public bool Primary { get; set; }
    }
}