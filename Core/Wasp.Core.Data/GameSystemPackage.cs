using System.IO.Compression;

namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines a package for storing a game system.
    /// </summary>
    public class GameSystemPackage
    {
        private GameSystemPackage(PackageSettings settings)
        {
            this.Settings = settings;
        }

        /// <summary>
        /// Gets or sets the game system in the package.
        /// </summary>
        public GameSystem? GameSystem { get; set; }

        /// <summary>
        /// Gets or sets the settings for this package.
        /// </summary>
        public PackageSettings Settings { get; set; }

        /// <summary>
        /// Loads a package.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to load from.</param>
        /// <param name="settings">The <see cref="PackageSettings"/> instance to use.</param>
        /// <returns>The new <see cref="GameSystemPackage"/> instance.</returns>
        public static async Task<GameSystemPackage> LoadAsync(Stream stream, PackageSettings settings)
        {
            var package = new GameSystemPackage(settings);
            if (!settings.IsCompressed)
            {
                package.GameSystem = await settings.Format.DeserializeGameSystemAsync(stream);
            }
            else
            {
                using var archive = new ZipArchive(stream, ZipArchiveMode.Read);

                // There should only be one entry in the archive
                if (archive.Entries.Count > 1) throw new InvalidOperationException("Invalid package file");

                var entry = archive.Entries[0];
                using var zipStream = entry.Open();
                package.GameSystem = await settings.Format.DeserializeGameSystemAsync(zipStream);
            }

            return package;
        }

        /// <summary>
        /// Loads a package.
        /// </summary>
        /// <param name="path">The path to load from.</param>
        /// <param name="settings">The <see cref="PackageSettings"/> instance to use.</param>
        /// <returns>The new <see cref="GameSystemPackage"/> instance.</returns>
        public static async Task<GameSystemPackage> LoadAsync(string path, PackageSettings? settings = null)
        {
            var settingsToUse = settings ?? new PackageSettings
            {
                IsCompressed = Path.GetExtension(path) == ".gstz",
                Name = Path.GetFileNameWithoutExtension(path),
            };
            using var stream = File.OpenRead(path);
            return await LoadAsync(stream, settingsToUse);
        }

        /// <summary>
        /// Starts a new <see cref="GameSystemPackage"/> with the default settings.
        /// </summary>
        /// <returns>The new <see cref="GameSystemPackage"/> instance.</returns>
        public static GameSystemPackage New()
        {
            return new GameSystemPackage(new PackageSettings());
        }

        /// <summary>
        /// Saves the package.
        /// </summary>
        /// <param name="path">The path to save the package to.</param>
        public async Task SaveAsync(string path)
        {
            using var stream = File.Create(path);
            await SaveAsync(stream);
        }

        /// <summary>
        /// Saves the package.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to use.</param>
        public async Task SaveAsync(Stream stream)
        {
            if (this.GameSystem == null) throw new InvalidOperationException("Game system has not been set.");
            if (!this.Settings.IsCompressed)
            {
                // Save directly to the stream
                await this.Settings.Format.SerializeGameSystemAsync(this.GameSystem, stream);
                return;
            }

            using var archive = new ZipArchive(stream, ZipArchiveMode.Create);
            var filename = this.Settings.Name ?? "data";
            var entry = archive.CreateEntry(filename + ".ros");
            using var zipStream = entry.Open();
            await this.Settings.Format.SerializeGameSystemAsync(this.GameSystem, zipStream);
        }
    }
}