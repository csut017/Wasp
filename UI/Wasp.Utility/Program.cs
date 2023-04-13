using Wasp.Core.Data;

namespace Wasp.Utility
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var input = "c:\\temp\\data.ros";
            var output = "c:\\temp\\data-converted.ros";

            var formatProvider = Formats.Xml;
            using var inputStream = File.OpenRead(input);
            var roster = await formatProvider.DeserializeAsync(inputStream);
            Console.WriteLine("Loaded roster");

            if (roster != null)
            {
                using var outputStream = File.Create(output);
                await formatProvider.SerializeAsync(roster, outputStream);
            }
            Console.WriteLine("Saved roster");
        }
    }
}