using System;
using System.Windows.Input;

namespace Wasp.UI.DataEditor.ViewModels
{
    internal class SimpleCommand : ICommand
    {
        private readonly Action<object?> onExecute;
        private bool isExecutable;

        public SimpleCommand(Action<object?> onExecute, bool isExecutable = true)
        {
            this.onExecute = onExecute;
            IsExecutable = isExecutable;
        }

        public event EventHandler? CanExecuteChanged;

        public bool IsExecutable
        {
            get => isExecutable;
            set
            {
                if (isExecutable == value) return;
                isExecutable = value;
                this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public bool CanExecute(object? parameter)
        {
            return this.IsExecutable;
        }

        public void Execute(object? parameter)
        {
            this.onExecute(parameter);
        }
    }
}