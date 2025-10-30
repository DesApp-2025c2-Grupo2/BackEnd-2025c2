using Application.Contracts.DTOs.Response;
using Application.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Endpoints;


[ApiController]
[Route("[controller]")]
public class AgendaController : ControllerBase
{
    private readonly IProjectLogger logger;
    public AgendaController(IProjectLogger logger)
    {
        this.logger = logger;
    }
    [HttpGet("getByProfesional")]
    public Task<IActionResult> GetAllByProfesionalAsync([FromQuery] int profesionalId)
    {
        try
        {
            //var result = await service.GetByProfesionalAsync(profesionalId);
            AgendasResponse agendas = new AgendasResponse
            {
                new AgendaResponse
                {
                    Id = 1,
                    EspecialidadId = 101,
                    Direccion = "Calle Falsa 123",
                    DuracionConsulta = 30,
                    HorariosAtencion = new List<HorarioAtencionResponse>
                    {
                        new HorarioAtencionResponse
                        {
                            Id = 1,
                            DiasDeLaSemana = new List<string> { "Lunes", "Miércoles", "Viernes" },
                            HoraInicio = "09:00",
                            HoraFin = "12:00"
                        },
                        new HorarioAtencionResponse
                        {
                            Id = 2,
                            DiasDeLaSemana = new List<string> { "Martes", "Jueves" },
                            HoraInicio = "14:00",
                            HoraFin = "18:00"
                        }
                    }
                },
                new AgendaResponse
                {
                    Id = 2,
                    EspecialidadId = 102,
                    Direccion = "Avenida Siempre Viva 742",
                    DuracionConsulta = 20,
                    HorariosAtencion = new List<HorarioAtencionResponse>
                    {
                        new HorarioAtencionResponse
                        {
                            Id = 3,
                            DiasDeLaSemana = new List<string> { "Lunes", "Martes", "Miércoles" },
                            HoraInicio = "10:00",
                            HoraFin = "13:00"
                        },
                        new HorarioAtencionResponse
                        {
                            Id = 4,
                            DiasDeLaSemana = new List<string> { "Jueves", "Viernes" },
                            HoraInicio = "15:00",
                            HoraFin = "19:00"
                        }
                    }
                }
            };
            logger.LogSuccess("Agenda obtenida exitosamente.");
            return Task.FromResult<IActionResult>(Ok(agendas));
        }
        catch (Exception ex)
        {
            logger.LogError("Error al obtener la Agenda.", ex);
            return Task.FromResult<IActionResult>(StatusCode(500, "Error interno del servidor."));
        }
    }
}
