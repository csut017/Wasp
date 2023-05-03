using System;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Wasp.UI.DataEditor.DataModels;
using Data = Wasp.Core.Data;

namespace Wasp.UI.DataEditor.ViewModels
{
    public abstract class ViewModel
        : ViewModelBase
    {
        private bool isDirty;

        public ViewModel(Main main, ConfigurationItem item)
        {
            this.Main = main;
            Item = item;
            GenerateIdCommand = new SimpleCommand(OnGenerateId);
        }

        public ICommand GenerateIdCommand { get; }

        public bool IsDirty
        {
            get => isDirty;
            private set
            {
                if (isDirty == value) return;
                isDirty = value;
                this.NotifyPropertyChanged();
            }
        }

        public ConfigurationItem Item { get; }

        public Main Main { get; private set; }

        public void MarkAsDirty(UndoAction? undo = null)
        {
            this.IsDirty = true;
            this.Main.MarkAsDirty(undo);
        }

        protected UndoAction? GenerateUndoAction(string name, Action onUndo, Action onRedo, [CallerMemberName] string? propertyName = null)
        {
            return new UndoAction(name, onUndo, onRedo, () => this.NotifyPropertyChanged(propertyName), this);
        }

        protected abstract void UpdateId(string newId);

        private void OnGenerateId(object? obj)
        {
            var id = Data.Entry.GenerateId();
            UpdateId(id);
        }
    }
}