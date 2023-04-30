using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Wasp.UI.DataEditor.ViewModels
{
    public class DynamicMenuItem
    {
        public DynamicMenuItem(string name, ICommand command, string? image = null, bool startNewGroup = false)
        {
            Name = name;
            Command = command;
            ImageName = image;
            StartNewGroup = startNewGroup;
        }

        public ICommand Command { get; }

        public Image Image
        {
            get
            {
                var uri = new Uri($"pack://application:,,,/images/{ImageName ?? "unknown"}.png");
                var image = new Image
                {
                    Height = 16,
                    Width = 16,
                    Source = new BitmapImage(uri),
                    Stretch = System.Windows.Media.Stretch.Fill,
                };
                return image;
            }
        }

        public string? ImageName { get; }

        public string Name { get; }

        public bool StartNewGroup { get; }
    }
}