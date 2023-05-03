using System.Reflection;
using Wasp.Core.Data;

namespace Wasp.Utility
{
    internal class Program
    {
        public static async Task AnalyseLibrary()
        {
            var assembly = typeof(RosterPackage).Assembly;
            var typeNames = new HashSet<string>();
            var classes = new List<Tuple<string, string?, List<Tuple<string, string>>>>();
            foreach (var type in assembly.GetExportedTypes().Where(c => c.IsClass))
            {
                var properties = new List<Tuple<string, string>>();
                foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy))
                {
                    string propertyType = property.PropertyType.Name;
                    if (property.DeclaringType != type) propertyType = "*" + propertyType;
                    properties.Add(Tuple.Create(property.Name, propertyType));

                    if (!typeNames.Contains(property.Name)) typeNames.Add(property.Name);
                }

                var parent = type.BaseType?.Name;
                classes.Add(Tuple.Create(type.Name, parent, properties));
            }

            var count = 0;
            var types = new Dictionary<string, int>();
            foreach (var type in typeNames.OrderBy(t => t))
            {
                types.Add(type, count++);
            }

            using var output = File.CreateText(@"C:\temp\classes.csv");
            var headers = typeNames.OrderBy(t => t).ToList();
            headers.Insert(0, "Class");
            headers.Insert(1, "Parent");
            await output.WriteLineAsync(string.Join(',', headers));
            var template = (string[])Array.CreateInstance(typeof(string), count + 2);
            foreach (var @class in classes.OrderBy(c => c.Item1))
            {
                var row = (string[])template.Clone();
                row[0] = @class.Item1;
                row[1] = @class.Item2 ?? string.Empty;
                foreach (var property in @class.Item3)
                {
                    var index = types[property.Item1] + 2;
                    row[index] = property.Item2;
                }

                await output.WriteLineAsync(string.Join(',', row));
            }
        }

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
            //var input = "C:\\Users\\csut017\\BattleScribe\\data\\Warhammer 40,000 9th Edition\\Warhammer 40,000 9th Edition.gst";
            //var output = "C:\\Users\\csut017\\BattleScribe\\data\\Warhammer 40,000 9th Edition\\Warhammer 40,000 9th Edition-copy.gst";
            //var package = await ConfigurationPackage.LoadAsync(input);
            //await package.SaveAsync(output);

            await AnalyseLibrary();
        }
    }
}