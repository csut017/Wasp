namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines an item as having rules.
    /// </summary>
    public interface IRulesParent
    {
        /// <summary>
        /// Gets the associated rules.
        /// </summary>
        List<Rule>? Rules { get; set; }
    }
}