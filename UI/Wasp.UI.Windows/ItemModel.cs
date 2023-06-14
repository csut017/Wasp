using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;

namespace Wasp.UI.Windows
{
    public class ItemModel
        : INotifyPropertyChanged
    {
        public ItemModel(string id)
        {
            this.Items = new();
            this.ItemsView = (CollectionView)CollectionViewSource.GetDefaultView(this.Items);
            this.ItemsView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            Id = id;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public ItemModel? ArmyRoster { get; set; }

        public string Id { get; set; }

        public ObservableCollection<ItemModel> Items { get; }

        public CollectionView ItemsView { get; }

        public string? Name { get; set; }

        public ItemModel? SourceRoster { get; set; }

        protected void NotifyPropertyChanged([CallerMemberName] string caller = "")
        {
            if (string.IsNullOrEmpty(caller)) return;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }
    }
}