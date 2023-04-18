using System.Windows.Input;

namespace Wasp.UI.Windows
{
    public static class CustomCommands
    {
        public static readonly RoutedUICommand DeselectUnit = new(
                        "DeselectUnit",
                        "DeselectUnit",
                        typeof(CustomCommands),
                        new InputGestureCollection
                        {
                            new KeyGesture(Key.Left, ModifierKeys.Alt)
                        }
                    );

        public static readonly RoutedUICommand ImportRoster = new(
                        "ImportRoster",
                        "ImportRoster",
                        typeof(CustomCommands),
                        new InputGestureCollection
                        {
                            new KeyGesture(Key.O, ModifierKeys.Control | ModifierKeys.Shift)
                        }
                    );

        public static readonly RoutedUICommand ReportDataSheets = new(
                        "ReportDataSheets",
                        "ReportDataSheets",
                        typeof(CustomCommands),
                        new InputGestureCollection()
                    );

        public static readonly RoutedUICommand ReportOrderOfBattle = new(
                        "ReportOrderOfBattle",
                        "ReportOrderOfBattle",
                        typeof(CustomCommands),
                        new InputGestureCollection()
                    );

        public static readonly RoutedUICommand SelectUnit = new(
                                "SelectUnit",
                        "SelectUnit",
                        typeof(CustomCommands),
                        new InputGestureCollection
                        {
                            new KeyGesture(Key.Right, ModifierKeys.Alt)
                        }
                    );
    }
}