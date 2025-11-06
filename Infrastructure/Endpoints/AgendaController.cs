using Application.Contracts.DTOs.Response;
using Application.Contracts.DTOs.Request;
using Application.Contracts.Interfaces;
using Application.Utilities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Infrastructure.Endpoints;


[ApiController]
[Route("[controller]")]
public class AgendaController : ControllerBase
{
    private readonly IProjectLogger logger;
    private readonly IPrestadorRepository prestadorRepository;
    private readonly IPrestadorService prestadorService;
    public AgendaController(IProjectLogger logger, IPrestadorRepository prestadorRepository, IPrestadorService prestadorService)
    {
        this.logger = logger;
        this.prestadorRepository = prestadorRepository;
        this.prestadorService = prestadorService;
    }
    [HttpGet("getByProfesional")]
    public async Task<IActionResult> GetAllByProfesionalAsync([FromQuery] int profesionalId)
    {
        try
        {
            var agendasDb = await prestadorRepository.GetAgendasByProfesionalAsync(profesionalId);
            var direcciones = new List<object>();
            foreach (var agenda in agendasDb)
            {
                var horarios = await prestadorRepository.GetHorariosByAgendaAsync(agenda.Id);
                var grupos = horarios
                    .GroupBy(h => new { inicio = h.HoraInicio.ToString("HH:mm"), fin = h.HoraFin.ToString("HH:mm"), dur = h.DuracionConsulta })
                    .Select(g => new
                    {
                        id = g.Select(x => x.Id).FirstOrDefault(),
                        diasDeLaSemana = g.Select(x => Enum.GetName(typeof(DiaAtencion), x.DiaDeAtencion) ?? x.DiaDeAtencion.ToString()).Distinct().ToList(),
                        horaInicio = g.Key.inicio,
                        horaFin = g.Key.fin,
                        duracionMinutos = (int?)g.Key.dur,
                        especialidades = g.Select(x => x.EspecialidadId).Where(e => e > 0).Distinct().ToList(),
                        especialidadId = (int?)g.Select(x => x.EspecialidadId).Where(e => e > 0).Distinct().FirstOrDefault()
                    })
                    .ToList();

                direcciones.Add(new
                {
                    id = agenda.Id,
                    direccion = agenda.Direccion,
                    duracionConsulta = (int?)agenda.DuracionConsulta,
                    horariosAtencion = grupos
                });
            }
            logger.LogSuccess("Agenda obtenida exitosamente.");
            return Ok(direcciones);
        }
        catch (Exception ex)
        {
            logger.LogError("Error al obtener la Agenda.", ex);
            return StatusCode(500, "Error interno del servidor.");
        }
    }

    [HttpPut("{profesionalId}/direcciones")]
    public async Task<IActionResult> ReplaceDireccionesAsync([FromRoute] int profesionalId, [FromBody] JsonElement body)
    {
        try
        {
            PrestadorHorariosRequest request = ParseDireccionesBody(body);
            await prestadorService.UpdateHorariosAsync(profesionalId, request);

            var agendasDb = await prestadorRepository.GetAgendasByProfesionalAsync(profesionalId);
            var direcciones = new List<object>();
            foreach (var agenda in agendasDb)
            {
                var horarios = await prestadorRepository.GetHorariosByAgendaAsync(agenda.Id);
                var grupos = horarios
                    .GroupBy(h => new { inicio = h.HoraInicio.ToString("HH:mm"), fin = h.HoraFin.ToString("HH:mm"), dur = h.DuracionConsulta })
                    .Select(g => new
                    {
                        id = g.Select(x => x.Id).FirstOrDefault(),
                        diasDeLaSemana = g.Select(x => Enum.GetName(typeof(DiaAtencion), x.DiaDeAtencion) ?? x.DiaDeAtencion.ToString()).Distinct().ToList(),
                        horaInicio = g.Key.inicio,
                        horaFin = g.Key.fin,
                        duracionMinutos = (int?)g.Key.dur,
                        especialidades = g.Select(x => x.EspecialidadId).Where(e => e > 0).Distinct().ToList(),
                        especialidadId = (int?)g.Select(x => x.EspecialidadId).Where(e => e > 0).Distinct().FirstOrDefault()
                    })
                    .ToList();

                direcciones.Add(new
                {
                    id = agenda.Id,
                    direccion = agenda.Direccion,
                    duracionConsulta = (int?)agenda.DuracionConsulta,
                    horariosAtencion = grupos
                });
            }

            logger.LogSuccess("Agenda actualizada exitosamente.");
            return Ok(direcciones);
        }
        catch (Exception ex)
        {
            logger.LogError("Error al actualizar la Agenda.", ex);
            return StatusCode(500, "Error interno del servidor.");
        }
    }

    private static PrestadorHorariosRequest ParseDireccionesBody(JsonElement body)
    {
        var result = new PrestadorHorariosRequest();

        List<JsonElement> direccionesElements;
        if (body.ValueKind == JsonValueKind.Array)
        {
            direccionesElements = body.EnumerateArray().ToList();
        }
        else if (body.ValueKind == JsonValueKind.Object)
        {
            if (body.TryGetProperty("direcciones", out var dirProp) && dirProp.ValueKind == JsonValueKind.Array)
            {
                direccionesElements = dirProp.EnumerateArray().ToList();
            }
            else if (body.TryGetProperty("lugares", out var lugProp) && lugProp.ValueKind == JsonValueKind.Array)
            {
                direccionesElements = lugProp.EnumerateArray().ToList();
            }
            else
            {
                direccionesElements = new List<JsonElement>();
            }
        }
        else
        {
            direccionesElements = new List<JsonElement>();
        }

        foreach (var dir in direccionesElements)
        {
            var direccion = new DireccionHorariosDTO
            {
                Id = dir.TryGetProperty("id", out var idProp) && idProp.ValueKind == JsonValueKind.Number
                    ? idProp.GetInt32()
                    : (int?)null,
                Direccion = dir.TryGetProperty("direccion", out var direccionProp) && direccionProp.ValueKind == JsonValueKind.String
                    ? direccionProp.GetString()
                    : string.Empty,
                DuracionConsulta = dir.TryGetProperty("duracionConsulta", out var durProp) && durProp.ValueKind == JsonValueKind.Number
                    ? durProp.GetInt32()
                    : (int?)null
            };

            if (dir.TryGetProperty("horariosAtencion", out var horariosProp) && horariosProp.ValueKind == JsonValueKind.Array)
            {
                foreach (var h in horariosProp.EnumerateArray())
                {
                    var horario = new HorarioEdicionDTO
                    {
                        Id = h.TryGetProperty("id", out var hIdProp) && hIdProp.ValueKind == JsonValueKind.Number ? hIdProp.GetInt32() : (int?)null,
                        HoraInicio = h.TryGetProperty("horaInicio", out var hiProp) && hiProp.ValueKind == JsonValueKind.String ? hiProp.GetString() : "",
                        HoraFin = h.TryGetProperty("horaFin", out var hfProp) && hfProp.ValueKind == JsonValueKind.String ? hfProp.GetString() : "",
                        DuracionMinutos = h.TryGetProperty("duracionMinutos", out var dmProp) && dmProp.ValueKind == JsonValueKind.Number ? dmProp.GetInt32() : (int?)null,
                        Especialidades = h.TryGetProperty("especialidades", out var espProp) && espProp.ValueKind == JsonValueKind.Array
                            ? espProp.EnumerateArray().Where(x => x.ValueKind == JsonValueKind.Number).Select(x => x.GetInt32()).ToList()
                            : new List<int>()
                    };

                    // Si no vienen "especialidades" pero sí "especialidadId", normalizar a array de 1
                    if ((horario.Especialidades == null || horario.Especialidades.Count == 0)
                        && h.TryGetProperty("especialidadId", out var espIdProp)
                        && espIdProp.ValueKind == JsonValueKind.Number)
                    {
                        horario.Especialidades = new List<int> { espIdProp.GetInt32() };
                    }

                    // Mapear listas de días en string a ints 0..6
                    if (h.TryGetProperty("diasDeLaSemana", out var diasProp) && diasProp.ValueKind == JsonValueKind.Array)
                    {
                        var dias = new List<int>();
                        foreach (var d in diasProp.EnumerateArray())
                        {
                            if (d.ValueKind == JsonValueKind.String)
                            {
                                var name = (d.GetString() ?? string.Empty).Trim();
                                dias.Add(MapDiaNombreToInt(name));
                            }
                            else if (d.ValueKind == JsonValueKind.Number)
                            {
                                dias.Add(d.GetInt32());
                            }
                        }
                        horario.DiasDeLaSemana = dias;
                    }

                    direccion.HorariosAtencion.Add(horario);
                }
            }

            result.Direcciones.Add(direccion);
        }

        return result;
    }

    private static int MapDiaNombreToInt(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre)) return 0;
        var n = nombre.ToLowerInvariant();
        return n switch
        {
            "domingo" => 0,
            "lunes" => 1,
            "martes" => 2,
            "miércoles" => 3,
            "miercoles" => 3,
            "jueves" => 4,
            "viernes" => 5,
            "sábado" => 6,
            "sabado" => 6,
            _ => 0
        };
    }
}
