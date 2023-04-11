using System.Xml.Serialization;

namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines a selection.
    /// </summary>
    [XmlRoot("selection", Namespace = BattleScribeConstants.XmlNamespace)]
    public class Selection
        : Serializable<Selection>
    {
        /// <summary>
        /// Gets the categories in the force.
        /// </summary>
        [XmlArray("categories", Namespace = BattleScribeConstants.XmlNamespace)]
        [XmlArrayItem("category", Namespace = BattleScribeConstants.XmlNamespace)]
        public List<Category> Categories { get; } = new List<Category>();

        /// <summary>
        /// Gets the costs of the roster.
        /// </summary>
        [XmlArray("costs", Namespace = BattleScribeConstants.XmlNamespace)]
        [XmlArrayItem("cost", Namespace = BattleScribeConstants.XmlNamespace)]
        public List<ItemCost> Costs { get; } = new List<ItemCost>();

        /// <summary>
        /// Gets or sets the entry id.
        /// </summary>
        [XmlAttribute("entryId", Namespace = BattleScribeConstants.XmlNamespace)]
        public string? EntryId { get; set; }

        /// <summary>
        /// Gets or sets the id of the selection.
        /// </summary>
        [XmlAttribute("id", Namespace = BattleScribeConstants.XmlNamespace)]
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the selection.
        /// </summary>
        [XmlAttribute("name", Namespace = BattleScribeConstants.XmlNamespace)]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the number for the selection.
        /// </summary>
        [XmlAttribute("number", Namespace = BattleScribeConstants.XmlNamespace)]
        public int Number { get; set; }

        /// <summary>
        /// Gets or sets the type of the selection.
        /// </summary>
        [XmlAttribute("type", Namespace = BattleScribeConstants.XmlNamespace)]
        public string? Type { get; set; }
    }
}