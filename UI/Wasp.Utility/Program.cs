using Wasp.Core.Data;

namespace Wasp.Utility
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var input = "c:\\temp\\data.ros";
            var output = "c:\\temp\\data-converted.rosz";

            var package = await Package.LoadAsync(input);
            Console.WriteLine("Loaded roster");

            package.Settings.IsCompressed = true;
            await package.SaveAsync(output);
            Console.WriteLine("Saved roster");
        }
    }
}