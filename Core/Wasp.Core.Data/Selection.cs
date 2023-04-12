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
        [XmlArray("categories", Namespace = BattleScribeConstants.XmlNamespace, Order = 50)]
        [XmlArrayItem("category", Namespace = BattleScribeConstants.XmlNamespace)]
        public List<Category> Categories { get; } = new List<Category>();

        /// <summary>
        /// Gets whether Categories should be serialized.
        /// </summary>
        public bool CategoriesSpecified { get => this.Categories != null && this.Categories.Any(); }

        /// <summary>
        /// Gets the costs of the roster.
        /// </summary>
        [XmlArray("costs", Namespace = BattleScribeConstants.XmlNamespace, Order = 30)]
        [XmlArrayItem("cost", Namespace = BattleScribeConstants.XmlNamespace)]
        public List<ItemCost> Costs { get; } = new List<ItemCost>();

        /// <summary>
        /// Gets whether Costs should be serialized.
        /// </summary>
        public bool CostsSpecified { get => this.Costs != null && this.Costs.Any(); }

        /// <summary>
        /// Gets or sets the custom name of the selection.
        /// </summary>
        [XmlAttribute("customName", Namespace = BattleScribeConstants.XmlNamespace)]
        public string? CustomName { get; set; }

        /// <summary>
        /// Gets or sets the entry group id.
        /// </summary>
        [XmlAttribute("entryGroupId", Namespace = BattleScribeConstants.XmlNamespace)]
        public string? EntryGroupId { get; set; }

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
        /// Gets or sets the page number of the selection.
        /// </summary>
        [XmlAttribute("page", Namespace = BattleScribeConstants.XmlNamespace)]
        public string? Page { get; set; }

        /// <summary>
        /// Gets or sets the child profiles.
        /// </summary>
        [XmlArray("profiles", Namespace = BattleScribeConstants.XmlNamespace, Order = 10)]
        [XmlArrayItem("profile", Namespace = BattleScribeConstants.XmlNamespace)]
        public List<Profile>? Profiles { get; set; }

        /// <summary>
        /// Gets whether Profiles should be serialized.
        /// </summary>
        public bool ProfilesSpecified { get => this.Profiles != null && this.Profiles.Any(); }

        /// <summary>
        /// Gets or sets the child rules.
        /// </summary>
        [XmlArray("rules", Namespace = BattleScribeConstants.XmlNamespace, Order = 90)]
        [XmlArrayItem("rule", Namespace = BattleScribeConstants.XmlNamespace)]
        public List<Rule>? Rules { get; set; }

        /// <summary>
        /// Gets whether Rules should be serialized.
        /// </summary>
        public bool RulesSpecified { get => this.Rules != null && this.Rules.Any(); }

        /// <summary>
        /// Gets or sets the child selections.
        /// </summary>
        [XmlArray("selections", Namespace = BattleScribeConstants.XmlNamespace, Order = 20)]
        [XmlArrayItem("selection", Namespace = BattleScribeConstants.XmlNamespace)]
        public List<Selection>? Selections { get; set; }

        /// <summary>
        /// Gets whether Selections should be serialized.
        /// </summary>
        public bool SelectionsSpecified { get => this.Selections != null && this.Selections.Any(); }

        /// <summary>
        /// Gets or sets the type of the selection.
        /// </summary>
        [XmlAttribute("type", Namespace = BattleScribeConstants.XmlNamespace)]
        public string? Type { get; set; }
    }
}