using Application.Contracts.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Application.Services;

public class ReporteService : IReporteService
{
    public Task<byte[]> GenerarReporteEjemplo()
    {
        var documento = new ReporteEjemplo();
        byte[] pdfBytes = documento.GeneratePdf();
        return Task.FromResult(pdfBytes);
    }
}

public class ReporteEjemplo : IDocument
{
    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Margin(40);
            page.Header().Text("Ejemplo de PDF").FontSize(20).Bold();
            page.Content().Text("Hola desde QuestPDF!").FontSize(14);
        });
    }
}
