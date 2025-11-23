using Application.Contracts.DTOs.Request;
using Application.Contracts.DTOs.Response;
using Application.Contracts.ExternalServicesInterfaces;
using Application.Contracts.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using System.Data;

namespace Application.Services;

public class ReporteService : IReporteService
{
    private readonly IReporteRepository repository;
    private readonly IPDFGeneratorService pdfGenerator;
    public ReporteService(IReporteRepository reporteRepository, IPDFGeneratorService pDFGeneratorService)
    {
        repository = reporteRepository;
        pdfGenerator = pDFGeneratorService;
    }

    public async Task<(string, byte[])> GenerateAsync(ReporteRequest reporteRequest)
    {
        Reporte reporte = new Reporte
        {
            Tipo = (TipoReporte)reporteRequest.TipoReporte,
            FechaDesde = reporteRequest.FechaDesde,
            FechaHasta = reporteRequest.FechaHasta,
            AfiliadoId = reporteRequest.AfiliadoId,
            FechaGeneracion = DateTime.Now.Date
        };

        (string,DataTable) reporteData = await repository.GenerateAsync(reporte);

        if (reporteData.Item2 == null) throw new Exception("Error al generar los datos del reporte.");

        //reporteData.Item1 = reporteData.Item1.PadLeft(8, '0');

        byte[] pdfBytes = (TipoReporte)reporteRequest.TipoReporte switch
        {
            TipoReporte.AltaAfiliadosPorPeriodo => pdfGenerator.GenerateAltaAfiliadosAsync(reporteData),
            TipoReporte.AltaPrestadoresPorPeriodo => pdfGenerator.GenerateAltaPrestadoresAsync(reporteData),
            TipoReporte.PrestadoresPorEspecialidadYCodigoPostal => pdfGenerator.GeneratePrestadoresPorEspecialidadYCodigoPostalAsync(reporteData),
            TipoReporte.SituacionesTerapeuticasPorAfiliado => pdfGenerator.GenerateSituacionesTerapeuticasPorAfiliadoAsync(reporteData),
            TipoReporte.PrestadoresSinAgendas => pdfGenerator.GeneratePrestadoresSinAgendasAsync(reporteData),
            _ => throw new NotImplementedException("Tipo de reporte no soportado")
        };

        return (reporteData.Item1, pdfBytes);
    }

    public async Task<ReportesResponse> GetAllAsync()
    {
        ReportesResponse response = new ReportesResponse();

        List<Reporte> reportes =  await repository.GetAllAsync();
        reportes.ForEach(rep =>
        {
            response.Add(new ReporteResponse
            {
                HexaID = rep.CodigoIdentificatorio,
                TipoReporte = rep.Tipo.ToString(),
                Parametros = $"Desde: {rep.FechaDesde?.ToString("yyyy-MM-dd") ?? "N/A"}, Hasta: {rep.FechaHasta?.ToString("yyyy-MM-dd") ?? "N/A"}, AfiliadoId: {rep.AfiliadoId?.ToString() ?? "N/A"}",
                FechaGeneracion = rep.FechaGeneracion
            });
        });
        return response;
    }


    public async Task<byte[]> RetrieveAsync(string hexaId, int tipo)
    {
        TipoReporte tipoReporte;
        try
        {
            tipoReporte = (TipoReporte)tipo;
        }
        catch (Exception)
        {
            throw new ArgumentException("Tipo de reporte inválido.");
        }
        DataTable reporteData = await repository.RetrieveAsync(hexaId);
        byte[] pdfBytes = tipoReporte switch
        {
            TipoReporte.AltaAfiliadosPorPeriodo => pdfGenerator.GenerateAltaAfiliadosAsync((hexaId,reporteData)),
            TipoReporte.AltaPrestadoresPorPeriodo => pdfGenerator.GenerateAltaPrestadoresAsync((hexaId, reporteData)),
            TipoReporte.PrestadoresPorEspecialidadYCodigoPostal => pdfGenerator.GeneratePrestadoresPorEspecialidadYCodigoPostalAsync((hexaId, reporteData)),
            TipoReporte.SituacionesTerapeuticasPorAfiliado => pdfGenerator.GenerateSituacionesTerapeuticasPorAfiliadoAsync((hexaId, reporteData)),
            TipoReporte.PrestadoresSinAgendas => pdfGenerator.GeneratePrestadoresSinAgendasAsync((hexaId, reporteData)),
            _ => throw new NotImplementedException("Tipo de reporte no soportado")
        };
        return pdfBytes;

    }

}
