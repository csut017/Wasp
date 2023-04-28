using System.Diagnostics;
using System.IO.Compression;

namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines a package for storing configuration about a game system.
    /// </summary>
    public class ConfigurationPackage
    {
        private ConfigurationPackage(PackageSettings settings)
        {
            this.Settings = settings;
        }

        /// <summary>
        /// Gets or sets the game system in the package.
        /// </summary>
        public GameSystemConfiguration? Definition { get; set; }

        /// <summary>
        /// Gets or sets the settings for this package.
        /// </summary>
        public PackageSettings Settings { get; set; }

        /// <summary>
        /// Loads a package.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to load from.</param>
        /// <param name="settings">The <see cref="PackageSettings"/> instance to use.</param>
        /// <returns>The new <see cref="ConfigurationPackage"/> instance.</returns>
        public static async Task<ConfigurationPackage> LoadAsync(Stream stream, PackageSettings settings)
        {
            var package = new ConfigurationPackage(settings);
            if (!settings.IsCompressed)
            {
                package.Definition = await settings.Format.DeserializeConfigurationAsync(stream, settings.ConfigurationType);
            }
            else
            {
                using var archive = new ZipArchive(stream, ZipArchiveMode.Read);

                // There should only be one entry in the archive
                if (archive.Entries.Count > 1) throw new InvalidOperationException("Invalid package file");

                var entry = archive.Entries[0];
                using var zipStream = entry.Open();
                try
                {
                    package.Definition = await settings.Format.DeserializeConfigurationAsync(zipStream, settings.ConfigurationType);
                }
                catch
                {
#if DEBUG
                    var filename = Path.GetTempFileName();
                    entry.ExtractToFile(filename, true);
                    Debug.WriteLine($"Wrote file to {filename}");
#endif
                    throw;
                }
            }

            return package;
        }

        /// <summary>
        /// Loads a package.
        /// </summary>
        /// <param name="path">The path to load from.</param>
        /// <param name="settings">The <see cref="PackageSettings"/> instance to use.</param>
        /// <returns>The new <see cref="ConfigurationPackage"/> instance.</returns>
        public static async Task<ConfigurationPackage> LoadAsync(string path, PackageSettings? settings = null)
        {
            var fileType = Path.GetExtension(path);
            var settingsToUse = settings ?? new PackageSettings
            {
                IsCompressed = fileType.EndsWith("z"),
                Name = Path.GetFileNameWithoutExtension(path),
            };

            if (fileType.StartsWith(".gst"))
            {
                settingsToUse.ConfigurationType = ConfigurationType.GameSystem;
            }
            else if (fileType.StartsWith(".cat"))
            {
                settingsToUse.ConfigurationType = ConfigurationType.Catalogue;
            }

            using var stream = File.OpenRead(path);
            return await LoadAsync(stream, settingsToUse);
        }

        /// <summary>
        /// Starts a new <see cref="ConfigurationPackage"/> with the default settings.
        /// </summary>
        /// <returns>The new <see cref="ConfigurationPackage"/> instance.</returns>
        public static ConfigurationPackage New()
        {
            return new ConfigurationPackage(new PackageSettings());
        }

        /// <summary>
        /// Loads an associated game system definition.
        /// </summary>
        /// <param name="pathToSearch">The path to search for the game system definition.</param>
        /// <returns>A <see cref="ConfigurationPackage"/> containing the associated game system if found; null otherwise.</returns>
        public async Task<ConfigurationPackage?> LoadAssociatedGameSystemAsync(string pathToSearch)
        {
            if (this.Definition == null) throw new Exception("Definition not loaded");
            // Get the candidate files
            var files = Directory.GetFiles(pathToSearch, "*.gst*", SearchOption.TopDirectoryOnly);
            foreach (var file in files.Where(f => f.EndsWith(".gst") || f.EndsWith(".gstz")))
            {
                var gameSystemId = await LoadIdAsync(file);
                if (this.Definition.GameSystemId != gameSystemId) continue;

                // We've found the right file, so let's load it
                var gameSystem = await ConfigurationPackage.LoadAsync(file);
                return gameSystem;
            }

            return null;
        }

        /// <summary>
        /// Loads the id of a definition from a file.
        /// </summary>
        /// <param name="filePath">The path of the file.</param>
        /// <returns>The id of the contained definition if found; null otherwise.</returns>
        public async Task<string?> LoadIdAsync(string filePath)
        {
            using var stream = File.Open(filePath, FileMode.Open);
            if (!filePath.EndsWith("z"))
            {
                return await this.Settings.Format.DeserializeIdAsync(stream);
            }

            using var archive = new ZipArchive(stream, ZipArchiveMode.Read);
            if (archive.Entries.Count > 1) throw new InvalidOperationException("Invalid package file");

            var entry = archive.Entries[0];
            using var zipStream = entry.Open();
            return await this.Settings.Format.DeserializeIdAsync(zipStream);
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
            if (this.Definition == null) throw new InvalidOperationException("Game system has not been set.");
            if (!this.Settings.IsCompressed)
            {
                // Save directly to the stream
                await this.Settings.Format.SerializeGameSystemAsync(this.Definition, stream, this.Settings.ConfigurationType);
                return;
            }

            using var archive = new ZipArchive(stream, ZipArchiveMode.Create);
            var filename = this.Settings.Name ?? "data";
            var fileExtension = (this.Settings.ConfigurationType) switch
            {
                ConfigurationType.Catalogue => ".cat",
                ConfigurationType.GameSystem => ".gst",
                _ => throw new ApplicationException($"Unknown configuration type: {this.Settings.ConfigurationType}"),
            };
            var entry = archive.CreateEntry(filename + fileExtension);
            using var zipStream = entry.Open();
            await this.Settings.Format.SerializeGameSystemAsync(this.Definition, zipStream, this.Settings.ConfigurationType);
        }

        /// <summary>
        /// Updates the settings to match the file path.
        /// </summary>
        /// <param name="path">The file path to use.</param>
        public void UpdateSettings(string path)
        {
            var fileType = Path.GetExtension(path);
            var settingsToUse = this.Settings ?? new PackageSettings();
            settingsToUse.IsCompressed = fileType.EndsWith("z");
            settingsToUse.Name = Path.GetFileNameWithoutExtension(path);
            if (fileType.StartsWith(".gst"))
            {
                settingsToUse.ConfigurationType = ConfigurationType.GameSystem;
            }
            else if (fileType.StartsWith(".cat"))
            {
                settingsToUse.ConfigurationType = ConfigurationType.Catalogue;
            }

            this.Settings = settingsToUse;
        }
    }
}