﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        private object? currentViewModel;
        private string? filePath;
        private Visibility loadingVisibility = Visibility.Collapsed;
        private int numberOfChanges;
        private Data.ConfigurationPackage? package;
        private ConfigurationItem? selectedItem;

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

        public Stack<UndoAction> UndoStack { get; } = new();

        public void CloseApplication()
        {
            Application.Current.Shutdown();
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
            this.RefreshData(true);
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

        private void GenerateApplicationName()
        {
            var isDirty = IsDirty ? "*" : string.Empty;
            this.ApplicationName = this.package == null
                ? DefaultApplicationName
                : $"{DefaultApplicationName} - {this.package?.Definition?.Name} v{this.package?.Definition?.Revision} {isDirty}";
        }

        private void RefreshData(bool clearStacks)
        {
            RefreshItems();
            RefreshPublications();
            if (clearStacks)
            {
                this.UndoStack.Clear();
                this.RedoStack.Clear();
            }
        }

        private void RefreshItems()
        {
            this.Items.Clear();
            if (this.package?.Definition == null) return;
            var root = new ConfigurationItem
            {
                Item = this.package.Definition,
                IsExpanded = true,
                Name = $"{this.package.Definition.Name} v{this.package.Definition.Revision}",
            };
            root.ViewModel = new Catalogue(this.package.Definition, this, root);
            this.Items.Add(root);

            var definitionType = this.package.Definition.Type;
            if (definitionType == Data.ConfigurationType.Catalogue) root.Children.Add(ConfigurationItem.New("Catalogue Links", this.package.Definition.CatalogueLinks, item => item.Name));
            root.Children.Add(ConfigurationItem.New("Publications", this.package.Definition.Publications, item => item.FullName, (entity, item) => new Publication(entity, this, item)));
            root.Children.Add(ConfigurationItem.New("Cost Types", this.package.Definition.CostTypes, item => item.Name, (entity, item) => new CostType(entity, this, item)));
            root.Children.Add(ConfigurationItem.New("Profile Types", this.package.Definition.ProfileTypes, item => item.Name, (entity, item) => new ProfileType(entity, this, item)));
            root.Children.Add(ConfigurationItem.New("Category Entries", this.package.Definition.CategoryEntries, item => item.Name));
            root.Children.Add(ConfigurationItem.New("Force Entries", this.package.Definition.ForceEntries, item => item.Name));
            root.Children.Add(ConfigurationItem.New("Shared Selection Entries", this.package.Definition.SharedSelectionEntries, item => item.Name));
            root.Children.Add(ConfigurationItem.New("Shared Selection Entry Groups", this.package.Definition.SharedSelectionEntryGroups, item => item.Name));
            root.Children.Add(ConfigurationItem.New("Shared Profiles", this.package.Definition.SharedProfiles, item => item.Name, (entity, item) => new Profile(entity, this, item)));
            root.Children.Add(ConfigurationItem.New("Shared Rules", this.package.Definition.SharedRules, item => item.Name, (entity, item) => new Rule(entity, this, item)));
            root.Children.Add(ConfigurationItem.New("Shared Info Groups", this.package.Definition.SharedInformationGroups, item => item.Name));
            root.Children.Add(ConfigurationItem.New("Root Selection Entries", this.package.Definition.EntryLinks, item => item.Name));
            root.Children.Add(ConfigurationItem.New("Root Rules", this.package.Definition.Rules, item => item.Name, (entity, item) => new Rule(entity, this, item)));
        }

        private void RefreshPublications()
        {
            this.Publications.Clear();
            if (this.Definition?.Publications == null) return;

            foreach (var publication in this.Definition.Publications)
            {
                this.Publications.Add(publication);
            }
        }
    }
}