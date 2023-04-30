using System.Windows.Input;

namespace Wasp.UI.DataEditor.ViewModels
{
    public class DynamicMenuItem
    {
        public DynamicMenuItem(string name, ICommand command, string? image = null, bool startNewGroup = false)
        {
            Name = name;
            Command = command;
            Image = image;
            StartNewGroup = startNewGroup;
        }

        public ICommand Command { get; }

        public string? Image { get; }

        public string Name { get; }

        public bool StartNewGroup { get; }
    }
}