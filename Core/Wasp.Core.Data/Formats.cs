namespace Wasp.Core.Data
{
    /// <summary>
    /// Exposes the allowed formats.
    /// </summary>
    public static class Formats
    {
        private static readonly IFormatProvider xml = new Xml.Format();

        /// <summary>
        /// Gets the XML format provider.
        /// </summary>
        public static IFormatProvider Xml
        {
            get => xml;
        }
    }
}