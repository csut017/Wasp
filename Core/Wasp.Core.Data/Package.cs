using System.IO.Compression;

namespace Wasp.Core.Data
{
    /// <summary>
    /// Defines a package for storing a roster.
    /// </summary>
    public class Package
    {
        private Package(PackageSettings settings)
        {
            this.Settings = settings;
        }

        /// <summary>
        /// Gets or sets the roster in the package.
        /// </summary>
        public Roster? Roster { get; set; }

        /// <summary>
        /// Gets or sets the settings for this package.
        /// </summary>
        public PackageSettings Settings { get; set; }

        /// <summary>
        /// Loads a package.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to load from.</param>
        /// <param name="settings">The <see cref="PackageSettings"/> instance to use.</param>
        /// <returns>The new <see cref="Package"/> instance.</returns>
        public static async Task<Package> LoadAsync(Stream stream, PackageSettings settings)
        {
            var package = new Package(settings);
            if (!settings.IsCompressed)
            {
                package.Roster = await settings.Format.DeserializeAsync(stream);
            }
            else
            {
                using var archive = new ZipArchive(stream, ZipArchiveMode.Read);

                // There should only be one entry in the archive
                if (archive.Entries.Count > 1) throw new InvalidOperationException("Invalid package file");

                var entry = archive.Entries[0];
                using var zipStream = entry.Open();
                package.Roster = await settings.Format.DeserializeAsync(zipStream);
            }

            return package;
        }

        /// <summary>
        /// Loads a package.
        /// </summary>
        /// <param name="path">The path to load from.</param>
        /// <param name="settings">The <see cref="PackageSettings"/> instance to use.</param>
        /// <returns>The new <see cref="Package"/> instance.</returns>
        public static async Task<Package> LoadAsync(string path, PackageSettings? settings = null)
        {
            var settingsToUse = settings ?? new PackageSettings
            {
                IsCompressed = Path.GetExtension(path) == ".rosz",
                Name = Path.GetFileNameWithoutExtension(path),
            };
            using var stream = File.OpenRead(path);
            return await LoadAsync(stream, settingsToUse);
        }

        /// <summary>
        /// Starts a new <see cref="Package"/> with the default settings.
        /// </summary>
        /// <returns>The new <see cref="Package"/> instance.</returns>
        public static Package New()
        {
            return new Package(new PackageSettings());
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
            if (this.Roster == null) throw new InvalidOperationException("Roster has not been set.");
            if (!this.Settings.IsCompressed)
            {
                // Save directly to the stream
                await this.Settings.Format.SerializeAsync(this.Roster, stream);
                return;
            }

            using var archive = new ZipArchive(stream, ZipArchiveMode.Create);
            var filename = this.Settings.Name ?? "data";
            var entry = archive.CreateEntry(filename + ".ros");
            using var zipStream = entry.Open();
            await this.Settings.Format.SerializeAsync(this.Roster, zipStream);
        }
    }
}