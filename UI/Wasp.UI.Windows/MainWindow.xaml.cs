using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Wasp.UI.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly RootModel dataModel = new();

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = dataModel;
        }

        private void OnClose(object sender, ExecutedRoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void OnDeselectUnit(object sender, ExecutedRoutedEventArgs e)
        {
            if (((Button)e.OriginalSource).DataContext is not ItemModel item) return;
            this.dataModel.Deselect(item);
        }

        private async void OnImportRoster(object sender, ExecutedRoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                AddExtension = true,
                CheckFileExists = true,
                DefaultExt = ".rosz",
                Filter = "Roster file (*.rosz;*.ros)|*.rosz;*.ros|All files (*.*)|*.*",
                FilterIndex = 1,
                Multiselect = false,
                Title = "Open Roster",
                ValidateNames = true,
            };
            if (dialog.ShowDialog(this).GetValueOrDefault(false))
            {
                await dataModel.ImportAsync(dialog.FileName);
            }
        }

        private void OnNewOrderOfBattle(object sender, ExecutedRoutedEventArgs e)
        {
            this.dataModel.New();
        }

        private async void OnOpenRoster(object sender, ExecutedRoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                AddExtension = true,
                CheckFileExists = true,
                DefaultExt = ".rosz",
                Filter = "Roster file (*.rosz;*.ros)|*.rosz;*.ros|All files (*.*)|*.*",
                FilterIndex = 1,
                Multiselect = false,
                Title = "Open Roster",
                ValidateNames = true,
            };
            if (dialog.ShowDialog(this).GetValueOrDefault(false))
            {
                await dataModel.OpenAsync(dialog.FileName);
            }
        }

        private async void OnSaveAsRoster(object sender, ExecutedRoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                AddExtension = true,
                CheckPathExists = true,
                DefaultExt = ".rosz",
                Filter = "Compressed roster file (*.rosz)|*.rosz|Uncompressed roster file (*.ros)|*.ros|All files (*.*)|*.*",
                FilterIndex = 1,
                OverwritePrompt = true,
                Title = "Save Roster",
                ValidateNames = true,
            };
            if (dialog.ShowDialog(this).GetValueOrDefault(false))
            {
                await dataModel.SaveAsync(dialog.FileName);
            }
        }

        private async void OnSaveRoster(object sender, ExecutedRoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(dataModel.FilePath))
            {
                this.OnSaveAsRoster(sender, e);
            }
            else
            {
                await dataModel.SaveAsync();
            }
        }

        private void OnSelectUnit(object sender, ExecutedRoutedEventArgs e)
        {
            if (((Button)e.OriginalSource).DataContext is not ItemModel item) return;
            this.dataModel.Select(item);
        }
    }
}