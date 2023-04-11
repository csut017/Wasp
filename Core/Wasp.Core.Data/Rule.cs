using System.Xml.Serialization;

namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines a rule.
    /// </summary>
    [XmlRoot("rule", Namespace = BattleScribeConstants.XmlNamespace)]
    public class Rule
        : Serializable<Rule>
    {
        /// <summary>
        /// Gets or sets the description of the rule.
        /// </summary>
        [XmlElement("description", Namespace = BattleScribeConstants.XmlNamespace)]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the id of the rule.
        /// </summary>
        [XmlAttribute("id", Namespace = BattleScribeConstants.XmlNamespace)]
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether this rule is hidden or not.
        /// </summary>
        [XmlAttribute("hidden", Namespace = BattleScribeConstants.XmlNamespace)]
        public bool IsHidden { get; set; }

        /// <summary>
        /// Gets or sets the name of the rule.
        /// </summary>
        [XmlAttribute("name", Namespace = BattleScribeConstants.XmlNamespace)]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the page number for finding this rule.
        /// </summary>
        [XmlAttribute("page", Namespace = BattleScribeConstants.XmlNamespace)]
        public int Page { get; set; }

        /// <summary>
        /// Gets or sets the id of the publication this rule is from.
        /// </summary>
        [XmlAttribute("publicationId", Namespace = BattleScribeConstants.XmlNamespace)]
        public string? PublicationId { get; set; }
    }
}