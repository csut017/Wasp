using System.Xml.Serialization;

namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines a force in a roster.
    /// </summary>
    [XmlRoot("force", Namespace = BattleScribeConstants.XmlNamespace)]
    public class Force
        : Serializable<Force>
    {
        /// <summary>
        /// Gets or sets the catalogue id.
        /// </summary>
        [XmlAttribute("catalogueId", Namespace = BattleScribeConstants.XmlNamespace)]
        public string? CatalogueId { get; set; }

        /// <summary>
        /// Gets or sets the catalogue name.
        /// </summary>
        [XmlAttribute("catalogueName", Namespace = BattleScribeConstants.XmlNamespace)]
        public string? CatalogueName { get; set; }

        /// <summary>
        /// Gets or sets the catalogue revision.
        /// </summary>
        [XmlAttribute("catalogueRevision", Namespace = BattleScribeConstants.XmlNamespace)]
        public string? CatalogueRevision { get; set; }

        /// <summary>
        /// Gets the categories in the force.
        /// </summary>
        [XmlArray("categories", Namespace = BattleScribeConstants.XmlNamespace, Order = 40)]
        [XmlArrayItem("category", Namespace = BattleScribeConstants.XmlNamespace)]
        public List<Category> Categories { get; } = new List<Category>();

        /// <summary>
        /// Gets or sets the entry id.
        /// </summary>
        [XmlAttribute("entryId", Namespace = BattleScribeConstants.XmlNamespace)]
        public string? EntryId { get; set; }

        /// <summary>
        /// Gets or sets the id of the force.
        /// </summary>
        [XmlAttribute("id", Namespace = BattleScribeConstants.XmlNamespace)]
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the force.
        /// </summary>
        [XmlAttribute("name", Namespace = BattleScribeConstants.XmlNamespace)]
        public string? Name { get; set; }

        /// <summary>
        /// Gets the publications references by this force.
        /// </summary>
        [XmlArray("publications", Namespace = BattleScribeConstants.XmlNamespace, Order = 30)]
        [XmlArrayItem("publication", Namespace = BattleScribeConstants.XmlNamespace)]
        public List<Publication> Publications { get; } = new List<Publication>();

        /// <summary>
        /// Gets the force level rules.
        /// </summary>
        [XmlArray("rules", Namespace = BattleScribeConstants.XmlNamespace, Order = 10)]
        [XmlArrayItem("rule", Namespace = BattleScribeConstants.XmlNamespace)]
        public List<Rule> Rules { get; } = new List<Rule>();

        /// <summary>
        /// Gets or sets the child selections.
        /// </summary>
        [XmlArray("selections", Namespace = BattleScribeConstants.XmlNamespace, Order = 20)]
        [XmlArrayItem("selection", Namespace = BattleScribeConstants.XmlNamespace)]
        public List<Selection>? Selections { get; set; }
    }
}