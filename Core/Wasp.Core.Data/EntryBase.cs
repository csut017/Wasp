namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines the base entry type. All other entries must derive from this class.
    /// </summary>
    public abstract class EntryBase
        : IEquatable<EntryBase>
    {
        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        public virtual string? DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the id of the configuration entry.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public virtual string? Name { get; set; }

        /// <summary>
        /// Compares if two <see cref="EntryBase"/> instances are the same.
        /// </summary>
        /// <param name="other">The other <see cref="EntryBase"/> instance.</param>
        /// <returns>True if they are same (their IDs match); false otherwise.</returns>
        public bool Equals(EntryBase? other)
        {
            if ((other?.Id == null) || (this.Id == null)) return false;
            return this.Id == other.Id;
        }

        /// <summary>
        /// Compares if this is the same as another object.
        /// </summary>
        /// <param name="other">The other <see cref="EntryBase"/> instance.</param>
        /// <returns>True if they are same (their IDs match); false otherwise.</returns>
        public override bool Equals(object? other)
        {
            return Equals(other as EntryBase);
        }

        /// <summary>
        /// Generates a hashcode for this instance.
        /// </summary>
        /// <returns>The hashcode.</returns>
        public override int GetHashCode()
        {
            return this.Id?.GetHashCode() ?? 0;
        }
    }
}