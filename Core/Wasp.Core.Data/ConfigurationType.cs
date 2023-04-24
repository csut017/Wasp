namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines the type of configuration data.
    /// </summary>
    public enum ConfigurationType
    {
        /// <summary>
        /// The configuration type is unknown.
        /// </summary>
        Unknown,

        /// <summary>
        /// The configuration is for a catalogue.
        /// </summary>
        Catalogue,

        /// <summary>
        /// The configuration is for a game system.
        /// </summary>
        GameSystem,
    }
}