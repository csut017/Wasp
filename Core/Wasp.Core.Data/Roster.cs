using System.Xml.Serialization;

namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines a roster (army list).
    /// </summary>
    [XmlRoot("roster", Namespace = BattleScribeConstants.XmlNamespace)]
    public class Roster
        : Serializable<Roster>
    {
        /// <summary>
        /// Gets or sets the version of BattleScribe used to generate this roster.
        /// </summary>
        [XmlAttribute("battleScribeVersion", Namespace = BattleScribeConstants.XmlNamespace)]
        public string? BattleScribeVersion { get; set; }

        /// <summary>
        /// Gets the costs of the roster.
        /// </summary>
        [XmlArray("costs", Namespace = BattleScribeConstants.XmlNamespace)]
        [XmlArrayItem("cost", Namespace = BattleScribeConstants.XmlNamespace)]
        public List<ItemCost> Costs { get; } = new List<ItemCost>();

        /// <summary>
        /// Gets the forces in the roster.
        /// </summary>
        [XmlArray("forces", Namespace = BattleScribeConstants.XmlNamespace)]
        [XmlArrayItem("force", Namespace = BattleScribeConstants.XmlNamespace)]
        public List<Force> Forces { get; } = new List<Force>();

        /// <summary>
        /// Gets or sets the id of the game system this roster is for.
        /// </summary>
        [XmlAttribute("gameSystemId", Namespace = BattleScribeConstants.XmlNamespace)]
        public string? GameSystemId { get; set; }

        /// <summary>
        /// Gets or sets the name of the game system this roster is for.
        /// </summary>
        [XmlAttribute("gameSystemName", Namespace = BattleScribeConstants.XmlNamespace)]
        public string? GameSystemName { get; set; }

        /// <summary>
        /// Gets or sets the revision of the game system this roster is for.
        /// </summary>
        [XmlAttribute("gameSystemRevision", Namespace = BattleScribeConstants.XmlNamespace)]
        public string? GameSystemRevision { get; set; }

        /// <summary>
        /// Gets or sets the id of the roster.
        /// </summary>
        [XmlAttribute("id", Namespace = BattleScribeConstants.XmlNamespace)]
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the roster.
        /// </summary>
        [XmlAttribute("name", Namespace = BattleScribeConstants.XmlNamespace)]
        public string? Name { get; set; }
    }
}