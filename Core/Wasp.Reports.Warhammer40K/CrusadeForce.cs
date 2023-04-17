using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Wasp.Core.Data;

namespace Wasp.Reports.Warhammer40K
{
    public class CrusadeForce
        : IDocument
    {
        private readonly Roster roster;

        public CrusadeForce(Roster roster)
        {
            this.roster = roster;
        }

        public void Compose(IDocumentContainer container)
        {
            container
                .Page(page =>
                {
                    page.DefaultTextStyle(Typography.Normal);

                    page.Margin(1, Unit.Centimetre);
                    page.Size(PageSizes.A4);

                    page.Content().Element(ComposeContent);
                    page.Footer().Element(ComposeFooter);
                });
        }

        public DocumentMetadata GetMetadata()
        {
            return new DocumentMetadata()
            {
                Title = roster.Name
            };
        }

        private static void ComposeCrusadeCard(ColumnDescriptor column, Selection unit, ForceDetails details)
        {
            column.Item().PageBreak();
            column.Item().Row(row =>
            {
                row.RelativeItem().PaddingRight(10).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(4, Unit.Centimetre);
                        columns.RelativeColumn();
                    });

                    var role = string.Empty;
                    if (unit.Categories != null)
                    {
                        foreach (var category in unit.Categories)
                        {
                            if ((category.EntryId != null) && details.Categories.TryGetValue(category.EntryId, out var roleType))
                            {
                                role = roleType;
                                break;
                            }
                        }
                    }

                    table.Cell().Element(DarkCellStyle).Text("Unit Name:");
                    table.Cell().Element(LightCellStyle).Text(unit.CustomName ?? unit.Name);
                    table.Cell().Element(DarkCellStyle).Text("Battlefield Role:");
                    table.Cell().Element(LightCellStyle).Text(role);
                    table.Cell().Element(DarkCellStyle).Text("Crusade Faction:");
                    table.Cell().Element(LightCellStyle).Text(details.FactionName);
                    table.Cell().Element(DarkCellStyle).Text("Selectable Keywords:");
                    table.Cell().Element(LightCellStyle).Text(string.Empty);
                });
                row.ConstantItem(7.5f, Unit.Centimetre).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                    });

                    table.Header(header =>
                    {
                        header.Cell().Element(DarkCellStyle).AlignCenter().Text("Power Rating");
                        header.Cell().Element(DarkCellStyle).AlignCenter().Text("Experience Points");
                        header.Cell().Element(DarkCellStyle).AlignCenter().Text("Crusade Points");
                    });

                    table.Cell().Element(LightCellStyle).AlignCenter().Padding(13).Text(string.Empty);
                    table.Cell().Element(LightCellStyle).AlignCenter().Padding(13).Text(string.Empty);
                    table.Cell().Element(LightCellStyle).AlignCenter().Padding(13).Text(string.Empty);
                });
            });

            column.Item().PaddingTop(10).Row(row =>
            {
                row.RelativeItem().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(4.0f, Unit.Centimetre);
                        columns.RelativeColumn();
                        columns.ConstantColumn(7.5f, Unit.Centimetre);
                    });

                    table.Cell().Element(DarkCellStyle).Text("Unit Type:");
                    table.Cell().Element(LightCellStyle).Text(unit.Name);
                    table.Cell().Element(DarkCellStyle).Text("Other Upgrades and Selectable Abilities");
                    table.Cell().Element(DarkCellStyle).Text("Equipment:");
                    table.Cell().Element(LightCellStyle).Text(string.Empty);
                    table.Cell().RowSpan(4).Element(LightCellStyle).Text(string.Empty);
                    table.Cell().Element(DarkCellStyle).Text("Pyschic Powers:");
                    table.Cell().Element(LightCellStyle).Text(string.Empty);
                    table.Cell().Element(DarkCellStyle).Text("Warlord Traits:");
                    table.Cell().Element(LightCellStyle).Text(string.Empty);
                    table.Cell().Element(DarkCellStyle).Text("Relics:");
                    table.Cell().Element(LightCellStyle).Text(string.Empty);
                });
            });
        }

        private static void ComposeOrderHeader(IContainer container, Force force, ForceDetails details)
        {
            container.PaddingBottom(5).Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(3.7f, Unit.Centimetre);
                    columns.RelativeColumn();
                });

                table.Cell().Element(DarkCellStyle).Text("Crusade Force Name:");
                table.Cell().Element(LightCellStyle).Text(force.Name);
                table.Cell().Element(DarkCellStyle).Text("Crusade Faction:");
                table.Cell().Element(LightCellStyle).Text(details.FactionName);
                table.Cell().Element(DarkCellStyle).Text("Player Name:");
                table.Cell().Element(LightCellStyle).Text(string.Empty);
            });
        }

        private static void ComposeOrderNotes(IContainer container, Force force)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                });

                table.Header(header =>
                {
                    header.Cell().Element(DarkCellStyle).Text("Crusade Goals, Information, and Notable Victories");
                });

                table.Cell().Element(LightCellStyle).MinHeight(3.0f, Unit.Centimetre).Text(string.Empty);
            });
        }

        private static void ComposeOrderOfBattle(ColumnDescriptor column, Force force, ForceDetails details)
        {
            column.Item().PaddingBottom(10).Element(container => ComposeOrderHeader(container, force, details));
            column.Item().PaddingBottom(10).Element(container => ComposeOrderTallies(container, force));
            column.Item().PaddingBottom(10).Element(container => ComposeOrderTable(container, force));
            column.Item().Element(container => ComposeOrderNotes(container, force));
        }

        private static void ComposeOrderTable(IContainer container, Force force)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(2.0f, Unit.Centimetre);
                    columns.RelativeColumn();
                    columns.ConstantColumn(3.0f, Unit.Centimetre);
                    columns.ConstantColumn(3.0f, Unit.Centimetre);
                });

                table.Header(header =>
                {
                    header.Cell().ColumnSpan(2).Element(DarkCellStyle).Text("Crusade Cards");
                    header.Cell().Element(DarkCellStyle).AlignCenter().Text("Power Rating");
                    header.Cell().Element(DarkCellStyle).AlignCenter().Text("Crusade Points");
                });

                var count = 0;
                if (force.Selections != null)
                {
                    foreach (var unit in force.Selections.Where(s => s.Type == "model" || s.Type == "unit"))
                    {
                        var powerLevel = 0d;
                        var cost = unit.Costs?.FirstOrDefault(c => c.TypeId == "e356-c769-5920-6e14");
                        if (!string.IsNullOrEmpty(cost?.Value)) powerLevel = double.Parse(cost.Value);

                        count++;
                        table.Cell().Element(LightCellStyle).Element(OrderRowStyle).AlignRight().Text($"Unit {count}:");
                        var name = string.IsNullOrEmpty(unit.CustomName) ? unit.Name : $"{unit.CustomName} [{unit.Name}]";
                        table.Cell().Element(LightCellStyle).Element(OrderRowStyle).Text(name);
                        table.Cell().Element(LightCellStyle).Element(OrderRowStyle).AlignCenter().Text(powerLevel.ToString("0"));
                        table.Cell().Element(LightCellStyle).Element(OrderRowStyle).AlignCenter().Text(string.Empty);
                    }
                }

                for (var loop = count; loop < 20; loop++)
                {
                    table.Cell().Element(LightCellStyle).Element(OrderRowStyle).AlignRight().Text($"Unit {loop + 1}:");
                    table.Cell().Element(LightCellStyle).Element(OrderRowStyle).Text(string.Empty);
                    table.Cell().Element(LightCellStyle).Element(OrderRowStyle).AlignCenter().Text(string.Empty);
                    table.Cell().Element(LightCellStyle).Element(OrderRowStyle).AlignCenter().Text(string.Empty);
                }
            });
        }

        private static void ComposeOrderTallies(IContainer container, Force force)
        {
            var totalSupply = 0.0d;
            if (force.Selections != null)
            {
                foreach (var unit in force.Selections.Where(s => s.Type == "model" || s.Type == "unit"))
                {
                    var powerLevel = 0d;
                    var cost = unit.Costs?.FirstOrDefault(c => c.TypeId == "e356-c769-5920-6e14");
                    if (!string.IsNullOrEmpty(cost?.Value)) powerLevel = double.Parse(cost.Value);

                    totalSupply += powerLevel;
                }
            }

            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                table.Header(header =>
                {
                    header.Cell().Element(DarkCellStyle).AlignCenter().Text("Battle Talley");
                    header.Cell().Element(DarkCellStyle).AlignCenter().Text("Battles Won");
                    header.Cell().Element(DarkCellStyle).AlignCenter().Text("Requisition Points");
                    header.Cell().Element(DarkCellStyle).AlignCenter().Text("Supply Limit");
                    header.Cell().Element(DarkCellStyle).AlignCenter().Text("Supply Used");
                });

                table.Cell().Element(LightCellStyle).Element(TallyRowStyle).Text(string.Empty);
                table.Cell().Element(LightCellStyle).Element(TallyRowStyle).Text(string.Empty);
                table.Cell().Element(LightCellStyle).Element(TallyRowStyle).Text(string.Empty);
                table.Cell().Element(LightCellStyle).Element(TallyRowStyle).Text(string.Empty);
                table.Cell().Element(LightCellStyle).Element(TallyRowStyle).Text(totalSupply.ToString("0"));
            });
        }

        private static IContainer DarkCellStyle(IContainer container)
        {
            return container
                .Background(Colors.Black)
                .DefaultTextStyle(Typography.Caption)
                .Border(0.5f)
                .BorderColor(Colors.Black)
                .Padding(3);
        }

        private static IContainer LightCellStyle(IContainer container)
        {
            return container
                .Background(Colors.White)
                .DefaultTextStyle(Typography.Normal)
                .Border(0.5f)
                .BorderColor(Colors.Black)
                .PaddingHorizontal(5)
                .PaddingVertical(3);
        }

        private static IContainer OrderRowStyle(IContainer container)
        {
            return container
                .AlignMiddle()
                .Height(0.6f, Unit.Centimetre);
        }

        private static IContainer TallyRowStyle(IContainer container)
        {
            return container
                .AlignMiddle()
                .AlignCenter()
                .Height(1, Unit.Centimetre);
        }

        private void ComposeContent(IContainer container)
        {
            if (roster?.Forces == null) return;
            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    foreach (var (force, index) in roster.Forces.Select((item, index) => (item, index)))
                    {
                        if (index > 0) column.Item().PageBreak();

                        var details = new ForceDetails
                        {
                            FactionName = force.CatalogueName ?? string.Empty
                        };
                        if (force.Categories != null)
                        {
                            foreach (var category in force.Categories.Where(c => c.EntryId != null))
                            {
                                details.Categories.Add(category.EntryId!, category.Name ?? string.Empty);
                            }
                        }

                        ComposeOrderOfBattle(column, force, details);

                        if (force.Selections == null) continue;
                        foreach (var unit in force.Selections.Where(s => s.Type == "model" || s.Type == "unit"))
                        {
                            ComposeCrusadeCard(column, unit, details);
                        }
                    }
                });
            });
        }

        private void ComposeFooter(IContainer container)
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

        private struct ForceDetails
        {
            public ForceDetails()
            {
            }

            public Dictionary<string, string> Categories { get; } = new Dictionary<string, string>();

            public string FactionName { get; set; } = string.Empty;
        }
    }
}