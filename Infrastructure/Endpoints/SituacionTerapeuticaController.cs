using Application.Contracts.DTOs.Request;
using Application.Contracts.DTOs.Response;
using Application.Contracts.Interfaces;
using Application.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Endpoints;


[ApiController]
[Route("sterapeuticas")]
public class SituacionTerapeuticaController : ControllerBase
{
    private readonly IProjectLogger logger;
    private readonly ISituacionTerapeuticaService service;
    public SituacionTerapeuticaController(IProjectLogger logger, ISituacionTerapeuticaService service)
    {
        this.logger = logger;
        this.service = service;
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var result = await service.GetAllAsync();
            logger.LogSuccess("Especialidades obtenidas exitosamente.");
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError("Error al obtener las Especialidades.", ex);
            return StatusCode(500, "Error interno del servidor.");
        }
    }

    [HttpPost("toggleStatus/{id}")]
    public async Task<IActionResult> ToggleStatus(int id)
    {
        try
        {
            var result = await service.ToggleStatusAsync(id);
            logger.LogSuccess("Estado de la Situación Terapéutica cambiado exitosamente.");
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError("Error al cambiar el estado de la Situación Terapéutica.", ex);
            return StatusCode(500, "Error interno del servidor.");
        }
    }

    [HttpPost("save")]
    public async Task<IActionResult> Save([FromBody] SituacionTerapeuticaRequest request, [FromQuery] int id = 0)
    {
        try
        {
            if (id == 0)
            {
                var result = await service.AddAsync(request);
                logger.LogSuccess("Situación Terapéutica agregada exitosamente.");
                return Ok(result);
            }
            else
            {
                var result = await service.UpdateAsync(id, request);
                logger.LogSuccess("Situación Terapéutica actualizada exitosamente.");
                return Ok(result);
            }
        }
        catch (Exception ex)
        {
            logger.LogError("Error al guardar la Situación Terapéutica.", ex);
            return StatusCode(500, "Error interno del servidor.");
        }
    }
}
