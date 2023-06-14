namespace Wasp.UI.Windows
{
    internal class CostedItemModel
        : ItemModel
    {
        private double cost;
        private double costDifference;

        public CostedItemModel(string id)
            : base(id)

        {
        }

        public double Cost
        {
            get => cost;
            set
            {
                if (cost == value) return;
                cost = value;
                NotifyPropertyChanged();
            }
        }

        public double CostDifference
        {
            get => costDifference;
            set
            {
                if (costDifference == value) return;
                costDifference = value;
                NotifyPropertyChanged();
            }
        }
    }
}