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
        SharedSelectionEntryGroup = 2048,
        SharedProfile = 4096,
        SharedRule = 8192,
        SharedInfoGroup = 16384,

        CatalogueEntries = CatalogueLink | Publication | CostType | ProfileType | CategoryEntry | ForceEntry
            | SelectionEntry | EntryLink | Rule | InfoLink
            | SharedSelectionEntry | SharedSelectionEntryGroup | SharedProfile | SharedRule | SharedInfoGroup,
    }
}