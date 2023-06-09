﻿using System;
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

        public Main()
        {
            this.AddCommands = new(this);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public AddCommands AddCommands { get; }

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

        public ObservableCollection<Data.CostType> CostTypes { get; } = new();

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

        public ObservableCollection<DynamicMenuItem> MenuItemsForAddingEntries { get; } = new();

        public ObservableCollection<Data.ProfileType> ProfileTypes { get; } = new();

        public ObservableCollection<Data.Publication> Publications { get; } = new();

        public Stack<UndoAction> RedoStack { get; } = new();

        public ConfigurationItem? SelectedItem
        {
            get => selectedItem;
            set
            {
                if (object.ReferenceEquals(selectedItem, value)) return;
                if (selectedItem != null) selectedItem.IsSelected = false;
                selectedItem = value;
                if (selectedItem != null) selectedItem.IsSelected = true;
                this.NotifyPropertyChanged();
                this.CurrentViewModel = value?.ViewModel;
                this.AddCommands.PopulateMenuOptions(this.MenuItemsForAddingEntries);
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

        public IEnumerable<Data.ItemCost> PopulateCosts(List<Data.ItemCost>? costs)
        {
            var working = CostTypes
                .Where(c => !string.IsNullOrEmpty(c.Id))
                .ToDictionary(c => c.Id!, c => new Data.ItemCost { Name = c.Name, Value = "0", });

            if (costs != null)
            {
                foreach (var cost in costs.Where(c => !string.IsNullOrEmpty(c.TypeId)))
                {
                    if (working.TryGetValue(cost.TypeId!, out var costItem)) costItem!.Value = cost.Value;
                }
            }

            return working.Values.OrderBy(c => c.Name);
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

        private ConfigurationItem FindOrAddChild(ConfigurationItem root, bool isImported, string name, string? image)
        {
            var item = root.Children.FirstOrDefault(c => c.Name == name);
            if (item != null) return item;

            item = ConfigurationItem.New(isImported, name, image);
            item.Item = this.package?.Definition;
            root.Children.Add(item);
            return item;
        }

        private void GenerateApplicationName()
        {
            var isDirty = IsDirty ? "*" : string.Empty;
            this.ApplicationName = this.package == null
                ? DefaultApplicationName
                : $"{DefaultApplicationName} - {this.package?.Definition?.Name} v{this.package?.Definition?.Revision} {isDirty}";
        }

        private void LoadItemsFromDefinition(ConfigurationItem root, Data.GameSystemConfiguration definition, bool isImported, string suffix = "")
        {
            if (definition.Type == Data.ConfigurationType.Catalogue)
            {
                FindOrAddChild(root, isImported, "Catalogue Links", "catalogue_link")
                    .AddAllowEntryTypes(AddableEntryType.CatalogueLink)
                    .PopulateChildren(isImported, "catalogue_link", definition.CatalogueLinks, item => item.Name + suffix);
            }

            FindOrAddChild(root, isImported, "Publications", "publication")
                .AddAllowEntryTypes(AddableEntryType.Publication)
                .PopulateChildren(isImported, "publication", definition.Publications, item => item.FullName + suffix, (entity, item) => new Publication(entity, this, item));
            FindOrAddChild(root, isImported, "Cost Types", "cost_type")
                .AddAllowEntryTypes(AddableEntryType.CostType)
                .PopulateChildren(isImported, "cost_type", definition.CostTypes, item => item.Name + suffix, (entity, item) => new CostType(entity, this, item));
            FindOrAddChild(root, isImported, "Profile Types", "profile_type")
                .AddAllowEntryTypes(AddableEntryType.ProfileType)
                .PopulateChildren(isImported, "profile_type", definition.ProfileTypes, item => item.Name + suffix, (entity, item) => new ProfileType(entity, this, item));
            FindOrAddChild(root, isImported, "Category Entries", "catalogue_entry")
                .AddAllowEntryTypes(AddableEntryType.CategoryEntry)
                .PopulateChildren(isImported, "catalogue_entry", definition.CategoryEntries, item => item.Name + suffix, (entity, item) => new CategoryEntry(entity, this, item));
            FindOrAddChild(root, isImported, "Force Entries", "force_entry")
                .AddAllowEntryTypes(AddableEntryType.ForceEntry)
                .PopulateChildren(isImported, "force_entry", definition.ForceEntries, item => item.Name + suffix, (entity, item) => new ForceEntry(entity, this, item));
            FindOrAddChild(root, isImported, "Shared Selection Entries", "shared_selection_entry")
                .AddAllowEntryTypes(AddableEntryType.SharedSelectionEntry)
                .PopulateChildren(isImported, "selection_entry", definition.SharedSelectionEntries, item => item.Name + suffix, (entity, item) => new SelectionEntry(entity, this, item));
            FindOrAddChild(root, isImported, "Shared Selection Entry Groups", "shared_selection_entry_group")
                .AddAllowEntryTypes(AddableEntryType.SharedSelectionEntryGroup)
                .PopulateChildren(isImported, "selection_entry_group", definition.SharedSelectionEntryGroups, item => item.Name + suffix, (entity, item) => new SelectionEntryGroup(entity, this, item));
            FindOrAddChild(root, isImported, "Shared Profiles", "shared_profile")
                .AddAllowEntryTypes(AddableEntryType.SharedProfile)
                .PopulateChildren(isImported, "profile", definition.SharedProfiles, item => item.Name + suffix, (entity, item) => new Profile(entity, this, item));
            FindOrAddChild(root, isImported, "Shared Rules", "shared_rule")
                .AddAllowEntryTypes(AddableEntryType.SharedRule)
                .PopulateChildren(isImported, "rule", definition.SharedRules, item => item.Name + suffix, (entity, item) => new Rule(entity, this, item));
            FindOrAddChild(root, isImported, "Shared Info Groups", "shared_info")
                .AddAllowEntryTypes(AddableEntryType.SharedInfoGroup)
                .PopulateChildren(isImported, "info", definition.SharedInformationGroups, item => item.Name + suffix, (entity, item) => new InformationGroup(entity, this, item));
            FindOrAddChild(root, isImported, "Root Selection Entries", "selection_entry")
                .AddAllowEntryTypes(AddableEntryType.SelectionEntry | AddableEntryType.EntryLink)
                .PopulateChildren(isImported, "selection_entry", definition.EntryLinks, item => item.Name + suffix, (entity, item) => new EntryLink(entity, this, item));
            FindOrAddChild(root, isImported, "Root Rules", "rule")
                .AddAllowEntryTypes(AddableEntryType.Rule | AddableEntryType.InfoLink)
                .PopulateChildren(isImported, "rule", definition.Rules, item => item.Name + suffix, (entity, item) => new Rule(entity, this, item));
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

        private void RefreshCostTypes()
        {
            this.CostTypes.Clear();
            this.CostTypes.Add(new Data.CostType());
            if (this.Definition?.CostTypes != null)
            {
                foreach (var costType in this.Definition.CostTypes.OrderBy(p => p.Name))
                {
                    costType.DisplayName = costType.Name;
                    this.CostTypes.Add(costType);
                }
            }

            if (this.gameSystem?.Definition?.CostTypes != null)
            {
                var systemName = this.gameSystem?.Definition.Name;
                foreach (var costType in this.gameSystem!.Definition.CostTypes.OrderBy(p => p.Name))
                {
                    costType.DisplayName = $"{costType.Name} [{systemName}]";
                    this.CostTypes.Add(costType);
                }
            }
        }

        private void RefreshData(bool clearStacks, bool clearData)
        {
            if (clearData)
            {
                RefreshCostTypes();
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
                this.SelectedItem = rootNode;
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
            rootNode.AddableEntryTypes = AddableEntryType.CatalogueEntries;
            this.Items.Add(rootNode);
            LoadItemsFromDefinition(rootNode, this.package.Definition, false);
        }

        private void UpdatedImportedItems()
        {
            if (rootNode == null) throw new Exception("Must initialise root node first");
            if (this.showImportedEntries)
            {
                if (this.gameSystem?.Definition != null) LoadItemsFromDefinition(this.rootNode, this.gameSystem.Definition, true, $" [{this.gameSystem.Definition.Name}]");
            }
            else
            {
                RemoveImportedItems(rootNode);
            }
        }
    }
}