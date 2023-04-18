using QuestPDF.Fluent;
using System.Diagnostics;
using Wasp.Core.Data;
using Wasp.Reports.Warhammer40K;

namespace Wasp.Utility
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var input = "C:\\Users\\csut017\\BattleScribe\\rosters\\Ay Eldi (T'au).rosz";
            var output = "c:\\temp\\data-converted.rosz";
            var report = "c:\\temp\\data-converted.pdf";

            var package = await Package.LoadAsync(input);
            Console.WriteLine("Loaded roster");

            package.Settings.IsCompressed = true;
            await package.SaveAsync(output);
            Console.WriteLine("Saved roster");

            if (package.Roster == null) return;
            var generator = new CrusadeForce(package.Roster);
            using var stream = File.Create(report);
            generator.GeneratePdf(stream);
            Console.WriteLine("Report generated");

            Process.Start("explorer.exe", report);
        }
    }
}