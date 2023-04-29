using System.Collections.ObjectModel;
using System.Linq;
using Wasp.UI.DataEditor.DataModels;
using Data = Wasp.Core.Data;

namespace Wasp.UI.DataEditor.ViewModels
{
    public class InformationGroup
        : ViewModel
    {
        public InformationGroup(Data.InformationGroup definition, Main main, ConfigurationItem item)
            : base(main, item)
        {
            this.Definition = definition;
        }

        public string? Comment
        {
            get => Definition.Comment;
            set
            {
                var oldValue = Definition.Comment;
                Definition.Comment = value;
                NotifyPropertyChanged();
                MarkAsDirty(GenerateUndoAction("Change information group comment", () => Definition.Comment = oldValue, () => Definition.Comment = value));
            }
        }

        public Data.InformationGroup Definition { get; private set; }

        public string? Id
        {
            get => Definition.Id;
            set
            {
                var oldValue = Definition.Id;
                Definition.Id = value;
                NotifyPropertyChanged();
                MarkAsDirty(GenerateUndoAction("Change information group id", () => Definition.Id = oldValue, () => Definition.Id = value));
            }
        }

        public bool? IsHidden
        {
            get => Definition.IsHidden;
            set
            {
                var oldValue = Definition.IsHidden;
                Definition.IsHidden = value;
                NotifyPropertyChanged();
                MarkAsDirty(GenerateUndoAction("Change whether information group is hidden", () => Definition.IsHidden = oldValue, () => Definition.IsHidden = value));
            }
        }

        public string? Name
        {
            get => Definition.Name;
            set
            {
                var oldValue = Definition.Name;
                Definition.Name = value;
                NotifyPropertyChanged();
                MarkAsDirty(GenerateUndoAction("Change information group full name", () => Definition.Name = oldValue, () => Definition.Name = value));
            }
        }

        public string? Page
        {
            get => Definition.Page;
            set
            {
                var oldValue = Definition.Page;
                Definition.Page = value;
                NotifyPropertyChanged();
                MarkAsDirty(GenerateUndoAction("Change information group page reference", () => Definition.Page = oldValue, () => Definition.Page = value));
            }
        }

        public ObservableCollection<Data.Publication> Publications
        {
            get => Main.Publications;
        }

        public Data.Publication? SelectedPublication
        {
            get
            {
                var publication = Main.Publications.FirstOrDefault(p => p.Id == Definition.PublicationId);
                return publication;
            }
            set
            {
                var oldValue = Definition.PublicationId;
                var newValue = value?.Id;
                Definition.PublicationId = newValue;
                NotifyPropertyChanged();
                MarkAsDirty(GenerateUndoAction("Change information group publication reference", () => Definition.PublicationId = oldValue, () => Definition.PublicationId = newValue));
            }
        }

        protected override void UpdateId(string newId)
        {
            this.Id = newId;
        }
    }
}