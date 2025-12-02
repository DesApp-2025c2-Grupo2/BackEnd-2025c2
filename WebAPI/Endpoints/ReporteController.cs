using Application.Contracts.DTOs.Request;
using Application.Contracts.DTOs.Response;
using Application.Contracts.Interfaces;
using Application.Utilities;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Endpoints;


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
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            response.ForEach(rep => rep.FileURL = $"{baseUrl}/{rep.FileURL}");
            result = Ok(response);
        }
        catch (Exception ex)
        {
            logger.LogError("Error al obtener todos los reportes.", ex);
            result = StatusCode(500, "Error interno del servidor.");
        }
        return result;
    }

    [HttpGet("regenerate")]
    public async Task<IActionResult> RegenerateAsync([FromQuery][Required] string hexaId, [FromQuery][Required] int tipo)
    {
        ActionResult result;
        try
        {
            var relativePath = await service.RegenerateAsync(hexaId,tipo);
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var fullUrl = $"{baseUrl}/{relativePath}";
            result = Ok(new { FileURL = fullUrl });
        }
        catch (Exception ex)
        {
            logger.LogError("Error al regenerar el reporte.", ex);
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
            var relativePath = await service.GenerateAsync(reporteRequest);
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var fullUrl = $"{baseUrl}/{relativePath}";
            result = Ok(new { FileURL = fullUrl });
        }
        catch (Exception ex)
        {
            logger.LogError("Error al generar el reporte.", ex);
            result = StatusCode(500, "Error interno del servidor.");
        }
        return result;
    }
}
