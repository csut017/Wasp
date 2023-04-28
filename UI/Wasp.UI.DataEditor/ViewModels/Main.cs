using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using Wasp.UI.DataEditor.DataModels;

using Data = Wasp.Core.Data;

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
        private Dictionary<string, Data.CategoryEntry> categoryEntryMap = new();
        private object? currentViewModel;
        private string? filePath;
        private Data.ConfigurationPackage? gameSystem;
        private Visibility loadingVisibility = Visibility.Collapsed;
        private int numberOfChanges;
        private Data.ConfigurationPackage? package;
        private ConfigurationItem? rootNode;
        private ConfigurationItem? selectedItem;
        private bool showImportedEntries;

        public event PropertyChangedEventHandler? PropertyChanged;

        public string ApplicationName
        {
            get => this.applicationName;
            set
            {
                this.applicationName = value;
                this.NotifyPropertyChanged();
            }
        }

        public bool CanRedo { get => this.RedoStack.Any(); }

        public bool CanUndo { get => this.UndoStack.Any(); }

        public ObservableCollection<Data.CategoryEntry> CategoryEntries { get; } = new();

        public object? CurrentViewModel
        {
            get => this.currentViewModel;
            set
            {
                this.currentViewModel = value;
                this.NotifyPropertyChanged();
            }
        }

        public string DefaultExtensionForSave
        {
            get
            {
                return (this.package?.Settings.ConfigurationType) switch
                {
                    Data.ConfigurationType.Catalogue => ".catz",
                    Data.ConfigurationType.GameSystem => ".gstz",
                    _ => throw new ApplicationException($"Unknown configuration type: {this.package?.Settings.ConfigurationType}"),
                };
            }
        }

        public Data.GameSystemConfiguration? Definition
        {
            get => this.package?.Definition;
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
                    Data.ConfigurationType.Catalogue => "Catalogue (*.catz))|*.catz|Uncompressed catalogue (*.cat))|*.cat",
                    Data.ConfigurationType.GameSystem => "Game definition (*.gstz)|*.gstz|Uncompressed game definition (*.gst)|*.gst",
                    _ => throw new ApplicationException($"Unknown configuration type: {this.package?.Settings.ConfigurationType}"),
                };
            }
        }

        public Data.GameSystemConfiguration? GameSystem
        {
            get => this.gameSystem?.Definition;
        }

        public bool HasFile { get; private set; }

        public bool IsDirty
        {
            get => numberOfChanges > 0;
        }

        public ObservableCollection<ConfigurationItem> Items { get; } = new();

        public Visibility LoadingVisibility
        {
            get => loadingVisibility;
            set
            {
                loadingVisibility = value;
                this.NotifyPropertyChanged();
            }
        }

        public ObservableCollection<Data.ProfileType> ProfileTypes { get; } = new();

        public ObservableCollection<Data.Publication> Publications { get; } = new();

        public Stack<UndoAction> RedoStack { get; } = new();

        public ConfigurationItem? SelectedItem
        {
            get => selectedItem;
            set
            {
                selectedItem = value;
                this.NotifyPropertyChanged();
                this.CurrentViewModel = value?.ViewModel;
            }
        }

        public Data.PackageSettings? Settings
        {
            get => this.package?.Settings;
        }

        public bool ShowImportedEntries
        {
            get => showImportedEntries;
            set
            {
                if (showImportedEntries == value) return;
                showImportedEntries = value;
                UpdatedImportedItems();
                NotifyPropertyChanged();
            }
        }

        public Stack<UndoAction> UndoStack { get; } = new();

        public void CloseApplication()
        {
            Application.Current.Shutdown();
        }

        public void EnsureCategoryIsNamed(Data.CategoryLink category)
        {
            if ((category.TargetId != null) && categoryEntryMap.TryGetValue(category.TargetId, out var entry))
            {
                category.DisplayName = entry.Name;
            }
        }

        public void MarkAsDirty(UndoAction? undo = null)
        {
            this.numberOfChanges++;
            this.NotifyPropertyChanged(nameof(IsDirty));
            this.GenerateApplicationName();
            if (undo != null)
            {
                this.UndoStack.Push(undo);
                this.RedoStack.Clear();
                this.NotifyPropertyChanged(nameof(CanRedo));
                this.NotifyPropertyChanged(nameof(CanUndo));
            }
        }

        public async Task OpenAsync(string path)
        {
            this.filePath = path;
            this.package = await Data.ConfigurationPackage.LoadAsync(path);
            if ((this.package?.Definition != null)
                && (this.package?.Definition?.Type != Data.ConfigurationType.GameSystem)
                && !string.IsNullOrEmpty(this.package?.Definition?.GameSystemId)
                && (this.gameSystem?.Definition?.Id != this.package?.Definition?.GameSystemId))
            {
                // Need to load the game system
                var pathToSearch = Path.GetDirectoryName(path) ?? throw new Exception("Invalid directory path");
                this.gameSystem = await this.package!.LoadAssociatedGameSystemAsync(pathToSearch);
            }
            this.HasFile = true;
        }

        public void Redo()
        {
            if (!this.RedoStack.Any()) return;

            var action = this.RedoStack.Pop();
            action.OnRedo();
            action.OnChange();

            this.NotifyPropertyChanged(nameof(CanUndo));
            this.NotifyPropertyChanged(nameof(CanRedo));
            this.CurrentViewModel = action.ViewModel;
            if (action.ViewModel is ViewModel viewModel) this.SelectedItem = viewModel.Item;
            this.UndoStack.Push(action);
        }

        public void Refresh()
        {
            this.GenerateApplicationName();
            this.RefreshData(true, true);
        }

        public async Task SaveAsync()
        {
            if (this.package == null) throw new ApplicationException("No package to save");
            if (string.IsNullOrEmpty(this.filePath)) throw new ApplicationException("FilePath not set");
            package.UpdateSettings(this.filePath);
            await this.package.SaveAsync(this.filePath);
        }

        public void Undo()
        {
            if (!this.UndoStack.Any()) return;

            var action = this.UndoStack.Pop();
            action.OnUndo();
            action.OnChange();

            this.NotifyPropertyChanged(nameof(CanUndo));
            this.NotifyPropertyChanged(nameof(CanRedo));
            this.CurrentViewModel = action.ViewModel;
            if (action.ViewModel is ViewModel viewModel)
            {
                this.SelectedItem = viewModel.Item;
                viewModel.Item.IsSelected = true;
            }
            this.RedoStack.Push(action);
        }

        internal async Task SaveAsync(string fileName)
        {
            this.filePath = fileName;
            await this.SaveAsync();
        }

        protected void NotifyPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private static ConfigurationItem FindOrAddChild(ConfigurationItem root, bool isImported, string name, string? image)
        {
            var item = root.Children.FirstOrDefault(c => c.Name == name);
            if (item != null) return item;

            item = ConfigurationItem.New(isImported, name, image);
            root.Children.Add(item);
            return item;
        }

        private static void RemoveImportedItems(ConfigurationItem nodeToProcess)
        {
            var importedNodes = nodeToProcess.Children.Where(n => n.IsImported).ToList();
            foreach (var node in importedNodes)
            {
                nodeToProcess.Children.Remove(node);
            }

            foreach (var node in nodeToProcess.Children)
            {
                RemoveImportedItems(node);
            }
        }

        private void GenerateApplicationName()
        {
            var isDirty = IsDirty ? "*" : string.Empty;
            this.ApplicationName = this.package == null
                ? DefaultApplicationName
                : $"{DefaultApplicationName} - {this.package?.Definition?.Name} v{this.package?.Definition?.Revision} {isDirty}";
        }

        private void LoadItemsFromDefinition(ConfigurationItem root, Data.GameSystemConfiguration definition, bool isImported)
        {
            if (definition.Type == Data.ConfigurationType.Catalogue)
            {
                FindOrAddChild(root, isImported, "Catalogue Links", "catalogue_link")
                    .PopulateChildren(isImported, "catalogue_link", definition.CatalogueLinks, item => item.Name);
            }

            FindOrAddChild(root, isImported, "Publications", "publication")
                .PopulateChildren(isImported, "publication", definition.Publications, item => item.FullName, (entity, item) => new Publication(entity, this, item));
            FindOrAddChild(root, isImported, "Cost Types", "cost_type")
                .PopulateChildren(isImported, "cost_type", definition.CostTypes, item => item.Name, (entity, item) => new CostType(entity, this, item));
            FindOrAddChild(root, isImported, "Profile Types", "profile_type")
                .PopulateChildren(isImported, "profile_type", definition.ProfileTypes, item => item.Name, (entity, item) => new ProfileType(entity, this, item));
            FindOrAddChild(root, isImported, "Category Entries", "catalogue_entry")
                .PopulateChildren(isImported, "catalogue_entry", definition.CategoryEntries, item => item.Name, (entity, item) => new CategoryEntry(entity, this, item));
            FindOrAddChild(root, isImported, "Force Entries", "force_entry")
                .PopulateChildren(isImported, "force_entry", definition.ForceEntries, item => item.Name);
            FindOrAddChild(root, isImported, "Shared Selection Entries", "shared_selection_entry")
                .PopulateChildren(isImported, "selection_entry", definition.SharedSelectionEntries, item => item.Name, (entity, item) => new SelectionEntry(entity, this, item));
            FindOrAddChild(root, isImported, "Shared Selection Entry Groups", "shared_selection_entry_group")
                .PopulateChildren(isImported, "selection_entry_group", definition.SharedSelectionEntryGroups, item => item.Name, (entity, item) => new SelectionEntryGroup(entity, this, item));
            FindOrAddChild(root, isImported, "Shared Profiles", "shared_profile")
                .PopulateChildren(isImported, "profile", definition.SharedProfiles, item => item.Name, (entity, item) => new Profile(entity, this, item));
            FindOrAddChild(root, isImported, "Shared Rules", "shared_rule")
                .PopulateChildren(isImported, "rule", definition.SharedRules, item => item.Name, (entity, item) => new Rule(entity, this, item));
            FindOrAddChild(root, isImported, "Shared Info Groups", "shared_info")
                .PopulateChildren(isImported, "info", definition.SharedInformationGroups, item => item.Name);
            FindOrAddChild(root, isImported, "Root Selection Entries", "selection_entry")
                .PopulateChildren(isImported, "selection_entry", definition.EntryLinks, item => item.Name);
            FindOrAddChild(root, isImported, "Root Rules", "rule")
                .PopulateChildren(isImported, "rule", definition.Rules, item => item.Name, (entity, item) => new Rule(entity, this, item));
        }

        private void RefreshCategoryEntries()
        {
            this.CategoryEntries.Clear();
            this.CategoryEntries.Add(new Data.CategoryEntry());
            if (this.Definition?.CategoryEntries != null)
            {
                foreach (var categoryEntry in this.Definition.CategoryEntries.OrderBy(p => p.Name))
                {
                    categoryEntry.DisplayName = categoryEntry.Name;
                    this.CategoryEntries.Add(categoryEntry);
                }
            }

            if (this.gameSystem?.Definition?.CategoryEntries != null)
            {
                var systemName = this.gameSystem?.Definition.Name;
                foreach (var categoryEntry in this.gameSystem!.Definition.CategoryEntries.OrderBy(p => p.Name))
                {
                    categoryEntry.DisplayName = $"{categoryEntry.Name} [{systemName}]";
                    this.CategoryEntries.Add(categoryEntry);
                }
            }

            this.categoryEntryMap = this.CategoryEntries
                .Where(ce => ce?.Id != null)
                .ToDictionary(ce => ce.Id!);
        }

        private void RefreshData(bool clearStacks, bool clearData)
        {
            if (clearData)
            {
                RefreshPublications();
                RefreshProfileTypes();
                RefreshCategoryEntries();
            }

            if (clearStacks)
            {
                this.UndoStack.Clear();
                this.RedoStack.Clear();
            }

            // Load the view models last
            if (clearData)
            {
                ReloadItems();
            }

            UpdatedImportedItems();
        }

        private void RefreshProfileTypes()
        {
            this.ProfileTypes.Clear();
            this.ProfileTypes.Add(new Data.ProfileType());
            if (this.Definition?.ProfileTypes != null)
            {
                foreach (var profileType in this.Definition.ProfileTypes.OrderBy(p => p.Name))
                {
                    profileType.DisplayName = profileType.Name;
                    this.ProfileTypes.Add(profileType);
                }
            }

            if (this.gameSystem?.Definition?.ProfileTypes != null)
            {
                var systemName = this.gameSystem?.Definition.Name;
                foreach (var profileType in this.gameSystem!.Definition.ProfileTypes.OrderBy(p => p.Name))
                {
                    profileType.DisplayName = $"{profileType.Name} [{systemName}]";
                    this.ProfileTypes.Add(profileType);
                }
            }
        }

        private void RefreshPublications()
        {
            this.Publications.Clear();
            this.Publications.Add(new Data.Publication());
            if (this.Definition?.Publications != null)
            {
                foreach (var publication in this.Definition.Publications.OrderBy(p => p.FullName))
                {
                    publication.DisplayName = publication.FullName;
                    this.Publications.Add(publication);
                }
            }

            if (this.gameSystem?.Definition?.Publications != null)
            {
                var systemName = this.gameSystem?.Definition.Name;
                foreach (var publication in this.gameSystem!.Definition.Publications.OrderBy(p => p.FullName))
                {
                    publication.DisplayName = $"{publication.FullName} [{systemName}]";
                    this.Publications.Add(publication);
                }
            }
        }

        private void ReloadItems()
        {
            this.Items.Clear();
            if (this.package?.Definition == null) return;
            rootNode = ConfigurationItem.New(false, $"{this.package.Definition.Name} v{this.package.Definition.Revision}", "catalogue");
            rootNode.IsExpanded = true;
            rootNode.Item = this.package.Definition;
            rootNode.ViewModel = new Catalogue(this.package.Definition, this, rootNode);
            this.Items.Add(rootNode);
            LoadItemsFromDefinition(rootNode, this.package.Definition, false);
        }

        private void UpdatedImportedItems()
        {
            if (rootNode == null) throw new Exception("Must initialise root node first");
            if (this.showImportedEntries)
            {
                if (this.gameSystem?.Definition != null) LoadItemsFromDefinition(this.rootNode, this.gameSystem.Definition, true);
            }
            else
            {
                RemoveImportedItems(rootNode);
            }
        }
    }
}