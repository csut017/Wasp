using QuestPDF.Infrastructure;
using Wasp.Core.Data;

namespace Wasp.Reports.Warhammer40K
{
    public interface IRosterDocument
        : IDocument
    {
        void Initialise(Roster roster);
    }
}