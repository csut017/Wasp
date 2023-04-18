using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using Wasp.Core.Data;

namespace Wasp.UI.Windows
{
    internal class RootModel
        : INotifyPropertyChanged
    {
        private Package? package;

        public RootModel()
        {
            this.SelectedUnits = new();
            this.SelectedUnitsView = (CollectionView)CollectionViewSource.GetDefaultView(this.SelectedUnits);
            this.SelectedUnitsView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public double AvailablePoints { get; set; }

        public ObservableCollection<ItemModel> AvailableUnits { get; } = new();

        public string? FilePath { get; set; }

        public Roster? Main { get; set; }

        public double SelectedPoints { get; set; }

        public ObservableCollection<ItemModel> SelectedUnits { get; }

        public CollectionView SelectedUnitsView { get; }

        public void CalculatePoints()
        {
            this.AvailablePoints = this.AvailableUnits.Sum(CalculateTotalCost);
            this.SelectedPoints = this.SelectedUnits.Sum(CalculateTotalCost);
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AvailablePoints)));
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedPoints)));
        }

        public void Deselect(ItemModel item)
        {
            (item.Parent?.Items ?? this.AvailableUnits).Add(item);
            this.SelectedUnits.Remove(item);
            this.CalculatePoints();
        }

        public async Task OpenAsync(string path)
        {
            this.FilePath = path;
            this.package = await Package.LoadAsync(path);
            this.Main = package.Roster;
            this.SelectedUnits.Clear();
            this.Refresh();
        }

        public void Refresh()
        {
            this.AvailableUnits.Clear();
            if (Main?.Forces == null) return;
            foreach (var force in Main.Forces)
            {
                var forceItem = new ItemModel { Name = force.CatalogueName };
                if (force.Selections != null)
                {
                    foreach (var selection in force.Selections.Where(s => s.Type == "model" || s.Type == "unit"))
                    {
                        var cost = CalculateTotalCost(selection);
                        forceItem.Items.Add(
                            new CostedItemModel
                            {
                                Cost = cost,
                                Name = selection.Name,
                                Parent = forceItem,
                            });
                    }
                }
                this.AvailableUnits.Add(forceItem);
            }

            this.CalculatePoints();
        }

        public async Task SaveAsync(string? filePath = null)
        {
            this.FilePath = filePath ?? this.FilePath;
            if (string.IsNullOrEmpty(this.FilePath)) throw new ArgumentNullException(nameof(filePath));
            if (this.package != null)
            {
                await this.package.SaveAsync(this.FilePath);
                return;
            }

            package = Package.New();
            package.Settings.IsCompressed = Path.GetExtension(FilePath) == ".rosz";
            package.Settings.Name = Main?.Name ?? "data";

            await package.SaveAsync(FilePath);
        }

        public void Select(ItemModel item)
        {
            (item.Parent?.Items ?? this.AvailableUnits).Remove(item);
            this.SelectedUnits.Add(item);
            this.CalculatePoints();
        }

        private static double CalculateTotalCost(Selection? item)
        {
            var childCosts = 0d;
            if (item?.Selections != null)
            {
                childCosts = item.Selections.Sum(CalculateTotalCost);
            }

            var pointsCost = item?.Costs?.FirstOrDefault(c => c.TypeId == "points");
            if ((pointsCost == null) || !double.TryParse(pointsCost.Value, out var cost)) return childCosts;
            return cost + childCosts;
        }

        private static double CalculateTotalCost(ItemModel? item)
        {
            var childCosts = 0d;
            if (item?.Items != null)
            {
                childCosts = item.Items.Sum(CalculateTotalCost);
            }

            return childCosts + (item is CostedItemModel costedItem ? costedItem.Cost : 0);
        }
    }
}