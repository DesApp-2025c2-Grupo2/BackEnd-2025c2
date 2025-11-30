using Application.Contracts.DTOs.Request;
using Application.Contracts.DTOs.Response;
using Application.Contracts.Interfaces;
using Application.Utilities;
using Domain.DataModels;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Ports;
using Domain.Interfaces.Repositories;
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
        string hexaId;
        DataTable reporteData;
        (hexaId,reporteData) = await repository.GenerateAsync(reporte);

        if (reporteData == null) throw new Exception("Error al generar los datos del reporte.");

        //hexaId = hexaId.PadLeft(8, '0');

        //Segun el tipo de reporte genero el PDF correspondiente
        byte[] pdfBytes = GetReport(reporte.Tipo, hexaId, reporteData);

        return (hexaId, pdfBytes);
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
                Parametros = $"Desde: {rep.FechaDesde?.ToString("yyyy-MM-dd") ?? "N/A"}, Hasta: {rep.FechaHasta?.ToString("yyyy-MM-dd") ?? "N/A"}",
                FechaGeneracion = rep.FechaGeneracion
            });
        });
        return response;
    }

    public async Task<(string, byte[])> RetrieveAsync(string hexaId, int tipo)
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
        DataTable reporteData;
        (hexaId, reporteData) = await repository.RetrieveAsync(hexaId);

        //Segun el tipo de reporte genero el PDF correspondiente
        byte[] pdfBytes = GetReport(tipoReporte, hexaId, reporteData);
        return (hexaId, pdfBytes);

    }

    private byte[] GetReport(TipoReporte tipo, string hexaID,DataTable dataTable)
    {
        byte[] pdfBytes;
        pdfBytes = tipo switch
        {
            TipoReporte.AltaAfiliadosPorPeriodo => pdfGenerator.GenerateReportPDF(
                hexaID,
                DTOMapper.ToReportDataList<AltaAfiliadosPorPeriodoReportDataRow>(dataTable)
            ),
            TipoReporte.AltaPrestadoresPorPeriodo => pdfGenerator.GenerateReportPDF(
                hexaID,
                DTOMapper.ToReportDataList<AltaPrestadoresPorPeriodoReportDataRow>(dataTable)
            ),
            TipoReporte.PrestadoresPorEspecialidadYCodigoPostal => pdfGenerator.GenerateReportPDF(
                hexaID,
                DTOMapper.ToReportDataList<PrestadoresPorEspecialidadYCodigoPostalReportDataRow>(dataTable)
            ),
            TipoReporte.SituacionesTerapeuticasPorAfiliado => pdfGenerator.GenerateReportPDF(
                hexaID,
                DTOMapper.ToReportDataList<SituacionesTerapeuticasPorAfiliadoReportDataRow>(dataTable)
            ),
            TipoReporte.PrestadoresSinAgendas => pdfGenerator.GenerateReportPDF(
                hexaID,
                DTOMapper.ToReportDataList<PrestadoresSinAgendasReportDataRow>(dataTable)
            ),
            _ => throw new NotImplementedException("Tipo de reporte no soportado")
        };
        return pdfBytes;
    }
}
