using Domain.DataModels;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Infrastructure.Adapters.PDFGenerator.Documents;

public abstract class ReportDocument<TRow> : IDocument where TRow : ReportDataRow
{
    protected readonly ReportDataList<TRow> data;
    protected readonly string logoPath;
    protected readonly string title;
    protected readonly string reportID;

    public ReportDocument(ReportDataList<TRow> data, string logoPath, string title, string reportID)
    {
        this.data = data;
        this.logoPath = logoPath;
        this.title = title;
        this.reportID = reportID;
    }

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.MarginTop(25);
            page.MarginBottom(20);
            page.MarginLeft(20);
            page.MarginRight(20);
            page.Header().Element(ComposeHeader);
            page.Content().Element(ComposeContent);
            page.Footer().Element(ComposeFooter);
        });
    }

    // ---------------------
    // Header
    // ---------------------
    protected void ComposeHeader(IContainer container)
    {
        container.Row(row =>
        {
            // Intentar cargar logo
            if (!string.IsNullOrWhiteSpace(logoPath) && File.Exists(logoPath))
            {
                row.RelativeItem(1).Height(50).AlignLeft().AlignMiddle().Image(logoPath).FitHeight();
            }
            else
            {
                // Espacio reservado para no romper el layout
                row.RelativeItem(1).Height(50).AlignLeft().AlignMiddle().Text("AESMED Medicina Integral");
            }
            row.RelativeItem(4).AlignLeft().AlignMiddle().Row(subRow =>
            {
                subRow.RelativeItem(1).Column(col =>
                {
                    col.Item().Text($"Reporte de {title}").Style(PDFStyles.Header1);
                    col.Item().Text($"ID: #{reportID}").Style(PDFStyles.Header3);
                });
            });
        });
    }

    // ---------------------
    // Content
    // ---------------------
    protected virtual void ComposeContent(IContainer container)
    {
        container.PaddingTop(10).Column(col =>
        {
            col.Spacing(10);
            // Si ReportDataList está vacío
            if (data.Count == 0) col.Item().Text("No hay información disponible.").Style(PDFStyles.Header1);
            
            return;
        });
    }

    // ---------------------
    // Footer
    // ---------------------
    protected void ComposeFooter(IContainer container)
    {
        container.AlignCenter().Text(text =>
        {
            text.Span("Página ").Style(PDFStyles.NormalText);
            text.CurrentPageNumber().Style(PDFStyles.NormalText);
            text.Span(" de ").Style(PDFStyles.NormalText);
            text.TotalPages().Style(PDFStyles.NormalText);
        });
    }
}
