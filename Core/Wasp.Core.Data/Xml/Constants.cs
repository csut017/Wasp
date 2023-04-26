namespace Wasp.Core.Data.Xml
{
    /// <summary>
    /// Defines constants to use with BattleScribe.
    /// </summary>
    internal class Constants
    {
        /// <summary>
        /// The XML namespace to use for serializing and deserializing the BattleScribe catalogue format.
        /// </summary>
        public const string CatalogueXmlNamespace = "http://www.battlescribe.net/schema/catalogueSchema";

        /// <summary>
        /// The XML namespace to use for serializing and deserializing the BattleScribe game system format.
        /// </summary>
        public const string GameSystemXmlNamespace = "http://www.battlescribe.net/schema/gameSystemSchema";

        /// <summary>
        /// The XML namespace to use for serializing and deserializing the BattleScribe roster format.
        /// </summary>
        public const string RosterXmlNamespace = "http://www.battlescribe.net/schema/rosterSchema";

        public static readonly Dictionary<ConfigurationType, ConfigurationTypeDetails> ConfigurationTypeRoots = new()
        {
            {ConfigurationType.Catalogue, new ("catalogue", CatalogueXmlNamespace) },
            {ConfigurationType.GameSystem, new("gameSystem", GameSystemXmlNamespace) },
        };
    }
}