namespace Wasp.Core.Data.Xml
{
    internal class ConfigurationTypeDetails
    {
        public ConfigurationTypeDetails(string root, string schema)
        {
            Root = root;
            Schema = schema;
        }

        public string Root { get; private set; }

        public string Schema { get; private set; }
    }
}