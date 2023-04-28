using Wasp.UI.DataEditor.DataModels;
using Data = Wasp.Core.Data;

namespace Wasp.UI.DataEditor.ViewModels
{
    public class ProfileType
        : ViewModel
    {
        public ProfileType(Data.ProfileType definition, Main main, ConfigurationItem item)
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
                MarkAsDirty(GenerateUndoAction("Change profile type comment", () => Definition.Comment = oldValue, () => Definition.Comment = value));
            }
        }

        public Data.ProfileType Definition { get; private set; }

        public string? Id
        {
            get => Definition.Id;
            set
            {
                var oldValue = Definition.Id;
                Definition.Id = value;
                NotifyPropertyChanged();
                MarkAsDirty(GenerateUndoAction("Change profile type id", () => Definition.Id = oldValue, () => Definition.Id = value));
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
                MarkAsDirty(GenerateUndoAction("Change profile type full name", () => Definition.Name = oldValue, () => Definition.Name = value));
            }
        }

        protected override void UpdateId(string newId)
        {
            this.Id = newId;
        }
    }
}