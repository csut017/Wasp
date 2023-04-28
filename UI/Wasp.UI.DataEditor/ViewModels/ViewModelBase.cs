using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Wasp.UI.DataEditor.ViewModels
{
    public abstract class ViewModelBase
        : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}