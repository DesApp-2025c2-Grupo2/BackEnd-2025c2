using Application.Contracts.DTOs.Response;
using Application.Contracts.Interfaces;
using Application.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Endpoints;

[ApiController]
[Route("[controller]")]
public class ReporteController : ControllerBase
{
    private readonly IProjectLogger logger;
    private readonly IReporteService reporteService;
    public ReporteController(IProjectLogger logger, IReporteService reporteService)
    {
        this.logger = logger;
        this.reporteService = reporteService;
    }

    /*
    [HttpGet("generate/download")]
    public async Task<IActionResult> GetReporteEjemploDownload()
    {
        try
        {
            logger.LogInformation("Iniciando la generación del reporte de ejemplo.");
            var reporteBytes = await reporteService.GenerarReporteEjemplo();
            logger.LogSuccess("Reporte generado exitosamente.");
            return File(reporteBytes, "application/pdf", "reporte_ejemplo.pdf");
        }
        catch (Exception ex)
        {
            logger.LogError("Error al generar el reporte de ejemplo.", ex);
            return StatusCode(500, "Ocurrió un error al generar el reporte.");
        }
    }

    [HttpGet("generate/view")]
    public async Task<IActionResult> GetReporteEjemploView()
    {
        try
        {
            logger.LogInformation("Iniciando la generación del reporte de ejemplo.");
            var reporteBytes = await reporteService.GenerarReporteEjemplo();
            logger.LogSuccess("Reporte generado exitosamente.");
            return File(reporteBytes, "application/pdf");
        }
        catch (Exception ex)
        {
            logger.LogError("Error al generar el reporte de ejemplo.", ex);
            return StatusCode(500, "Ocurrió un error al generar el reporte.");
        }
    }

    */

    /*
    endpoint van a ser 3.
    - all: devuelve todos los registros historicos de reportes generados alguna vez
    - retrieve: devuelve un reporte del historial. (requiere id)
    - generate: genera un reporte.
    */

    [HttpGet("all")]
    public async Task<IActionResult> GetAllReportes()
    {
        ActionResult result;
        try
        {
            logger.LogInformation("Iniciando la obtención de todos los reportes.");
            // Lógica para obtener todos los reportes (a implementar)
            ReportesResponse reportes = new ReportesResponse
            {
                new ReporteResponse
                {
                    HexaID = "1A2B3C",
                    TipoReporte = "VentasMensuales",
                    Parametros = "{ 'mes': '2024-05' }",
                    FechaGeneracion = DateTime.Now.Date.AddDays(-10)
                },
                new ReporteResponse
                {
                    HexaID = "4D5E6F",
                    TipoReporte = "AfiliadosActivos",
                    Parametros = "{ 'anio': '2024' }",
                    FechaGeneracion = DateTime.Now.Date.AddDays(-5)
                }
            };
            logger.LogSuccess("Reportes obtenidos exitosamente.");
            result = Ok(reportes);
        }
        catch (Exception ex)
        {
            logger.LogError("Error al obtener los reportes.", ex);
            result = StatusCode(500, "Ocurrió un error al obtener los reportes.");
        }
        return result;
    }

}
