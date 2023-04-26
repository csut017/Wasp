namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines an entry with a comment.
    /// </summary>
    public abstract class CommentedEntry
    {
        /// <summary>
        /// Gets or sets an optional comment on this configuration entry.
        /// </summary>
        public string? Comment { get; set; }
    }
}