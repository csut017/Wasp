using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Wasp.Core.Data;

namespace Wasp.UI.DataEditor.DataModels
{
    public class ConfigurationItem
        : INotifyPropertyChanged
    {
        private bool isExpanded;

        public ConfigurationItem()
        {
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<ConfigurationItem> Children { get; } = new();

        public bool IsExpanded
        {
            get => this.isExpanded;
            set
            {
                this.isExpanded = value;
                this.NotifyPropertyChanged(nameof(IsExpanded));
            }
        }

        public object? Item { get; set; }

        public string Name { get; set; } = "Unknown item";

        public static ConfigurationItem New<TItem>(string name, List<TItem>? items, Func<TItem, string?> generateName)
        {
            var item = new ConfigurationItem
            {
                Name = name,
            };
            if (items != null)
            {
                foreach (var child in items)
                {
                    GenerateChildAndAddToParent(child, item, generateName(child) ?? string.Empty);
                }
            }
            return item;
        }

        private static void GenerateChildAndAddToParent<TItem>(TItem? child, ConfigurationItem item, string name)
        {
            if (child == null) return;

            var newItem = new ConfigurationItem
            {
                Name = name,
                Item = child,
            };
            item.Children.Add(newItem);

            HandleConfigurableItem(child as IConfigurableEntry, newItem);
            HandleLinkedEntryItem(child as ILinkedEntry, newItem);
        }

        private static void HandleConfigurableItem(IConfigurableEntry? item, ConfigurationItem parent)
        {
            if (item is null) return;

            var indexedFields = new Dictionary<string, string>();
            if (item.Constraints != null)
            {
                foreach (var constraint in item.Constraints)
                {
                    GenerateChildAndAddToParent(constraint, parent, constraint.DisplayName);
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
                    GenerateChildAndAddToParent(modifier, parent, name);
                }
            }

            if (item.InformationLinks != null)
            {
                foreach (var link in item.InformationLinks)
                {
                    GenerateChildAndAddToParent(link, parent, link.Name ?? string.Empty);
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
                    GenerateChildAndAddToParent(link, parent, link.Name ?? string.Empty);
                }
            }
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}