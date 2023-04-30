using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Wasp.UI.DataEditor.DataModels;

using Data = Wasp.Core.Data;

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
            dynamicMenuItems[AddableEntryType.CategoryEntry] = new DynamicMenuItem("Category Entry", new SimpleCommand(AddCategoryEntry), "catalogue_entry");
            dynamicMenuItems[AddableEntryType.ForceEntry] = new DynamicMenuItem("Force Entry", new SimpleCommand(AddForceEntry), "force_entry");
            dynamicMenuItems[AddableEntryType.SelectionEntry] = new DynamicMenuItem("Selection Entry", new SimpleCommand(AddSelectionEntry), "selection_entry", true);
            dynamicMenuItems[AddableEntryType.EntryLink] = new DynamicMenuItem("Entry Entry", new SimpleCommand(AddEntryLink), "entry_link");
            dynamicMenuItems[AddableEntryType.Rule] = new DynamicMenuItem("Rule", new SimpleCommand(AddRule), "rule");
            dynamicMenuItems[AddableEntryType.InfoLink] = new DynamicMenuItem("Info Link", new SimpleCommand(AddInfoLink), "info_link");
            dynamicMenuItems[AddableEntryType.SharedSelectionEntry] = new DynamicMenuItem("Shared Selection Entry", new SimpleCommand(AddSharedSelectionEntry), "shared_selection_entry", true);
            dynamicMenuItems[AddableEntryType.SharedSelectionEntryGroup] = new DynamicMenuItem("Shared Selection Entry Group", new SimpleCommand(AddSharedSelectionEntryGroup), "shared_selection_entry_group");
            dynamicMenuItems[AddableEntryType.SharedRule] = new DynamicMenuItem("Shared Rule", new SimpleCommand(AddSharedRule), "shared_rule");
            dynamicMenuItems[AddableEntryType.SharedProfile] = new DynamicMenuItem("Shared Profile", new SimpleCommand(AddSharedProfile), "shared_profile");
            dynamicMenuItems[AddableEntryType.SharedInfoGroup] = new DynamicMenuItem("Shared Info Group", new SimpleCommand(AddSharedInfoGroup), "shared_info");
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

        private void AddCatalogueLink(object? parameter)
        {
            throw new NotImplementedException();
        }

        private void AddCategoryEntry(object? parameter)
        {
            throw new NotImplementedException();
        }

        private void AddCostType(object? parameter)
        {
            var costType = new Data.CostType { Name = "New CostType", Id = Data.NamedEntry.GenerateId() };
            var treeItem = ConfigurationItem.New(false, costType.Name, "cost_type");
            treeItem.Item = costType;
            treeItem.ViewModel = new CostType(costType, main, treeItem);
            main.SelectedItem?.Children.Add(treeItem);
            main.SelectedItem = treeItem;
        }

        private void AddEntryLink(object? parameter)
        {
            throw new NotImplementedException();
        }

        private void AddForceEntry(object? parameter)
        {
            throw new NotImplementedException();
        }

        private void AddInfoLink(object? parameter)
        {
            throw new NotImplementedException();
        }

        private void AddProfileType(object? parameter)
        {
            throw new NotImplementedException();
        }

        private void AddPublication(object? parameter)
        {
            throw new NotImplementedException();
        }

        private void AddRule(object? parameter)
        {
            throw new NotImplementedException();
        }

        private void AddSelectionEntry(object? parameter)
        {
            throw new NotImplementedException();
        }

        private void AddSharedInfoGroup(object? parameter)
        {
            throw new NotImplementedException();
        }

        private void AddSharedProfile(object? parameter)
        {
            throw new NotImplementedException();
        }

        private void AddSharedRule(object? parameter)
        {
            throw new NotImplementedException();
        }

        private void AddSharedSelectionEntry(object? parameter)
        {
            throw new NotImplementedException();
        }

        private void AddSharedSelectionEntryGroup(object? parameter)
        {
            throw new NotImplementedException();
        }
    }
}