using System.Xml.Serialization;

namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines a publication.
    /// </summary>
    public class Publication
    {
        /// <summary>
        /// Gets or sets the id of the publication.
        /// </summary>
        [XmlAttribute("id", Namespace = BattleScribeConstants.XmlNamespace)]
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the publication.
        /// </summary>
        [XmlAttribute("name", Namespace = BattleScribeConstants.XmlNamespace)]
        public string? Name { get; set; }
    }
}