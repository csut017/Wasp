using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Wasp.UI.DataEditor.ViewModels
{
    public class AddCommands
    {
        private readonly Dictionary<AddableEntryType, DynamicMenuItem> dynamicMenuItems = new();
        private readonly Main main;

        public AddCommands(Main main)
        {
            this.main = main;
            dynamicMenuItems[AddableEntryType.CatalogueLink] = new DynamicMenuItem("Catalogue Link", new SimpleCommand(AddCatalogueLink), "catalogue_link");
            dynamicMenuItems[AddableEntryType.Publication] = new DynamicMenuItem("Publication", new SimpleCommand(AddPublication), "publication");
            dynamicMenuItems[AddableEntryType.CostType] = new DynamicMenuItem("Cost Type", new SimpleCommand(AddCostType), "cost_type");
            dynamicMenuItems[AddableEntryType.ProfileType] = new DynamicMenuItem("Profile Type", new SimpleCommand(AddProfileType), "profile_type");
            dynamicMenuItems[AddableEntryType.ProfileType] = new DynamicMenuItem("Profile Type", new SimpleCommand(AddProfileType), "profile_type");
            dynamicMenuItems[AddableEntryType.SelectionEntry] = new DynamicMenuItem("Selection Entry", new SimpleCommand(AddSelectionEntry), "selection_entry", true);
        }

        public void PopulateMenuOptions(ObservableCollection<DynamicMenuItem> menuItems)
        {
            menuItems.Clear();
            var item = main?.SelectedItem;
            if (item == null) return;

            foreach (var menuItem in this.dynamicMenuItems)
            {
                if (item.IsEntryTypeAddable(menuItem.Key)) menuItems.Add(menuItem.Value);
            }
        }

        private void AddCatalogueLink(object? obj)
        {
            throw new NotImplementedException();
        }

        private void AddCostType(object? obj)
        {
            throw new NotImplementedException();
        }

        private void AddProfileType(object? obj)
        {
            throw new NotImplementedException();
        }

        private void AddPublication(object? obj)
        {
            throw new NotImplementedException();
        }

        private void AddSelectionEntry(object? obj)
        {
            throw new NotImplementedException();
        }
    }
}