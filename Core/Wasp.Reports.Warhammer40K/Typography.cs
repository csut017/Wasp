using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Wasp.Reports.Warhammer40K
{
    public static class Typography
    {
        public static TextStyle Caption => Normal.FontColor(Colors.White);

        public static TextStyle Header => Normal.Black();

        public static TextStyle Normal => TextStyle.Default.FontFamily("Roboto").FontColor(Colors.Black).FontSize(10);
    }
}