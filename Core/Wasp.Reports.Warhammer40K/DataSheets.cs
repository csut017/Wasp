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

        private static void ComposeUnitContent(ColumnDescriptor column, Selection unit)
        {
            // Add the name
            var name = string.IsNullOrEmpty(unit.CustomName)
                ? unit.Name
                : $"{unit.CustomName} [{unit.Name}]";
            column.Item().Row(row => row.RelativeItem().Element(HeaderStyle).Text(name ?? string.Empty));

            // Add the profiles
            column.Item().PaddingBottom(10).Row(row => row.RelativeItem().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(0.9f, Unit.Centimetre);
                    columns.RelativeColumn();
                    columns.ConstantColumn(0.9f, Unit.Centimetre);
                    columns.ConstantColumn(0.9f, Unit.Centimetre);
                    columns.ConstantColumn(0.9f, Unit.Centimetre);
                    columns.ConstantColumn(0.9f, Unit.Centimetre);
                    columns.ConstantColumn(0.9f, Unit.Centimetre);
                    columns.ConstantColumn(1, Unit.Centimetre);
                    columns.ConstantColumn(0.9f, Unit.Centimetre);
                    columns.ConstantColumn(0.9f, Unit.Centimetre);
                    columns.ConstantColumn(1, Unit.Centimetre);
                });

                table.Header(header =>
                {
                    header.Cell().Element(DarkCellStyle).AlignCenter().Text("#");
                    header.Cell().Element(DarkCellStyle).AlignLeft().Text("Unit");
                    header.Cell().Element(DarkCellStyle).AlignCenter().Text("M");
                    header.Cell().Element(DarkCellStyle).AlignCenter().Text("WS");
                    header.Cell().Element(DarkCellStyle).AlignCenter().Text("BS");
                    header.Cell().Element(DarkCellStyle).AlignCenter().Text("S");
                    header.Cell().Element(DarkCellStyle).AlignCenter().Text("T");
                    header.Cell().Element(DarkCellStyle).AlignCenter().Text("W");
                    header.Cell().Element(DarkCellStyle).AlignCenter().Text("A");
                    header.Cell().Element(DarkCellStyle).AlignCenter().Text("Ld");
                    header.Cell().Element(DarkCellStyle).AlignCenter().Text("Save");
                });

                var profiles = ExtractUniqueProfiles(unit, "Unit", new[] { "unit", "model" });
                var isOdd = true;
                foreach (var profile in profiles)
                {
                    var number = profile.Number.ToString();
                    foreach (var unitProfile in profile.Profiles)
                    {
                        Func<IContainer, IContainer> style = isOdd ? LightCellStyleOdd : LightCellStyleEven;
                        isOdd = !isOdd;

                        table.Cell().Element(style).AlignCenter().Text(number);
                        number = string.Empty;

                        table.Cell().Element(style).AlignLeft().Text(unitProfile.Name);
                        var characteristics = unitProfile.Characteristics;
                        if (characteristics == null) continue;
                        foreach (var characteristic in characteristics)
                        {
                            table.Cell().Element(style).AlignCenter().Text(characteristic.Value);
                        }
                    }
                }
            }));

            // Add the weapons
            column.Item().PaddingBottom(10).Row(row => row.RelativeItem().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.ConstantColumn(2, Unit.Centimetre);
                    columns.ConstantColumn(3, Unit.Centimetre);
                    columns.ConstantColumn(0.9f, Unit.Centimetre);
                    columns.ConstantColumn(0.9f, Unit.Centimetre);
                    columns.ConstantColumn(0.9f, Unit.Centimetre);
                    columns.ConstantColumn(5.6f, Unit.Centimetre);
                });

                table.Header(header =>
                {
                    header.Cell().Element(DarkCellStyle).AlignLeft().Text("Weapon");
                    header.Cell().Element(DarkCellStyle).AlignCenter().Text("Range");
                    header.Cell().Element(DarkCellStyle).AlignLeft().Text("Type");
                    header.Cell().Element(DarkCellStyle).AlignCenter().Text("S");
                    header.Cell().Element(DarkCellStyle).AlignCenter().Text("AP");
                    header.Cell().Element(DarkCellStyle).AlignCenter().Text("D");
                    header.Cell().Element(DarkCellStyle).AlignLeft().Text("Abilities");
                });

                var profiles = ExtractUniqueProfiles(unit, "Weapon", new[] { "upgrade", "model" });
                var isOdd = true;
                foreach (var profile in profiles)
                {
                    var column = 0;
                    foreach (var unitProfile in profile.Profiles)
                    {
                        Func<IContainer, IContainer> style = isOdd ? LightCellStyleOdd : LightCellStyleEven;
                        isOdd = !isOdd;

                        table.Cell().Element(style).AlignLeft().Text(profile.Name);
                        var characteristics = unitProfile.Characteristics;
                        if (characteristics == null) continue;
                        foreach (var characteristic in characteristics)
                        {
                            if (column++ is 1 or 5)
                            {
                                table.Cell().Element(style).Text(characteristic.Value);
                            }
                            else
                            {
                                table.Cell().Element(style).AlignCenter().Text(characteristic.Value);
                            }
                        }
                    }
                }
            }));

            // Add the abilities
            column.Item().PaddingBottom(10).Row(row => row.RelativeItem().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                });

                table.Header(header =>
                {
                    header.Cell().Element(DarkCellStyle).AlignLeft().Text("Abilities");
                });

                var profiles = ExtractUniqueProfiles(unit, "Abilities", new[] { "model", "unit" });
                var isOdd = true;
                foreach (var profile in profiles)
                {
                    foreach (var unitProfile in profile.Profiles)
                    {
                        Func<IContainer, IContainer> style = isOdd ? LightCellStyleOdd : LightCellStyleEven;
                        isOdd = !isOdd;

                        var characteristics = unitProfile.Characteristics;
                        if (characteristics == null) continue;
                        foreach (var characteristic in characteristics)
                        {
                            table.Cell().Element(style).Text(text =>
                            {
                                if (profile.Name != unit.Name)
                                {
                                    text.Span($"[{profile.Name}] {unitProfile.Name}: ").Bold();
                                }
                                else
                                {
                                    text.Span(unitProfile.Name + ": ").Bold();
                                }
                                text.Span(characteristic.Value);
                            });
                        }
                    }
                }

                var rules = ExtractRules(unit, new[] { "upgrade" });
                foreach (var rule in rules)
                {
                    Func<IContainer, IContainer> style = isOdd ? LightCellStyleOdd : LightCellStyleEven;
                    isOdd = !isOdd;

                    foreach (var ruleDetails in rule.Rules)
                    {
                        table.Cell().Element(style).Text(text =>
                        {
                            text.Span(ruleDetails.Name).Bold();
                        });
                    }
                }
            }));
        }

        private static IContainer DarkCellStyle(IContainer container)
        {
            return container
                .Background(Colors.Black)
                .DefaultTextStyle(Typography.Caption)
                .Padding(3);
        }

        private static List<UnitProfile> ExtractProfiles(Selection unit, string profileType, string[] selectionTypes)
        {
            var profiles = new List<UnitProfile>();
            var profile = new UnitProfile
            {
                Name = unit.Name ?? string.Empty,
            };

            if (int.TryParse(unit.Number, out var number)) profile.Number = number;
            if (unit.Profiles != null) profile.Profiles.AddRange(unit.Profiles.Where(p => p.TypeName == profileType));
            profile.GenerateHash();
            profiles.Add(profile);

            if (unit.Selections == null) return profiles;
            foreach (var subUnit in unit.Selections.Where(s => selectionTypes.Contains(s.Type)))
            {
                profiles.AddRange(ExtractProfiles(subUnit, profileType, selectionTypes));
            }

            return profiles;
        }

        private static List<UnitRule> ExtractRules(Selection unit, string[] selectionTypes)
        {
            var rules = new List<UnitRule>();
            var rule = new UnitRule
            {
                Name = unit.Name ?? string.Empty,
            };

            if (unit.Rules != null) rule.Rules.AddRange(unit.Rules);
            rules.Add(rule);

            if (unit.Selections == null) return rules;
            foreach (var subUnit in unit.Selections.Where(s => selectionTypes.Contains(s.Type)))
            {
                rules.AddRange(ExtractRules(subUnit, selectionTypes));
            }

            return rules;
        }

        private static List<UnitProfile> ExtractUniqueProfiles(Selection unit, string profileType, string[] selectionTypes)
        {
            var profiles = ExtractProfiles(unit, profileType, selectionTypes);
            var uniqueProfiles = new Dictionary<string, UnitProfile>();
            foreach (var profile in profiles)
            {
                if (uniqueProfiles.TryGetValue(profile.Hash, out var unitProfile))
                {
                    unitProfile.Number += profile.Number;
                    profile.Number = 0;
                }
                else
                {
                    uniqueProfiles.Add(profile.Hash, profile);
                }

                if (!profile.Profiles.Any()) profile.Number = 0;
            }

            return profiles.Where(p => !string.IsNullOrEmpty(p.Hash) && (p.Number > 0)).ToList();
        }

        private static IContainer HeaderStyle(IContainer container)
        {
            return container
                .DefaultTextStyle(Typography.Header.FontSize(12))
                .PaddingBottom(6);
        }

        private static IContainer LightCellStyleEven(IContainer container)
        {
            return container
                .Background("#dddddd")
                .DefaultTextStyle(Typography.Normal)
                .PaddingHorizontal(5)
                .PaddingVertical(3);
        }

        private static IContainer LightCellStyleOdd(IContainer container)
        {
            return container
                .Background(Colors.White)
                .DefaultTextStyle(Typography.Normal)
                .PaddingHorizontal(5)
                .PaddingVertical(3);
        }

        private void ComposeContent(IContainer container)
        {
            if (roster?.Forces == null) return;

            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    foreach (var force in roster.Forces)
                    {
                        if (force.Selections == null) continue;
                        foreach (var unit in force.Selections.Where(s => s.Type == "model" || s.Type == "unit"))
                        {
                            ComposeUnitContent(column, unit);
                        }
                    }
                });
            });
        }
    }
}