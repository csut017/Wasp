namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines a condition group.
    /// </summary>
    public class ConditionGroup
    {
        /// <summary>
        /// Gets or sets an optional comment for the condition group.
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
        /// Gets or sets the type of the group.
        /// </summary>
        public string? Type { get; set; }
    }
}