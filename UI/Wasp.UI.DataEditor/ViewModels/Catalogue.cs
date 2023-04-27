using Wasp.UI.DataEditor.DataModels;
using Data = Wasp.Core.Data;

namespace Wasp.UI.DataEditor.ViewModels
{
    public class Catalogue
        : ViewModel
    {
        public Catalogue(Data.GameSystemConfiguration definition, Main main, ConfigurationItem item)
            : base(main, item)
        {
            Definition = definition;
        }

        public string? AuthorContact
        {
            get => Definition.AuthorContact;
            set
            {
                var oldValue = Definition.AuthorContact;
                Definition.AuthorContact = value;
                NotifyPropertyChanged();
                MarkAsDirty(GenerateUndoAction("Change catalogue author contact", () => Definition.AuthorContact = oldValue, () => Definition.AuthorContact = value));
            }
        }

        public string? AuthorName
        {
            get => Definition.AuthorName;
            set
            {
                var oldValue = Definition.AuthorName;
                Definition.AuthorName = value;
                NotifyPropertyChanged();
                MarkAsDirty(GenerateUndoAction("Change catalogue author name", () => Definition.AuthorName = oldValue, () => Definition.AuthorName = value));
            }
        }

        public string? AuthorUrl
        {
            get => Definition.AuthorUrl;
            set
            {
                var oldValue = Definition.AuthorUrl;
                Definition.AuthorUrl = value;
                NotifyPropertyChanged();
                MarkAsDirty(GenerateUndoAction("Change catalogue author URL", () => Definition.AuthorUrl = oldValue, () => Definition.AuthorUrl = value));
            }
        }

        public string? Comment
        {
            get => Definition.Comment;
            set
            {
                var oldValue = Definition.Comment;
                Definition.Comment = value;
                NotifyPropertyChanged();
                MarkAsDirty(GenerateUndoAction("Change catalogue comment", () => Definition.Comment = oldValue, () => Definition.Comment = value));
            }
        }

        public Data.GameSystemConfiguration Definition { get; private set; }

        public string? Id
        {
            get => Definition.Id;
            set
            {
                var oldValue = Definition.Id;
                Definition.Id = value;
                NotifyPropertyChanged();
                MarkAsDirty(GenerateUndoAction("Change catalogue id", () => Definition.Id = oldValue, () => Definition.Id = value));
            }
        }

        public bool? IsLibrary
        {
            get => Definition.IsLibrary;
            set
            {
                var oldValue = Definition.IsLibrary;
                Definition.IsLibrary = value;
                NotifyPropertyChanged();
                MarkAsDirty(GenerateUndoAction("Change whether catalogue is a library", () => Definition.IsLibrary = oldValue, () => Definition.IsLibrary = value));
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
                MarkAsDirty(GenerateUndoAction("Change catalogue name", () => Definition.Name = oldValue, () => Definition.Name = value));
            }
        }

        public string? ReadMe
        {
            get => Definition.ReadMe;
            set
            {
                var oldValue = Definition.ReadMe;
                Definition.ReadMe = value;
                NotifyPropertyChanged();
                MarkAsDirty(GenerateUndoAction("Change catalogue ReadMe", () => Definition.ReadMe = oldValue, () => Definition.ReadMe = value));
            }
        }

        public string? Revision
        {
            get => Definition.Revision;
            set
            {
                var oldValue = Definition.Revision;
                Definition.Revision = value;
                NotifyPropertyChanged();
                MarkAsDirty(GenerateUndoAction("Change catalogue revision", () => Definition.Revision = oldValue, () => Definition.Revision = value));
            }
        }
    }
}