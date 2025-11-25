using QuestPDF.Elements;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Infrastructure.Adapters.PDFGenerator;

public static class PDFStyles
{
    public static TextStyle Title => TextStyle.Default.FontSize(24).Bold();
    public static TextStyle Subtitle => TextStyle.Default.FontSize(18).SemiBold();
    public static TextStyle Header1 => TextStyle.Default.FontSize(16).Bold();
    public static TextStyle Header2 => TextStyle.Default.FontSize(14).SemiBold();
    public static TextStyle Header3 => TextStyle.Default.FontSize(12).SemiBold();
    public static TextStyle NormalText => TextStyle.Default.FontSize(10);
    public static TextStyle TableHeaderText => TextStyle.Default.FontSize(10).Bold();
    public static TextStyle TableText => TextStyle.Default.FontSize(10);
    public static TextStyle SpacedText => TextStyle.Default.LineHeight(1.6f);
}
