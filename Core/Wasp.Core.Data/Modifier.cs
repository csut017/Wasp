namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines a modifier.
    /// </summary>
    public class Modifier : IConditionGroupsParent
    {
        /// <summary>
        /// Gets or sets the condition groups.
        /// </summary>
        public List<ConditionGroup>? ConditionGroups { get; set; }

        /// <summary>
        /// Gets or sets the conditions.
        /// </summary>
        public List<Constraint>? Conditions { get; set; }

        /// <summary>
        /// Gets or sets the field being modified.
        /// </summary>
        public string? Field { get; set; }

        /// <summary>
        /// Gets or sets the repeats.
        /// </summary>
        public List<Constraint>? Repeats { get; set; }

        /// <summary>
        /// Gets or sets the type of the modifier.
        /// </summary>
        public string? Type { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public string? Value { get; set; }
    }
}