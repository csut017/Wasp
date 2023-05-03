namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines an entry that can be configured.
    /// </summary>
    public abstract class ConfigurableEntryBase
        : PublicationEntryBase, IConfigurableEntry
    {
        /// <summary>
        /// Gets or sets the constraints.
        /// </summary>
        public List<Constraint>? Constraints { get; set; }

        /// <summary>
        /// Gets or sets the information links.
        /// </summary>
        public List<InformationLink>? InformationLinks { get; set; }

        /// <summary>
        /// Gets or sets the modifier groups.
        /// </summary>
        public List<ModifierGroup>? ModifierGroups { get; set; }

        /// <summary>
        /// Gets or sets the modifiers.
        /// </summary>
        public List<Modifier>? Modifiers { get; set; }
    }
}