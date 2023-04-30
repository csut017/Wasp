using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Wasp.Core.Data;
using Wasp.UI.DataEditor.ViewModels;

namespace Wasp.UI.DataEditor.DataModels
{
    public class ConfigurationItem
        : INotifyPropertyChanged
    {
        private bool isExpanded;
        private bool isSelected;

        private ConfigurationItem()
        {
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public AddableEntryType AddableEntryTypes { get; set; }

        public ObservableCollection<ConfigurationItem> Children { get; } = new();

        public string? Image { get; set; }

        public bool IsEditable { get => !this.IsImported; }

        public bool IsExpanded
        {
            get => this.isExpanded;
            set
            {
                if (this.isExpanded == value) return;
                this.isExpanded = value;
                this.NotifyPropertyChanged();
            }
        }

        public bool IsImported { get; private set; }

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (isSelected == value) return;
                isSelected = value;
                this.NotifyPropertyChanged();
            }
        }

        public object? Item { get; set; }

        public string Name { get; set; } = "Unknown item";

        public object? ViewModel { get; set; }

        public static ConfigurationItem New(bool isImported, string name, string? image)
        {
            var item = new ConfigurationItem
            {
                Image = $"images\\{image ?? "unknown"}.png",
                IsImported = isImported,
                Name = name,
            };
            return item;
        }

        public ConfigurationItem AllowEntryTypes(AddableEntryType addableEntryTypes)
        {
            AddableEntryTypes = addableEntryTypes;
            return this;
        }

        public bool IsEntryTypeAddable(AddableEntryType entryType)
        {
            return (AddableEntryTypes & entryType) == entryType;
        }

        public ConfigurationItem PopulateChildren<TItem>(bool isImported, string? image, List<TItem>? items, Func<TItem, string?> generateName, Func<TItem, ConfigurationItem, object>? viewModelGenerator = null)
        {
            if (items == null) return this;

            foreach (var child in items)
            {
                var newItem = GenerateChildAndAddToParent(child, this, generateName(child) ?? string.Empty, image, viewModelGenerator);
                if (newItem != null) newItem.IsImported = isImported;
            }
            return this;
        }

        private static ConfigurationItem? GenerateChildAndAddToParent<TItem>(TItem? child, ConfigurationItem item, string name, string? image, Func<TItem, ConfigurationItem, object>? viewModelGenerator)
        {
            if (child == null) return null;

            var newItem = new ConfigurationItem
            {
                Image = $"images\\{image ?? "unknown"}.png",
                Item = child,
                Name = name,
            };
            if (viewModelGenerator != null) newItem.ViewModel = viewModelGenerator.Invoke(child, newItem);
            item.Children.Add(newItem);

            HandleConfigurableItem(child as IConfigurableEntry, newItem);
            HandleLinkedEntryItem(child as ILinkedEntry, newItem);
            return newItem;
        }

        private static void HandleConfigurableItem(IConfigurableEntry? item, ConfigurationItem parent)
        {
            if (item is null) return;

            var indexedFields = new Dictionary<string, string>();
            if (item.Constraints != null)
            {
                foreach (var constraint in item.Constraints)
                {
                    GenerateChildAndAddToParent(constraint, parent, constraint.DisplayName, null, null);
                    if (constraint.Id != null) indexedFields.Add(constraint.Id, constraint.DisplayName);
                }
            }

            if (item.Modifiers != null)
            {
                foreach (var modifier in item.Modifiers)
                {
                    var name = indexedFields.TryGetValue(modifier.Field ?? string.Empty, out var fieldName)
                        ? $"Set [{fieldName}] to {modifier.Value}"
                        : $"Set to {modifier.Value}";
                    GenerateChildAndAddToParent(modifier, parent, name, null, null);
                }
            }

            if (item.InformationLinks != null)
            {
                foreach (var link in item.InformationLinks)
                {
                    GenerateChildAndAddToParent(link, parent, link.Name ?? string.Empty, null, null);
                }
            }
        }

        private static void HandleLinkedEntryItem(ILinkedEntry? item, ConfigurationItem parent)
        {
            if (item is null) return;

            if (item.EntryLinks != null)
            {
                foreach (var link in item.EntryLinks)
                {
                    GenerateChildAndAddToParent(link, parent, link.Name ?? string.Empty, null, null);
                }
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}