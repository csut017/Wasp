namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines an entity that can contain modifiers.
    /// </summary>
    public interface IModifiersParent
    {
        /// <summary>
        /// Gets or sets the modifiers.
        /// </summary>
        List<Modifier>? Modifiers { get; set; }
    }
}