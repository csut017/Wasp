namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines the settings for a package.
    /// </summary>
    public class PackageSettings
    {
        /// <summary>
        /// Gets or sets the type of configuration that the package holds.
        /// </summary>
        public ConfigurationType ConfigurationType { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IFormatProvider"/> to use.
        /// </summary>
        public IFormatProvider Format { get; set; } = Formats.Xml;

        /// <summary>
        /// Gets or sets a flag indicating whether the package is compressed.
        /// </summary>
        public bool IsCompressed { get; set; }

        /// <summary>
        /// Gets or sets the name of the package.
        /// </summary>
        public string? Name { get; set; }
    }
}