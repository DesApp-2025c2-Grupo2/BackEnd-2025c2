using Application.Contracts.DTOs.Internal;
using Application.Contracts.DTOs.Request;
using Application.Contracts.DTOs.Response;
using Application.Utilities;
using Application.Contracts.Interfaces;
using Infrastructure.Persistence.Configurations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Endpoints;


[ApiController]
[Route("[controller]")]
public class PrestadorController : ControllerBase
{
	private readonly IProjectLogger logger;
	private readonly IPrestadorService prestadorService;
	public PrestadorController(IProjectLogger logger, IPrestadorService prestadorService)
	{
		this.logger = logger;
		this.prestadorService = prestadorService;
	}

	[HttpGet("all")]
	public async Task<IActionResult> GetAllAsync()
	{
        ActionResult result;
        try
        {
            var prestadores = await prestadorService.GetAllAsync();
            result = Ok(prestadores);
        }
        catch (Exception ex)
        {
            logger.LogError("Error al obtener los Prestadores.", ex);
            Console.WriteLine(ex.StackTrace);
            result = StatusCode(500, "Error interno del servidor.");
        }
        return result;
    }

    [HttpPut("toggleStatus/{id}")]
    public async Task<IActionResult> ToggleStatusAsync([FromRoute] int id)
    {
        ActionResult result;
        try
        {
            bool prestadorStatus = await prestadorService.ToggleStatusAsync(id);
            result = Ok(prestadorStatus);
        }
        catch (KeyNotFoundException knfEx)
        {
            logger.LogWarning($"Prestador con ID {id} no encontrado.\n{knfEx.Message}");
            result = NotFound($"Prestador con ID {id} no encontrado.");
        }
        catch (Exception ex)
        {
            logger.LogError($"Error al cambiar el estado del Prestador con ID {id}.", ex);
            result = StatusCode(500, "Error interno del servidor.");
        }
        return result;
    }
    [HttpPost("saveNew")]
    public async Task<IActionResult> SaveAsync([FromBody] PrestadorRequest request)
    {
        ActionResult result;
        try
        {
            if (request.Id.HasValue)
            {
                logger.LogWarning("El ID no debe ser proporcionado al crear un nuevo Prestador.");
                result = BadRequest("El ID no debe ser proporcionado al crear un nuevo Prestador.");
            }
            else
            {
                PrestadorResponse prestador = await prestadorService.SaveAsync(request);
                result = Ok(prestador);
            }
        }
        catch (Exception ex)
        {
            logger.LogError("Error al guardar el Prestador.", ex);
            result = StatusCode(500, "Error interno del servidor.");
        }
        return result;
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateAsync([FromBody] PrestadorRequest request)
    {
        ActionResult result;
        try
        {
            if (!request.Id.HasValue)
            {
                logger.LogWarning("El ID es requerido al actualizar un Prestador.");
                result = BadRequest("El ID es requerido al actualizar un Prestador.");
            }
            else
            {
                PrestadorResponse prestador = await prestadorService.SaveAsync(request);
                result = Ok(prestador);
            }
        }
        catch (KeyNotFoundException knfEx)
        {
            logger.LogWarning($"Prestador con ID {request.Id} no encontrado.\n{knfEx.Message}");
            result = NotFound($"Prestador con ID {request.Id} no encontrado.");
        }
        catch (Exception ex)
        {
            logger.LogError("Error al actualizar el Prestador.", ex);
            result = StatusCode(500, "Error interno del servidor.");
        }
        return result;
    }

    [HttpPut("agendas/update")]
    public async Task<IActionResult> UpdateAgendaAsync([FromBody] AgendaRequest request)
    {
        ActionResult result;
        try
        {
            if (request.HorariosAtencion == null || !request.HorariosAtencion.Any())
            {
                logger.LogWarning("La lista de Horarios de Atención no puede estar vacía.");
                result = BadRequest("La lista de Horarios de Atención no puede estar vacía.");
            }
            else
            {
                AgendaResponse agenda = await prestadorService.UpdateAgendaAsync(request);
                result = Ok(agenda);
            }
        }
        catch (KeyNotFoundException knfEx)
        {
            logger.LogWarning($"Agenda con ID {request.Id} no encontrada.\n{knfEx.Message}");
            result = NotFound($"Agenda con ID {request.Id} no encontrada.");
        }
        catch (Exception ex)
        {
            logger.LogError("Error al actualizar la Agenda.", ex);
            result = StatusCode(500, "Error interno del servidor.");
        }
        return result;
    }

}
