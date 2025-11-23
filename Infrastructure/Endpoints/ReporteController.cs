using Application.Contracts.DTOs.Request;
using Application.Contracts.DTOs.Response;
using Application.Contracts.Interfaces;
using Application.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Endpoints;


[ApiController]
[Route("[controller]")]
public class ReporteController : ControllerBase
{
    private readonly IProjectLogger logger;
    private readonly IReporteService service;

    public ReporteController(IProjectLogger projectLogger, IReporteService reporteService)
    {
        logger = projectLogger;
        service = reporteService;
    }
    [HttpGet("all")]
    public async Task<IActionResult> GetAllAsync()
    {
        ActionResult result;
        try
        {
            ReportesResponse response = await service.GetAllAsync();
            result = Ok(response);
        }
        catch (Exception ex)
        {
            logger.LogError("Error al obtener todos los reportes.", ex);
            result = StatusCode(500, "Error interno del servidor.");
        }
        return result;
    }

    [HttpGet("retrieve")]
    public async Task<IActionResult> RetrieveAsync([FromQuery][Required] string hexaId, [FromQuery][Required] int tipo)
    {
        ActionResult result;
        try
        {
            var reporteData = await service.RetrieveAsync(hexaId,tipo);
            if (reporteData == null || reporteData.Length == 0)
            {
                result = NotFound("Reporte no encontrado.");
            }
            else
            {
                result = File(reporteData, "application/pdf", $"reporte_{hexaId}.pdf");
            }
        }
        catch (Exception ex)
        {
            logger.LogError($"Error al recuperar el reporte con ID {hexaId}.", ex);
            result = StatusCode(500, "Error interno del servidor.");
        }
        return result;
    }

    [HttpPost("generate")]
    public async Task<IActionResult> GenerateAsync([FromBody] ReporteRequest reporteRequest)
    {
        ActionResult result;
        try
        {
            (string,byte[]) reporteData = await service.GenerateAsync(reporteRequest);
            result = File(reporteData.Item2, "application/pdf", $"reporte_{reporteData.Item1}.pdf");
        }
        catch (Exception ex)
        {
            logger.LogError("Error al generar el reporte.", ex);
            result = StatusCode(500, "Error interno del servidor.");
        }
        return result;
    }
}
