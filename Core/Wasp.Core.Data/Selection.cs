namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines a selection.
    /// </summary>
    public class Selection
        : EntryBase
    {
        /// <summary>
        /// Gets the categories in the selection.
        /// </summary>
        public List<Category>? Categories { get; set; }

        /// <summary>
        /// Gets the costs of the roster.
        /// </summary>
        public List<ItemCost>? Costs { get; set; }

        /// <summary>
        /// Gets or sets the custom name of the selection.
        /// </summary>
        public string? CustomName { get; set; }

        /// <summary>
        /// Gets or sets the custom notes on the selection.
        /// </summary>
        public string? CustomNotes { get; set; }

        /// <summary>
        /// Gets or sets the entry group id.
        /// </summary>
        public string? EntryGroupId { get; set; }

        /// <summary>
        /// Gets or sets the entry id.
        /// </summary>
        public string? EntryId { get; set; }

        /// <summary>
        /// Gets or sets the number for the selection.
        /// </summary>
        public string? Number { get; set; }

        /// <summary>
        /// Gets or sets the page number of the selection.
        /// </summary>
        public string? Page { get; set; }

        /// <summary>
        /// Gets or sets the child profiles.
        /// </summary>
        public List<Profile>? Profiles { get; set; }

        /// <summary>
        /// Gets or sets the publication ID the selection is referencing.
        /// </summary>
        public string? PublicationId { get; set; }

        /// <summary>
        /// Gets or sets the child rules.
        /// </summary>
        public List<Rule>? Rules { get; set; }

        /// <summary>
        /// Gets or sets the child selections.
        /// </summary>
        public List<Selection>? Selections { get; set; }

        /// <summary>
        /// Gets or sets the type of the selection.
        /// </summary>
        public string? Type { get; set; }
    }
}