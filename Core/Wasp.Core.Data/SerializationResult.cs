namespace Wasp.Core.Data
{
    /// <summary>
    /// The result of deserialising a XML definition.
    /// </summary>
    public partial class SerializationResult<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Gets or sets the deserialized entity.
        /// </summary>
        public TEntity? Entity { get; set; }

        /// <summary>
        /// Gets a list of nodes that were not deserilized.
        /// </summary>
        public IList<UnknownNode> UnknownNodes { get; } = new List<UnknownNode>();
    }
}