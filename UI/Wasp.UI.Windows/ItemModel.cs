using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace Wasp.UI.Windows
{
    public class ItemModel
    {
        public ItemModel(string id)
        {
            this.Items = new();
            this.ItemsView = (CollectionView)CollectionViewSource.GetDefaultView(this.Items);
            this.ItemsView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            Id = id;
        }

        public string Id { get; set; }

        public ObservableCollection<ItemModel> Items { get; }

        public CollectionView ItemsView { get; }

        public string? Name { get; set; }

        public ItemModel? Parent { get; set; }
    }
}