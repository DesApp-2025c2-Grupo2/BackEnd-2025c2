using Application.Contracts.DTOs.Request;
using Application.Contracts.Interfaces;
using Application.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Endpoints;

[ApiController]
[Route("[controller]")]
public class EspecialidadController : ControllerBase
{
    private readonly IProjectLogger logger;
    private readonly IEspecialidadService service;
    public EspecialidadController(IProjectLogger projectLogger, IEspecialidadService especialidadService)
    {
        logger = projectLogger;
        service = especialidadService;
    }
    [HttpGet("all")]
    public async Task<IActionResult> GetAllAsync()
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
    [HttpPost("save")]
    public async Task<IActionResult> AddAsync([FromBody] EspecialidadRequest especialidadRequest, [FromQuery] int id = 0)
    {
        try
        {
            if (id == 0)
            {
                var result = await service.AddAsync(especialidadRequest);
                logger.LogSuccess("Especialidad agregada exitosamente.");
                return Ok(result);
            }
            else
            {
                var result = await service.UpdateAsync(id, especialidadRequest);
                logger.LogSuccess("Especialidad actualizada exitosamente.");
                return Ok(result);
            }
        }
        catch (Exception ex)
        {
            logger.LogError("Error al guardar la Especialidad.", ex);
            return StatusCode(500, "Error interno del servidor.");
        }
    }
    [HttpPatch("toggleStatus/{id}")]
    public async Task<IActionResult> ToggleStatusAsync(int id)
    {
        try
        {
            var result = await service.ToggleStatusAsync(id);
            logger.LogSuccess("Estado de la Especialidad cambiado exitosamente.");
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError("Error al cambiar el estado de la Especialidad.", ex);
            return StatusCode(500, "Error interno del servidor.");
        }
    }
}
