using System.Xml.Serialization;

namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines a characteristic.
    /// </summary>
    [XmlRoot("characteristic", Namespace = BattleScribeConstants.XmlNamespace)]
    public class Characteristic
        : Serializable<Characteristic>
    {
        /// <summary>
        /// Gets or sets the name of the characteristic.
        /// </summary>
        [XmlAttribute("name", Namespace = BattleScribeConstants.XmlNamespace)]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the type of the characteristic.
        /// </summary>
        [XmlAttribute("typeId", Namespace = BattleScribeConstants.XmlNamespace)]
        public string? TypeId { get; set; }

        /// <summary>
        /// Gets or sets the value of the characteristic.
        /// </summary>
        [XmlText()]
        public string? Value { get; set; }
    }
}