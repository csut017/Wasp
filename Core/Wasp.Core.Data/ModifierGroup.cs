namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines a modifier group.
    /// </summary>
    public class ModifierGroup
    {
        /// <summary>
        /// Gets or sets the conditions.
        /// </summary>
        public List<Constraint>? Conditions { get; set; }

        /// <summary>
        /// Gets or sets the modifiers.
        /// </summary>
        public List<Modifier>? Modifiers { get; set; }
    }
}