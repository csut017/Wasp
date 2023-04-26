﻿using System.IO.Compression;

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
        public GameSystemConfiguration? GameSystem { get; set; }

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
                package.GameSystem = await settings.Format.DeserializeConfigurationAsync(stream, settings.ConfigurationType);
            }
            else
            {
                using var archive = new ZipArchive(stream, ZipArchiveMode.Read);

                // There should only be one entry in the archive
                if (archive.Entries.Count > 1) throw new InvalidOperationException("Invalid package file");

                var entry = archive.Entries[0];
                using var zipStream = entry.Open();
                package.GameSystem = await settings.Format.DeserializeConfigurationAsync(zipStream, settings.ConfigurationType);
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