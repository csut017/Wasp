using System.IO;
using System.Windows;

namespace Wasp.UI.DataEditor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void OnStartUp(object sender, StartupEventArgs e)
        {
            var main = new MainWindow();
            if (e.Args.Length > 0)
            {
                var fileName = e.Args[0];
                if (File.Exists(fileName))
                {
                    main.OpenFile(fileName);
                }
                else
                {
                    MessageBox.Show($"Could not find {fileName}", "Unable to Open File", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            main.Show();
        }
    }
}