namespace Wasp.Core.Data
{
    /// <summary>
    /// Marks an item as configurable.
    /// </summary>
    public interface IConfigurableEntry
    {
        /// <summary>
        /// Gets or sets the constraints.
        /// </summary>
        List<Constraint>? Constraints { get; set; }

        /// <summary>
        /// Gets or sets the information links.
        /// </summary>
        List<InformationLink>? InformationLinks { get; set; }

        /// <summary>
        /// Gets or sets the modifier groups.
        /// </summary>
        List<ModifierGroup>? ModifierGroups { get; set; }

        /// <summary>
        /// Gets or sets the modifiers.
        /// </summary>
        List<Modifier>? Modifiers { get; set; }
    }
}