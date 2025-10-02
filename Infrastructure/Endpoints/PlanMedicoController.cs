using Application.Contracts.DTOs.Request;
using Application.Contracts.Interfaces;
using Application.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Endpoints;

[ApiController]
[Route("[controller]")]
public class PlanMedicoController : ControllerBase
{
    private readonly IProjectLogger logger;
    private readonly IPlanMedicoService service;
    public PlanMedicoController(IProjectLogger projectLogger, IPlanMedicoService planMedicoService)
    {
        logger = projectLogger;
        service = planMedicoService;
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllAsync()
    {
        try
        {
            var result = await service.GetAllAsync();
            logger.LogSuccess("Planes Medicos obtenidos exitosamente.");
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError("Error al obtener los Planes Medicos.", ex);
            return StatusCode(500, "Error interno del servidor.");
        }
    }

    [HttpPost("save")]
    public async Task<IActionResult> AddAsync([FromBody] PlanMedicoRequest planMedicoCreateDto, [FromQuery] int id)
    {
        try
        {
            if (id == 0)
            {
                var result = await service.AddAsync(planMedicoCreateDto);
                logger.LogSuccess("Plan Medico agregado exitosamente.");
                return Ok(result);
            }
            else
            {
                var result = await service.UpdateAsync(id, planMedicoCreateDto);
                logger.LogSuccess("Plan Medico actualizado exitosamente.");
                return Ok(result);
            }
        }
        catch (Exception ex)
        {
            logger.LogError("Error al guardar el Plan Medico.", ex);
            return StatusCode(500, "Error interno del servidor.");
        }
    }
    [HttpPatch("toggleStatus/{id}")]
    public async Task<IActionResult> ToggleStatusAsync(int id)
    {
        try
        {
            bool result = await service.ToggleStatusAsync(id);
            logger.LogSuccess("Estado del Plan Medico cambiado exitosamente.");
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError("Error al cambiar el estado del Plan Medico.", ex);
            return StatusCode(500, "Error interno del servidor.");
        }
    }

}
