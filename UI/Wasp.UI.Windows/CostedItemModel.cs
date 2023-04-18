namespace Wasp.UI.Windows
{
    internal class CostedItemModel
        : ItemModel
    {
        public CostedItemModel(string id)
            : base(id)

        {
        }

        public double Cost { get; set; }
    }
}