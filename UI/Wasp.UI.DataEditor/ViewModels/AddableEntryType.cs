using System;

namespace Wasp.UI.DataEditor.ViewModels
{
    [Flags]
    public enum AddableEntryType
    {
        Unknown = 0,

        CatalogueLink = 1,
        Publication = 2,
        CostType = 4,
        ProfileType = 8,
        CategoryEntry = 16,
        ForceEntry = 32,
        SelectionEntry = 64,
        EntryLink = 128,
        Rule = 256,
        InfoLink = 512,
        SharedSelectionEntry = 1024,

        CatalogueEntries = CatalogueLink + Publication + CostType + ProfileType + CategoryEntry + ForceEntry + SelectionEntry + EntryLink + Rule + InfoLink + SharedSelectionEntry,
    }
}