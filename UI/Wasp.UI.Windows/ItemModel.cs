using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace Wasp.UI.Windows
{
    public class ItemModel
    {
        public ItemModel()
        {
            this.Items = new();
            this.ItemsView = (CollectionView)CollectionViewSource.GetDefaultView(this.Items);
            this.ItemsView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
        }

        public ObservableCollection<ItemModel> Items { get; }

        public CollectionView ItemsView { get; }

        public string? Name { get; set; }

        public ItemModel? Parent { get; set; }
    }
}