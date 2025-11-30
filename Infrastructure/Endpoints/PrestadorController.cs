using Application.Contracts.DTOs.Internal;
using Application.Contracts.DTOs.Request;
using Application.Contracts.DTOs.Response;
using Application.Utilities;
using Application.Contracts.Interfaces;
using Infrastructure.Persistence.Configurations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Endpoints;


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
        throw new NotImplementedException();
    }

    //[HttpPost]
    //public async Task<IActionResult> CreateAsync([FromBody] PrestadorResponse request)
    //{
    //	var result = await prestadorService.CreateAsync(request);
    //	return Ok(result);
    //}

    //[HttpPut("{id}")]
    //public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] PrestadorResponse request)
    //{
    //	var result = await prestadorService.UpdateAsync(id, request);
    //	return Ok(result);
    //}



    //[HttpPut("{id}/estado")]
    //public async Task<IActionResult> UpdateEstadoAsync([FromRoute] int id, [FromBody] PrestadorEstadoRequest request)
    //{
    //	try
    //	{
    //		var result = await prestadorService.UpdateEstadoAsync(id, request);
    //		return Ok(result);
    //	}
    //	catch (Exception ex)
    //	{
    //		// Reglas de negocio para 409 en el futuro (turnos futuros, etc.)
    //		if (ex.Message.Contains("conflicto", StringComparison.OrdinalIgnoreCase))
    //			return Conflict(new { message = ex.Message });
    //		return NotFound();
    //	}
    //}

    //[HttpGet("getAll")]
    //public async Task<IActionResult> GetAllAsync()
    //{
    //	var result = await prestadorService.GetAllAsync();
    //	return Ok(result);
    //}

    //[HttpGet("{id:int}")]
    //public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    //{
    //	var result = await prestadorService.GetByIdAsync(id);
    //	return Ok(result);
    //}

    //// Endpoint ADMIN para borrar todos los prestadores y sus datos asociados
    //[HttpDelete("admin/clear-prestadores")]
    //public async Task<IActionResult> ClearPrestadoresAsync([FromServices] ProjectContext db)
    //{
    //	// Hijos directos de agendas/horarios
    //	await db.HorariosAtencion.ExecuteDeleteAsync();
    //	await db.Agendas.ExecuteDeleteAsync();

    //	// Tabla intermedia muchos-a-muchos Prestador-Especialidad
    //	await db.Database.ExecuteSqlRawAsync("DELETE FROM ESPECIALIZACIONES");

    //	// Datos de contacto/documentación asociados a prestadores (no a personas)
    //	await db.Telefonos.Where(t => t.PrestadorId != null).ExecuteDeleteAsync();
    //	await db.Emails.Where(e => e.PrestadorId != null).ExecuteDeleteAsync();
    //	await db.Documentaciones.Where(d => d.PrestadorId != null).ExecuteDeleteAsync();
    //	await db.Direcciones.Where(d => d.PrestadorId != null).ExecuteDeleteAsync();

    //	// Finalmente, todos los prestadores
    //	await db.Prestadores.ExecuteDeleteAsync();

    //	return Ok(new { message = "Prestadores y datos asociados borrados correctamente." });
    //}
}
