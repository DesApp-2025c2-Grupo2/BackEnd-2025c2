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
    public ActionResult<SituacionesTerapeuticasResponse> GetAll()
    {
        ActionResult<SituacionesTerapeuticasResponse> response = NotFound("No implementado");
        try
        {
            SituacionesTerapeuticasResponse data = service.GetAll();
            if (data == null || data.Count == 0)
            {
                response = NoContent();
            }
            else
            {
                response = Ok(data);
            }
        }
        catch (Exception ex)
        {
            logger.LogError("Error al obtener todas las situaciones terapeuticas", ex);
            response = StatusCode(500, "Error interno del servidor");
        }
        return response;
    }

    // Baja y Reactivacion
    [HttpPost("toggleStatus")]
    public ActionResult<SituacionTerapeuticaResponse> ToggleStatus([FromQuery] int id)
    {
        ActionResult<SituacionTerapeuticaResponse> response = NotFound("No implementado");
        try
        {
            SituacionTerapeuticaResponse? data = service.ToggleStatus(id);
            if (data == null)
            {
                response = NotFound($"No se encontró la situación terapéutica con ID {id}");
            }
            else
            {
                response = Ok(data);
            }
        }
        catch (Exception ex)
        {
            logger.LogError($"Error al cambiar el estado de la situación terapéutica con ID {id}", ex);
            response = StatusCode(500, "Error interno del servidor");
        }
        return response;
    }

    // Alta y Modificacion
    [HttpPost("save")]
    public ActionResult<SituacionTerapeuticaResponse> Save([FromBody] SituacionTerapeuticaRequest request, [FromQuery] int id = 0)
    {
        ActionResult<SituacionTerapeuticaResponse> response = NotFound("No implementado");
        try
        {
            SituacionTerapeuticaResponse? data;
            if (id > 0)
            {
                data = service.Update(id, request);
                if (data == null)
                {
                    response = NotFound($"No se encontró la situación terapéutica con ID {id}");
                }
                else
                {
                    response = Ok(data);
                }
            }
            else
            {
                data = service.Add(request);
                if (data == null)
                {
                    response = BadRequest("No se pudo crear la nueva situación terapéutica");
                }
                else
                {
                    response = CreatedAtAction(nameof(Save), new { id = data.id }, data);
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError($"Error al guardar la situación terapéutica con ID {id}", ex);
            response = StatusCode(500, "Error interno del servidor");
        }
        return response;
    }
}
