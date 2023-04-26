namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines a condition group.
    /// </summary>
    public class ConditionGroup
        : CommentedEntry
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
        /// Gets or sets the type of the group.
        /// </summary>
        public string? Type { get; set; }
    }
}