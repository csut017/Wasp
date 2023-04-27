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

        public Data.Publication Definition { get; private set; }
    }
}