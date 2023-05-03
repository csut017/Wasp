using System.Collections.ObjectModel;

namespace Wasp.Core.Data
{
    /// <summary>
    /// Manages all the game systems and catalogues on disk.
    /// </summary>
    public class GameSystemManager
    {
        private readonly List<GameSystemIndex> indices = new();

        /// <summary>
        /// Initialises a new <see cref="GameSystemManager"/> instance.
        /// </summary>
        public GameSystemManager()
        {
            this.Indices = indices.AsReadOnly();
            this.DataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Wasp");
            if (!Directory.Exists(DataPath)) Directory.CreateDirectory(DataPath);
        }

        /// <summary>
        /// Gets or sets the path to the data.
        /// </summary>
        public string DataPath { get; set; }

        /// <summary>
        /// Gets the current indices.
        /// </summary>
        public ReadOnlyCollection<GameSystemIndex> Indices { get; private set; }

        /// <summary>
        /// Generates an index from the data directories.
        /// </summary>
        /// <remarks>
        /// The index is used to speed up data loading and to check for any changes.
        /// </remarks>
        public async Task GenerateIndexAsync()
        {
            // All game system files should be in the data folder
            var datapath = Path.Combine(this.DataPath, DataPath, "data");

            // Only search the top-level folders
            foreach (var dir in Directory.GetDirectories(datapath))
            {
                var definitions = Directory.GetFiles(dir);
                var catalogues = definitions.Where(f => f.EndsWith(".catz") || f.EndsWith(".cat"));
                var gameSystems = new Dictionary<string, GameSystemIndex>();
                foreach (var definition in definitions.Where(f => f.EndsWith(".gstz") || f.EndsWith(".gst")))
                {
                    var ext = Path.GetExtension(definition);
                    var package = await ConfigurationPackage.LoadAsync(definition, level: ConfigurationLevel.Root);
                    if (package?.Definition == null) continue;
                    var index = package.Definition.AsIndex();
                    index.Children = new();
                    this.indices.Add(index);
                    if (index.Id == null) continue;
                    gameSystems.Add(index.Id, index);
                }

                foreach (var catalogue in catalogues)
                {
                    var package = await ConfigurationPackage.LoadAsync(catalogue, level: ConfigurationLevel.Root);
                    if (package?.Definition == null) continue;
                    var index = package.Definition.AsIndex();
                    if ((index.GameSystemId == null) || !gameSystems.TryGetValue(index.GameSystemId, out var gameSystem)) return;
                    gameSystem.Children!.Add(index);
                }
            }
        }

        /// <summary>
        /// Imports the BattleScribe files and converts as necessary.
        /// </summary>
        /// <param name="path">An optional path to import from. If omitted it will use the BattleScribe folder in the user's profile.</param>
        /// <param name="format">The format of the Wasp data files.</param>
        public async Task ImportFromBattleScribeDataAync(string? path = null, IFormatProvider? format = null)
        {
            // To be safe, rebuild the index first
            await GenerateIndexAsync();
            path ??= Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "BattleScribe");
            if (!Directory.Exists(path)) return;

            await CopyFolderAsync(path, this.DataPath);
        }

        private static async Task CopyFolderAsync(string path, string dataPath)
        {
            if (!Directory.Exists(dataPath)) Directory.CreateDirectory(dataPath);
            foreach (var file in Directory.GetFiles(path))
            {
                var newPath = Path.Combine(dataPath, Path.GetFileName(file));
                if (!File.Exists(newPath)) File.Copy(file, newPath);
            }

            foreach (var directory in Directory.GetDirectories(path))
            {
                var newPath = Path.Combine(dataPath, Path.GetFileName(directory)!);
                await CopyFolderAsync(directory, newPath);
            }
        }
    }
}