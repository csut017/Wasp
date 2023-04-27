using Microsoft.Win32;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Wasp.UI.DataEditor.ViewModels;

namespace Wasp.UI.DataEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Main mainViewModel = new();

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = mainViewModel;
        }

        public void OpenFile(string fileName)
        {
            var worker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
            };
            worker.DoWork += OnOpenFileDoWork;
            worker.ProgressChanged += OnOpenFileProgressChanged;
            worker.RunWorkerCompleted += OnOpenFileRunWorkerCompleted;
            this.mainViewModel.LoadingVisibility = Visibility.Visible;
            worker.RunWorkerAsync(fileName);
        }

        private void OnCanRedo(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.mainViewModel.CanRedo;
        }

        private void OnCanUndo(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.mainViewModel.CanUndo;
        }

        private void OnCloseApplication(object sender, ExecutedRoutedEventArgs e)
        {
            this.mainViewModel.CloseApplication();
        }

        private void OnItemSelected(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            this.mainViewModel.SelectedItem = (DataModels.ConfigurationItem)e.NewValue;
        }

        private void OnNewFile(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("TODO");
        }

        private void OnOpenFile(object sender, ExecutedRoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                AddExtension = true,
                CheckFileExists = true,
                DefaultExt = ".catz",
                Filter = "Catalogue file (*.catz;*.cat)|*.catz;*.cat|Game definition file (*.gstz;*.gst)|*.gstz;*.gst|All files (*.*)|*.*",
                FilterIndex = 1,
                Multiselect = false,
                Title = "Open File",
                ValidateNames = true,
            };
            this.mainViewModel.LoadingVisibility = Visibility.Visible;
            if (dialog.ShowDialog(this).GetValueOrDefault(false))
            {
                OpenFile(dialog.FileName);
            }else
            {
                this.mainViewModel.LoadingVisibility = Visibility.Collapsed;
            }
        }

        private void OnOpenFileDoWork(object? sender, DoWorkEventArgs e)
        {
            var fileName = e.Argument as string;
            mainViewModel.OpenAsync(fileName!).Wait();
        }

        private void OnOpenFileProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            // TODO
        }

        private void OnOpenFileRunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            this.mainViewModel.LoadingVisibility = Visibility.Collapsed;
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Unable to open file", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            mainViewModel.Refresh();
        }

        private void OnRedo(object sender, ExecutedRoutedEventArgs e)
        {
            this.mainViewModel.Redo();
            e.Handled = true;
        }

        private async void OnSaveFile(object sender, ExecutedRoutedEventArgs e)
        {
            if (mainViewModel.HasFile)
            {
                await mainViewModel.SaveAsync();
            }
            else
            {
                this.OnSaveFileAs(sender, e);
            }
        }

        private async void OnSaveFileAs(object sender, ExecutedRoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                AddExtension = true,
                CheckPathExists = true,
                DefaultExt = this.mainViewModel.DefaultExtensionForSave,
                Filter = this.mainViewModel.FilterForSave + "|All files (*.*)|*.*",
                FilterIndex = 1,
                OverwritePrompt = true,
                Title = "Save Roster",
                ValidateNames = true,
            };
            if (!string.IsNullOrEmpty(mainViewModel.FileName))
            {
                dialog.FileName = Path.GetFileName(mainViewModel.FileName);
                dialog.InitialDirectory = Path.GetDirectoryName(dialog.FileName);
            }
            if (dialog.ShowDialog(this).GetValueOrDefault(false))
            {
                await mainViewModel.SaveAsync(dialog.FileName);
            }
        }

        private void OnUndo(object sender, ExecutedRoutedEventArgs e)
        {
            this.mainViewModel.Undo();
            e.Handled = true;
        }
    }
}