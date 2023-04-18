using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Wasp.Core.Data;

namespace Wasp.Reports.Warhammer40K
{
    public class DataSheets
        : IRosterDocument
    {
        private Roster roster = new();

        public void Compose(IDocumentContainer container)
        {
            container
                .Page(page =>
                {
                    page.DefaultTextStyle(Typography.Normal);

                    page.Margin(1, Unit.Centimetre);
                    page.Size(PageSizes.A4);

                    page.Content().Element(ComposeContent);
                    page.Footer().Element(ReportHelpers.ComposeFooter);
                });
        }

        public DocumentMetadata GetMetadata()
        {
            return new DocumentMetadata()
            {
                Title = roster.Name
            };
        }

        public void Initialise(Roster roster)
        {
            this.roster = roster;
        }

        private void ComposeContent(IContainer container)
        {
            if (roster?.Forces == null) return;
            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text("TODO");
                });
            });
        }
    }
}