namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines the amount of configuration to process.
    /// </summary>
    public enum ConfigurationLevel
    {
        /// <summary>
        /// Process the entire configuration definition.
        /// </summary>
        All = 0,

        /// <summary>
        /// Process only the root level (e.g. game system or catalogue) definition.
        /// </summary>
        Root = 1
    }
}