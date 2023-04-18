using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Wasp.Reports.Warhammer40K
{
    internal static class ReportHelpers
    {
        public static void ComposeFooter(IContainer container)
        {
            container.PaddingBottom(-10).Row(row =>
            {
                row.RelativeItem().Text($"Generated {DateTime.Now:D}");
                row.RelativeItem().AlignRight().Text(x =>
                {
                    x.Span("Page ");
                    x.CurrentPageNumber();
                });
            });
        }
    }
}