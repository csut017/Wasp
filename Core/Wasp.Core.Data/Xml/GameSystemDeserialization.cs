using System.Xml;

namespace Wasp.Core.Data.Xml
{
    internal class GameSystemDeserialization
    {
        /// <summary>
        /// Deserialize the root level element.
        /// </summary>
        /// <param name="reader">The <see cref="TextReader"/> containing the definition to deserialize.</param>
        /// <returns>A <see cref="GameSystem"/> containing the deserialized data.</returns>
        public static async Task<GameSystem> DeserializeRootAsync(TextReader reader)
        {
            var gameSystem = new GameSystem();
            var settings = new XmlReaderSettings { Async = true };
            using var xmlReader = XmlReader.Create(reader, settings);
            while (await xmlReader.ReadAsync())
            {
                await xmlReader.MoveToContentAsync();
                switch (xmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        // Should only have a gameSystem at the root level
                        if (xmlReader.Name != "gameSystem") throw new Exception($"Invalid game system definition: expected a gameSystem node, found {xmlReader.Name} instead");
                        await DeserializeGameSystemAsync(xmlReader, gameSystem);
                        break;

                    default:
                        // Anything else is an error
                        throw new Exception($"Unexpected node type: {xmlReader.NodeType} ({xmlReader.Name})");
                }
            }

            return gameSystem;
        }

        private static Task DeserializeGameSystemAsync(XmlReader xmlReader, GameSystem gameSystem)
        {
            throw new NotImplementedException();
        }
    }
}