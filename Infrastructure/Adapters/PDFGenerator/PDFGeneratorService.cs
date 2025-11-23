using Application.Contracts.ExternalServicesInterfaces;
using Infrastructure.Adapters.PDFGenerator.Documents;
using Microsoft.AspNetCore.Hosting;
using QuestPDF.Fluent;
using System.Data;

namespace Infrastructure.Adapters.PDFGenerator;

public class PDFGeneratorService : IPDFGeneratorService
{
    private readonly string logoPath;

    public PDFGeneratorService(IWebHostEnvironment env)
    {
        logoPath = Path.Combine(Path.Combine(env.WebRootPath, "assets"), "AesMedLogo.png");
    }
    public byte[] GenerateAltaAfiliadosAsync((string, DataTable) dataTable)
    {
        //var doc = new AltaAfiliadosDocument(dataTable.Item2, logoPath, dataTable.Item1);
        //return doc.GeneratePdf();
        throw new NotImplementedException();
    }

    public byte[] GenerateAltaPrestadoresAsync((string, DataTable) dataTable)
    {
        //var doc = new AltaPrestadoresDocument(dataTable.Item2, logoPath, dataTable.Item1);
        //return doc.GeneratePdf();
        throw new NotImplementedException();
    }

    public byte[] GeneratePrestadoresPorEspecialidadYCodigoPostalAsync((string, DataTable) dataTable)
    {
        //var doc = new PrestadoresPorEspecialidadYCodigoPostalDocument(dataTable.Item2, logoPath, dataTable.Item1);
        //return doc.GeneratePdf();
        throw new NotImplementedException();
    }

    public byte[] GeneratePrestadoresSinAgendasAsync((string, DataTable) dataTable)
    {
        //var doc = new PrestadoresSinAgendasDocument(dataTable.Item2, logoPath, dataTable.Item1);
        //return doc.GeneratePdf();
        throw new NotImplementedException();
    }

    public byte[] GenerateSituacionesTerapeuticasPorAfiliadoAsync((string, DataTable) dataTable)
    {
        //var doc = new SituacionesTerapeuticasPorAfiliadoDocument(dataTable.Item2, logoPath, dataTable.Item1);
        //return doc.GeneratePdf();
        throw new NotImplementedException();
    }
}
