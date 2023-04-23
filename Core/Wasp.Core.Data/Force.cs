namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines a force in a roster.
    /// </summary>
    public class Force
        : ISelectionParent, IRulesParent, ICategoriesParent
    {
        /// <summary>
        /// Gets or sets the catalogue id.
        /// </summary>
        public string? CatalogueId { get; set; }

        /// <summary>
        /// Gets or sets the catalogue name.
        /// </summary>
        public string? CatalogueName { get; set; }

        /// <summary>
        /// Gets or sets the catalogue revision.
        /// </summary>
        public string? CatalogueRevision { get; set; }

        /// <summary>
        /// Gets the categories in the force.
        /// </summary>
        public List<Category>? Categories { get; set; }

        /// <summary>
        /// Gets or sets the entry id.
        /// </summary>
        public string? EntryId { get; set; }

        /// <summary>
        /// Gets or sets the id of the force.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the force.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets the publications referenced by this force.
        /// </summary>
        public List<Publication>? Publications { get; set; }

        /// <summary>
        /// Gets the associated rules.
        /// </summary>
        public List<Rule>? Rules { get; set; }

        /// <summary>
        /// Gets or sets the child selections.
        /// </summary>
        public List<Selection>? Selections { get; set; }
    }
}