using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using Wasp.Core.Data;
using Wasp.UI.DataEditor.DataModels;

namespace Wasp.UI.DataEditor.ViewModels
{
    /// <summary>
    /// Main data model for the application.
    /// </summary>
    public class Main
        : INotifyPropertyChanged
    {
        private const string DefaultApplicationName = "WASP: Data Editor 0.01";
        private string applicationName = DefaultApplicationName;
        private string? filePath;
        private ConfigurationPackage? package;

        public event PropertyChangedEventHandler? PropertyChanged;

        public string ApplicationName
        {
            get => this.applicationName;
            set
            {
                this.applicationName = value;
                this.NotifyPropertyChanged(nameof(ApplicationName));
            }
        }

        public string DefaultExtensionForSave
        {
            get
            {
                return (this.package?.Settings.ConfigurationType) switch
                {
                    ConfigurationType.Catalogue => ".catz",
                    ConfigurationType.GameSystem => ".gstz",
                    _ => throw new ApplicationException($"Unknown configuration type: {this.package?.Settings.ConfigurationType}"),
                };
            }
        }

        public string? FileName
        {
            get => this.filePath;
        }

        public string FilterForSave
        {
            get
            {
                return (this.package?.Settings.ConfigurationType) switch
                {
                    ConfigurationType.Catalogue => "Catalogue (*.catz))|*.catz|Uncompressed catalogue (*.cat))|*.cat",
                    ConfigurationType.GameSystem => "Game definition (*.gstz)|*.gstz|Uncompressed game definition (*.gst)|*.gst",
                    _ => throw new ApplicationException($"Unknown configuration type: {this.package?.Settings.ConfigurationType}"),
                };
            }
        }

        public bool HasFile { get; private set; }

        public ObservableCollection<ConfigurationItem> Items { get; } = new();

        public PackageSettings? Settings
        {
            get => this.package?.Settings;
        }

        public void CloseApplication()
        {
            Application.Current.Shutdown();
        }

        public async Task OpenAsync(string path)
        {
            this.filePath = path;
            this.package = await ConfigurationPackage.LoadAsync(path);
            this.HasFile = true;
            this.GenerateApplicationName();
            this.RefreshItems();
        }

        public async Task SaveAsync()
        {
            if (this.package == null) throw new ApplicationException("No package to save");
            if (string.IsNullOrEmpty(this.filePath)) throw new ApplicationException("FilePath not set");
            package.UpdateSettings(this.filePath);
            await this.package.SaveAsync(this.filePath);
        }

        internal async Task SaveAsync(string fileName)
        {
            this.filePath = fileName;
            await this.SaveAsync();
        }

        private void GenerateApplicationName()
        {
            this.ApplicationName = this.package == null
                ? DefaultApplicationName
                : $"{DefaultApplicationName} - {this.package?.Definition?.Name} v{this.package?.Definition?.Revision}";
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void RefreshItems()
        {
            this.Items.Clear();
            if (this.package?.Definition == null) return;
            var root = new ConfigurationItem
            {
                Item = this.package.Definition,
                IsExpanded = true,
                Name = $"{this.package.Definition.Name} v{this.package.Definition.Revision}"
            };
            this.Items.Add(root);
            root.Children.Add(ConfigurationItem.New("Catalogue Links", this.package.Definition.CatalogueLinks, item => item.Name));
            root.Children.Add(ConfigurationItem.New("Publications", this.package.Definition.Publications, item => item.FullName));
            root.Children.Add(ConfigurationItem.New("Cost Types", this.package.Definition.CostTypes, item => item.Name));
            root.Children.Add(ConfigurationItem.New("Profile Types", this.package.Definition.ProfileTypes, item => item.Name));
            root.Children.Add(ConfigurationItem.New("Category Entries", this.package.Definition.CategoryEntries, item => item.Name));
            root.Children.Add(ConfigurationItem.New("Force Entries", this.package.Definition.ForceEntries, item => item.Name));
            root.Children.Add(ConfigurationItem.New("Shared Selection Entries", this.package.Definition.SharedSelectionEntries, item => item.Name));
            root.Children.Add(ConfigurationItem.New("Shared Selection Entry Groups", this.package.Definition.SharedSelectionEntryGroups, item => item.Name));
            root.Children.Add(ConfigurationItem.New("Shared Profiles", this.package.Definition.SharedProfiles, item => item.Name));
            root.Children.Add(ConfigurationItem.New("Shared Rules", this.package.Definition.SharedRules, item => item.Name));
            root.Children.Add(ConfigurationItem.New("Shared Info Groups", this.package.Definition.SharedInformationGroups, item => item.Name));
            root.Children.Add(ConfigurationItem.New("Root Selection Entries", this.package.Definition.EntryLinks, item => item.Name));
            root.Children.Add(ConfigurationItem.New("Root Rules", this.package.Definition.Rules, item => item.Name));
        }
    }
}