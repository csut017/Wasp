namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines an element that has condition groups.
    /// </summary>
    public interface IConditionGroupsParent
    {
        /// <summary>
        /// Gets or sets the condition groups.
        /// </summary>
        List<ConditionGroup>? ConditionGroups { get; set; }
    }
}