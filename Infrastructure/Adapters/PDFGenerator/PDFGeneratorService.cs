using Application.Contracts.DTOs.Internal.ReportData;
using Application.Contracts.ExternalServicesInterfaces;
using Infrastructure.Adapters.PDFGenerator.Documents;
using Microsoft.AspNetCore.Hosting;
using QuestPDF.Fluent;

namespace Infrastructure.Adapters.PDFGenerator;

public class PDFGeneratorService : IPDFGeneratorService
{
    private readonly string logoPath;

    public PDFGeneratorService(IWebHostEnvironment env)
    {
        logoPath = Path.Combine(Path.Combine(env.WebRootPath, "assets"), "AesMedLogo.png");
    }

    public byte[] GenerateReportPDF<T>(string reportCode, ReportDataList<T> data) where T : ReportDataRow
    {
        ReportDocument<T> document = ReportDocumentRegistry.ResolveFor<T>(data, logoPath, reportCode);
        return document.GeneratePdf();
    }
}
