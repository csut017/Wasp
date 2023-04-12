using System.Xml.Serialization;

namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines a profile.
    /// </summary>
    [XmlRoot("profile", Namespace = BattleScribeConstants.XmlNamespace)]
    public class Profile
        : Serializable<Profile>
    {
        /// <summary>
        /// Gets the characteristics for this profile.
        /// </summary>
        [XmlArray("characteristics", Namespace = BattleScribeConstants.XmlNamespace)]
        [XmlArrayItem("characteristic", Namespace = BattleScribeConstants.XmlNamespace)]
        public List<Characteristic> Characteristics { get; } = new List<Characteristic>();

        /// <summary>
        /// Gets or sets the id of the profile.
        /// </summary>
        [XmlAttribute("id", Namespace = BattleScribeConstants.XmlNamespace)]
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether this rule is hidden or not.
        /// </summary>
        [XmlAttribute("hidden", Namespace = BattleScribeConstants.XmlNamespace)]
        public bool IsHidden { get; set; }

        /// <summary>
        /// Gets or sets the name of the profile.
        /// </summary>
        [XmlAttribute("name", Namespace = BattleScribeConstants.XmlNamespace)]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the page number for finding this rule.
        /// </summary>
        [XmlAttribute("page", Namespace = BattleScribeConstants.XmlNamespace)]
        public int Page { get; set; }

        /// <summary>
        /// Gets or sets the id of the publication this profile is from.
        /// </summary>
        [XmlAttribute("publicationId", Namespace = BattleScribeConstants.XmlNamespace)]
        public string? PublicationId { get; set; }

        /// <summary>
        /// Gets or sets the id of the profile type.
        /// </summary>
        [XmlAttribute("typeId", Namespace = BattleScribeConstants.XmlNamespace)]
        public string? TypeId { get; set; }

        /// <summary>
        /// Gets or sets the name of the profile type.
        /// </summary>
        [XmlAttribute("typeName", Namespace = BattleScribeConstants.XmlNamespace)]
        public string? TypeName { get; set; }
    }
}