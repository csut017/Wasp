namespace Wasp.Core.Data
{
    /// <summary>
    /// A node that the serializer could not handle.
    /// </summary>
    public record UnknownNode
    {
        /// <summary>
        /// Initialises a new instance of <see cref="UnknownNode"/>
        /// </summary>
        /// <param name="name">The name of the node.</param>
        /// <param name="lineNumber">The line number.</param>
        /// <param name="linePosition">The line position.</param>
        public UnknownNode(string name, int lineNumber, int linePosition)
        {
            Name = name;
            LineNumber = lineNumber;
            LinePosition = linePosition;
        }

        /// <summary>
        /// Gets the line number where the node occurred.
        /// </summary>
        public int LineNumber { get; }

        /// <summary>
        /// Gets the line position where the node occurred.
        /// </summary>
        public int LinePosition { get; }

        /// <summary>
        /// Gets the name of the node.
        /// </summary>
        public string Name { get; }
    }
}