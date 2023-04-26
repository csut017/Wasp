using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

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
                    item.Children.Add(new ConfigurationItem
                    {
                        Name = generateName(child) ?? "<Unknown>",
                        Item = child,
                    });
                }
            }
            return item;
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}