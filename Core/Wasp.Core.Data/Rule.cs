namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines a rule.
    /// </summary>
    public class Rule
    {
        /// <summary>
        /// Gets or sets the description of the rule.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the id of the rule.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether this rule is hidden or not.
        /// </summary>
        public bool IsHidden { get; set; }

        /// <summary>
        /// Gets or sets the modifiers.
        /// </summary>
        public List<Modifier>? Modifiers { get; set; }

        /// <summary>
        /// Gets or sets the name of the rule.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the page number for finding this rule.
        /// </summary>
        public string? Page { get; set; }

        /// <summary>
        /// Gets or sets the id of the publication this rule is from.
        /// </summary>
        public string? PublicationId { get; set; }
    }
}