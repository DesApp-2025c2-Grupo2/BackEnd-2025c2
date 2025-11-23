using Application.Contracts.DTOs.Request;
using Application.Contracts.DTOs.Response;

namespace Application.Contracts.Interfaces;

public interface IReporteService
{
    /// <summary>
    /// Devuelve todos los reportes generados alguna vez.
    /// </summary>
    /// <returns></returns>
    Task<ReportesResponse> GetAllAsync();
    /// <summary>
    /// Devuelve un reporte del historial.
    /// </summary>
    /// <param name="hexaId"></param>
    /// <param name="tipo"></param>
    Task<byte[]> RetrieveAsync(string hexaId, int tipo);
    /// <summary>
    /// Genera un reporte.
    /// </summary>
    /// <param name="reporteRequest"></param>
    /// <returns></returns>
    Task<(string, byte[])> GenerateAsync(ReporteRequest reporteRequest);
}
