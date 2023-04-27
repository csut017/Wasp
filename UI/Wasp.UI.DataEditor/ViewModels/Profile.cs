using System.Collections.ObjectModel;
using System.Linq;
using Wasp.UI.DataEditor.DataModels;
using Data = Wasp.Core.Data;

namespace Wasp.UI.DataEditor.ViewModels
{
    public class Profile
        : ViewModel
    {
        public Profile(Data.Profile definition, Main main, ConfigurationItem item)
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
                MarkAsDirty(GenerateUndoAction("Change profile comment", () => Definition.Comment = oldValue, () => Definition.Comment = value));
            }
        }

        public Data.Profile Definition { get; private set; }

        public string? Id
        {
            get => Definition.Id;
            set
            {
                var oldValue = Definition.Id;
                Definition.Id = value;
                NotifyPropertyChanged();
                MarkAsDirty(GenerateUndoAction("Change profile id", () => Definition.Id = oldValue, () => Definition.Id = value));
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
                MarkAsDirty(GenerateUndoAction("Change whether profile is hidden", () => Definition.IsHidden = oldValue, () => Definition.IsHidden = value));
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
                MarkAsDirty(GenerateUndoAction("Change profile full name", () => Definition.Name = oldValue, () => Definition.Name = value));
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
                MarkAsDirty(GenerateUndoAction("Change profile page reference", () => Definition.Page = oldValue, () => Definition.Page = value));
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
                MarkAsDirty(GenerateUndoAction("Change profile publication reference", () => Definition.PublicationId = oldValue, () => Definition.PublicationId = newValue));
            }
        }
    }
}