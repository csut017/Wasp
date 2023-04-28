using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Wasp.Core.Data;

namespace Wasp.UI.DataEditor.DataModels
{
    public class ConfigurationItem
        : INotifyPropertyChanged
    {
        private bool isExpanded;
        private bool isSelected;

        public ConfigurationItem()
        {
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<ConfigurationItem> Children { get; } = new();

        public string? Image { get; set; }

        public bool IsExpanded
        {
            get => this.isExpanded;
            set
            {
                this.isExpanded = value;
                this.NotifyPropertyChanged();
            }
        }

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value;
                this.NotifyPropertyChanged();
            }
        }

        public object? Item { get; set; }

        public string Name { get; set; } = "Unknown item";

        public object? ViewModel { get; set; }

        public static ConfigurationItem New<TItem>(string name, string? image, List<TItem>? items, Func<TItem, string?> generateName, Func<TItem, ConfigurationItem, object>? viewModelGenerator = null)
        {
            var item = new ConfigurationItem
            {
                Image = $"images\\{image ?? "unknown"}.png",
                Name = name,
            };
            if (items != null)
            {
                foreach (var child in items)
                {
                    GenerateChildAndAddToParent(child, item, generateName(child) ?? string.Empty, image, viewModelGenerator);
                }
            }
            return item;
        }

        public ConfigurationItem ChangeImage(string? image)
        {
            this.Image = $"images\\{image ?? "unknown"}.png";
            return this;
        }

        private static void GenerateChildAndAddToParent<TItem>(TItem? child, ConfigurationItem item, string name, string? image, Func<TItem, ConfigurationItem, object>? viewModelGenerator)
        {
            if (child == null) return;

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