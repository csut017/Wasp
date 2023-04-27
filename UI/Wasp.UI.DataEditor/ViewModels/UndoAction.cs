using System;

namespace Wasp.UI.DataEditor.ViewModels
{
    public class UndoAction
    {
        public UndoAction(string name, Action onUndo, Action onRedo, Action onChange, object viewModel)
        {
            Name = name;
            OnUndo = onUndo;
            OnRedo = onRedo;
            OnChange = onChange;
            ViewModel = viewModel;
        }

        public string Name { get; }

        public Action OnChange { get; }

        public Action OnRedo { get; }

        public Action OnUndo { get; }

        public object ViewModel { get; }
    }
}