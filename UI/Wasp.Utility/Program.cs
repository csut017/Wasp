using Wasp.Core.Data;

namespace Wasp.Utility
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            //var input = "C:\\Users\\csut017\\BattleScribe\\rosters\\Ay Eldi (T'au).rosz";
            //var output = "c:\\temp\\data-converted.rosz";
            //var report = "c:\\temp\\data-converted.pdf";

            //var package = await Package.LoadAsync(input);
            //Console.WriteLine("Loaded roster");

            //package.Settings.IsCompressed = true;
            //await package.SaveAsync(output);
            //Console.WriteLine("Saved roster");

            //if (package.Roster == null) return;
            //var generator = new DataSheets();
            //generator.Initialise(package.Roster);
            //using var stream = File.Create(report);
            //generator.GeneratePdf(stream);
            //Console.WriteLine("Report generated");

            //Process.Start("explorer.exe", report);

            //var input = "C:\\Users\\csut017\\BattleScribe\\data\\Warhammer 40,000 9th Edition\\Warhammer 40,000.gst";
            var input = "C:\\Users\\csut017\\BattleScribe\\data\\Warhammer 40,000 9th Edition\\T'au Empire.cat";
            var package = await ConfigurationPackage.LoadAsync(input);
        }
    }
}