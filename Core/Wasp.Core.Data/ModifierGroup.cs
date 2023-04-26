namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines a modifier group.
    /// </summary>
    public class ModifierGroup
    {
        /// <summary>
        /// Gets or sets an optional comment.
        /// </summary>
        public string? Comment { get; set; }

        /// <summary>
        /// Gets or sets the condition groups.
        /// </summary>
        public List<ConditionGroup>? ConditionGroups { get; set; }

        /// <summary>
        /// Gets or sets the conditions.
        /// </summary>
        public List<Constraint>? Conditions { get; set; }

        /// <summary>
        /// Gets or sets the modifier groups.
        /// </summary>
        public List<ModifierGroup>? ModifierGroups { get; set; }

        /// <summary>
        /// Gets or sets the modifiers.
        /// </summary>
        public List<Modifier>? Modifiers { get; set; }

        /// <summary>
        /// Gets or sets the repeats.
        /// </summary>
        public List<Constraint>? Repeats { get; set; }
    }
}