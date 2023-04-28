using Wasp.UI.DataEditor.DataModels;
using Data = Wasp.Core.Data;

namespace Wasp.UI.DataEditor.ViewModels
{
    public class Publication
        : ViewModel
    {
        public Publication(Data.Publication definition, Main main, ConfigurationItem item)
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
                MarkAsDirty(GenerateUndoAction("Change publication comment", () => Definition.Comment = oldValue, () => Definition.Comment = value));
            }
        }

        public Data.Publication Definition { get; private set; }

        public string? Id
        {
            get => Definition.Id;
            set
            {
                var oldValue = Definition.Id;
                Definition.Id = value;
                NotifyPropertyChanged();
                MarkAsDirty(GenerateUndoAction("Change publication id", () => Definition.Id = oldValue, () => Definition.Id = value));
            }
        }

        public string? Name
        {
            get => Definition.FullName;
            set
            {
                var oldValue = Definition.FullName;
                Definition.FullName = value;
                NotifyPropertyChanged();
                MarkAsDirty(GenerateUndoAction("Change publication full name", () => Definition.FullName = oldValue, () => Definition.FullName = value));
            }
        }

        public string? PublicationDate
        {
            get => Definition.PublicationDate;
            set
            {
                var oldValue = Definition.PublicationDate;
                Definition.PublicationDate = value;
                NotifyPropertyChanged();
                MarkAsDirty(GenerateUndoAction("Change catalogue publication date", () => Definition.PublicationDate = oldValue, () => Definition.PublicationDate = value));
            }
        }

        public string? PublisherName
        {
            get => Definition.PublisherName;
            set
            {
                var oldValue = Definition.PublisherName;
                Definition.PublisherName = value;
                NotifyPropertyChanged();
                MarkAsDirty(GenerateUndoAction("Change publisher name", () => Definition.PublisherName = oldValue, () => Definition.PublisherName = value));
            }
        }

        public string? PublisherUrl
        {
            get => Definition.PublisherUrl;
            set
            {
                var oldValue = Definition.PublisherUrl;
                Definition.PublisherUrl = value;
                NotifyPropertyChanged();
                MarkAsDirty(GenerateUndoAction("Change publisher URL", () => Definition.PublisherUrl = oldValue, () => Definition.PublisherUrl = value));
            }
        }

        public string? ShortName
        {
            get => Definition.ShortName;
            set
            {
                var oldValue = Definition.ShortName;
                Definition.ShortName = value;
                NotifyPropertyChanged();
                MarkAsDirty(GenerateUndoAction("Change publication short name", () => Definition.ShortName = oldValue, () => Definition.ShortName = value));
            }
        }

        protected override void UpdateId(string newId)
        {
            this.Id = newId;
        }
    }
}