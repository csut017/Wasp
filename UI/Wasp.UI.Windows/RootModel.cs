using QuestPDF.Fluent;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Data;
using Wasp.Core.Data;
using Wasp.Reports.Warhammer40K;

namespace Wasp.UI.Windows
{
    internal class RootModel
        : INotifyPropertyChanged
    {
        private readonly Dictionary<string, ItemModel> itemMappings = new();
        private readonly Dictionary<string, Selection> selectionMappings = new();
        private readonly List<Roster> sourceRosters = new();
        private ItemModel? currentArmy;
        private Roster orderOfBattle = new();
        private RosterPackage? package;

        public RootModel()
        {
            this.SelectedUnits = new();
            this.SelectedUnitsView = (CollectionView)CollectionViewSource.GetDefaultView(this.SelectedUnits);
            this.SelectedUnitsView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public double AvailablePoints { get; set; }

        public ObservableCollection<ItemModel> AvailableUnits { get; } = new();

        public ItemModel? CurrentArmy
        {
            get => currentArmy;
            set
            {
                if (ReferenceEquals(currentArmy, value)) return;
                currentArmy = value;
                NotifyPropertyChanged();
            }
        }

        public string? FilePath { get; set; }

        public ObservableCollection<ItemModel> SelectedUnits { get; }

        public CollectionView SelectedUnitsView { get; }

        public void AddArmy(string name)
        {
            CurrentArmy = new CostedItemModel(Guid.NewGuid().ToString()) { Name = name };
            SelectedUnits.Add(CurrentArmy);
        }

        public void Deselect(ItemModel item)
        {
            if (item.ArmyRoster is null)
            {
                foreach (var unit in item.Items.ToArray())
                {
                    RemoveItemFromArmy(unit);
                }

                SelectedUnits.Remove(item);
            }
            else
            {
                RemoveItemFromArmy(item);
            }

            CalculatePoints();
        }

        public void GenerateReport<TReport>(string fileName)
            where TReport : IRosterDocument, new()
        {
            var generator = new TReport();
            generator.Initialise(GenerateRoster());
            using var stream = File.Create(fileName);
            generator.GeneratePdf(stream);
            Process.Start("explorer.exe", fileName);
        }

        public async Task ImportAsync(string path)
        {
            this.package = await RosterPackage.LoadAsync(path);
            if (package.Roster == null) return;

            this.sourceRosters.Add(package.Roster);
            this.Refresh(package.Roster);
        }

        public void New()
        {
            this.ClearAll();
            this.Refresh();
            this.orderOfBattle = new();
            this.package = null;
        }

        public async Task OpenAsync(string path)
        {
            this.FilePath = path;
            this.package = await RosterPackage.LoadAsync(path);
            this.orderOfBattle = package.Roster ?? new();
            this.sourceRosters.Add(this.orderOfBattle);
            this.ClearAll();
            this.Refresh();

            if (this.orderOfBattle.Forces == null) return;
            foreach (var unit in this.orderOfBattle.Forces.SelectMany(f => f.Selections ?? new List<Selection>()).Where(s => !string.IsNullOrEmpty(s.Id)))
            {
                if (this.itemMappings.TryGetValue(unit.Id!, out var item))
                {
                    Select(item);
                }
            }
        }

        public async Task SaveAsync(string? filePath = null)
        {
            this.FilePath = filePath ?? this.FilePath;
            if (string.IsNullOrEmpty(this.FilePath)) throw new ArgumentNullException(nameof(filePath));
            if (this.package == null)
            {
                package = RosterPackage.New();
                package.Settings.Name = "data";
            }

            package.Settings.IsCompressed = Path.GetExtension(FilePath) == ".rosz";
            Roster roster = GenerateRoster();
            package.Roster = roster;
            await package.SaveAsync(FilePath);
        }

        public void Select(ItemModel item)
        {
            if (CurrentArmy is null) AddArmy("New Army");
            (item.SourceRoster?.Items ?? this.AvailableUnits).Remove(item);
            CurrentArmy!.Items.Add(item);
            item.ArmyRoster = CurrentArmy;
            CalculatePoints();
        }

        internal void SelectArmy(ItemModel? newItem)
        {
            if (newItem?.ArmyRoster is null)
            {
                CurrentArmy = newItem;
            }
            else
            {
                SelectArmy(newItem?.ArmyRoster);
            }
        }

        protected void NotifyPropertyChanged([CallerMemberName] string caller = "")
        {
            if (string.IsNullOrEmpty(caller)) return;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
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

        private void CalculatePoints()
        {
            this.AvailablePoints = this.AvailableUnits.Sum(CalculateTotalCost);
            foreach (var item in SelectedUnits.Cast<CostedItemModel>())
            {
                if (item is null) continue;
                item.Cost = 0;
                item.Cost = CalculateTotalCost(item);
            }

            var maxArmyCost = SelectedUnits.Max(a => (a as CostedItemModel)?.Cost ?? 0);
            foreach (var item in SelectedUnits.Cast<CostedItemModel>())
            {
                if (item is null) continue;
                item.CostDifference = item.Cost - maxArmyCost;
            }

            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AvailablePoints)));
        }

        private void ClearAll()
        {
            this.AvailableUnits.Clear();
            this.SelectedUnits.Clear();
        }

        private Roster GenerateRoster()
        {
            var force = new Force
            {
                Name = "Order of Battle",
                Selections = new List<Selection>(),
            };
            var roster = new Roster()
            {
                Forces = new List<Force> { force },
            };

            foreach (var unit in SelectedUnits)
            {
                if (selectionMappings.TryGetValue(unit.Id!, out var selection))
                {
                    force.Selections.Add(selection);
                }
            }

            return roster;
        }

        private void Refresh(Roster? rosterToLoad = null)
        {
            var loadList = new List<Force>();
            if (rosterToLoad == null)
            {
                loadList.AddRange(sourceRosters.SelectMany(r => r.Forces ?? new List<Force>()));
                this.itemMappings.Clear();
                this.selectionMappings.Clear();
            }
            else
            {
                loadList.AddRange(rosterToLoad.Forces ?? new List<Force>());
            }

            foreach (var force in loadList)
            {
                var forceItem = new ItemModel(force.Id ?? string.Empty) { Name = force.CatalogueName };
                if (force.Selections != null)
                {
                    foreach (var selection in force.Selections.Where(s => s.Type == "model" || s.Type == "unit"))
                    {
                        var cost = CalculateTotalCost(selection);
                        var id = selection.Id ?? string.Empty;
                        if (this.itemMappings.ContainsKey(id)) continue;

                        var unitItem = new CostedItemModel(id)
                        {
                            Cost = cost,
                            Name = selection.Name + (string.IsNullOrEmpty(selection.CustomName) ? string.Empty : $" [{selection.CustomName}]"),
                            SourceRoster = forceItem,
                        };
                        forceItem.Items.Add(unitItem);
                        if (!string.IsNullOrEmpty(selection.Id))
                        {
                            this.itemMappings.Add(unitItem.Id, unitItem);
                            this.selectionMappings.Add(selection.Id, selection);
                        }
                    }
                }
                this.AvailableUnits.Add(forceItem);
            }

            this.CalculatePoints();
        }

        private void RemoveItemFromArmy(ItemModel item)
        {
            (item.SourceRoster?.Items ?? this.AvailableUnits).Add(item);
            item.ArmyRoster?.Items.Remove(item);
            item.ArmyRoster = null;
        }
    }
}