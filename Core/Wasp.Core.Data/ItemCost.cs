using System.Xml.Serialization;

namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines a cost of an item.
    /// </summary>
    public class ItemCost
    {
        /// <summary>
        /// Gets or sets the name of the cost.
        /// </summary>
        [XmlAttribute("name", Namespace = BattleScribeConstants.XmlNamespace)]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the id of the cost type.
        /// </summary>
        [XmlAttribute("typeId", Namespace = BattleScribeConstants.XmlNamespace)]
        public string? TypeId { get; set; }

        /// <summary>
        /// Gets or sets the cost value.
        /// </summary>
        [XmlAttribute("value", Namespace = BattleScribeConstants.XmlNamespace)]
        public double Value { get; set; }
    }
}