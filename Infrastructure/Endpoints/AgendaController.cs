using Application.Contracts.DTOs.Response;
using Application.Contracts.DTOs.Request;
using Application.Contracts.Interfaces;
using Application.Utilities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Domain.Entities;

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

    private static Dictionary<string, int> BuildDireccionesMap(IEnumerable<Direccion>? direcciones)
    {
        if (direcciones == null) return new Dictionary<string, int>();

        return direcciones
            .GroupBy(d => (d.Calle ?? string.Empty).Trim())
            .ToDictionary(g => g.Key, g => g.First().Id);
    }

    // DTO que el front puede consumir directo: incluye id técnico (lugarId) y detalle de dirección
    private static object BuildDireccionDto(
        string direccionTexto,
        int lugarId,
        Direccion? direccionEntidad,
        IEnumerable<object> horariosAtencion)
    {
        object? detalleDireccion = null;
        if (direccionEntidad != null)
        {
            detalleDireccion = new
            {
                id = direccionEntidad.Id,
                calle = direccionEntidad.Calle,
                altura = direccionEntidad.Altura,
                piso = direccionEntidad.Piso,
                departamento = direccionEntidad.Departamento,
                provinciaCiudad = direccionEntidad.ProvinciaCiudad,
                codigoPostal = direccionEntidad.CodigoPostal
            };
        }

        return new
        {
            lugarId,
            detalleDireccion,
            horariosAtencion
        };
    }

    [HttpGet("getByProfesional/{profesionalId:int}")]
    public async Task<IActionResult> GetAllByProfesionalAsync([FromRoute] int profesionalId)
    {
        try
        {
            var prestador = await prestadorRepository.GetByIdWithDetailsAsync(profesionalId);
            if (prestador == null) return NotFound(new { message = "Profesional no encontrado" });

            var agendasDb = await prestadorRepository.GetAgendasByProfesionalAsync(profesionalId);

            // Consolidar por direccion (texto) y mapear lugarId estable desde Prestador.Direcciones
            var direccionesMap = BuildDireccionesMap(prestador.Direcciones);

            var gruposPorDireccion = agendasDb
                .GroupBy(a => (a.Direccion ?? string.Empty).Trim())
                .ToList();

            var direcciones = new List<object>();
            foreach (var grupo in gruposPorDireccion)
            {
                var direccionTexto = grupo.Key;

                // Solo mostramos direcciones que existan como lugares propios del profesional.
                // Los horarios que provienen de centros (sin lugar del profesional) no se exponen aquí.
                if (!direccionesMap.TryGetValue(direccionTexto, out var lugarId))
                    continue;

                // Reunir horarios de todas las agendas en esta dirección
                var horariosAcumulados = new List<Domain.Entities.HorarioAtencion>();
                foreach (var agenda in grupo)
                {
                    var horarios = await prestadorRepository.GetHorariosByAgendaAsync(agenda.Id);
                    horariosAcumulados.AddRange(horarios);
                }

                // Agrupar por tramo y especialidadId para obtener 'diasDeLaSemana' como strings
                var horariosCanonicos = horariosAcumulados
                    .GroupBy(h => new
                    {
                        inicio = h.HoraInicio.ToString("HH:mm"),
                        fin = h.HoraFin.ToString("HH:mm"),
                        dur = h.DuracionConsulta,
                        esp = h.EspecialidadId
                    })
                    .Select(g => new
                    {
                        id = g.Min(x => x.Id),
                        diasDeLaSemana = g.Select(x => ToTituloDia(x.DiaDeAtencion)).Distinct().ToList(),
                        horaInicio = g.Key.inicio,
                        horaFin = g.Key.fin,
                        duracionConsulta = (int?)g.Key.dur,
                        especialidadId = (int?)g.Key.esp,
                        prestadorId = profesionalId
                    })
                    .Cast<object>()
                    .ToList();

                Direccion? direccionEntidad = null;
                if (lugarId != 0)
                {
                    direccionEntidad = prestador.Direcciones?.FirstOrDefault(d => d.Id == lugarId);
                }

                direcciones.Add(BuildDireccionDto(direccionTexto, lugarId, direccionEntidad, horariosCanonicos));
            }

            logger.LogSuccess("Agenda obtenida exitosamente.");
            return Ok(new
            {
                profesionalId,
                direcciones
            });
        }
        catch (Exception ex)
        {
            logger.LogError("Error al obtener la Agenda.", ex);
            return StatusCode(500, "Error interno del servidor.");
        }
    }

    [HttpGet("getByCentro/{centroId:int}")]
    public async Task<IActionResult> GetAllByCentroAsync([FromRoute] int centroId)
    {
        try
        {
            var centro = await prestadorRepository.GetByIdWithDetailsAsync(centroId);
            if (centro == null) return NotFound(new { message = "Centro médico no encontrado" });
            if (centro.Rol != Domain.Enums.RolMedico.CentroMedico)
                return BadRequest(new { message = "El prestador indicado no es un centro médico" });

            var profesionales = centro.Profesionales ?? new List<Domain.Entities.Prestador>();
            if (profesionales.Count == 0)
            {
                return Ok(new { centroId, direcciones = Array.Empty<object>() });
            }

            // 1) Traer agendas y horarios de TODOS los profesionales del centro en 2 queries
            var profesionalIds = profesionales.Select(p => p.Id).ToList();
            var agendas = await prestadorRepository.GetAgendasByProfesionalesAsync(profesionalIds);
            if (agendas.Count == 0)
            {
                return Ok(new { centroId, direcciones = Array.Empty<object>() });
            }

            var agendaIds = agendas.Select(a => a.Id).ToList();
            var horarios = await prestadorRepository.GetHorariosByAgendasAsync(agendaIds);

            // Índices en memoria
            var horariosPorAgenda = horarios
                .GroupBy(h => h.AgendaId)
                .ToDictionary(g => g.Key, g => g.ToList());

            var profesionalPorId = profesionales.ToDictionary(p => p.Id, p => p);

            // Mapa de direcciones del propio centro: usamos SIEMPRE el lugar del centro como id estable
            var direccionesCentroMap = BuildDireccionesMap(centro.Direcciones);

            // key: texto direccion, value: (lugarId, direccionEntidad, lista de horariosAtencion)
            var direccionesCentro = new Dictionary<string, (int lugarId, Direccion? direccionEntidad, List<object> horariosAtencion)>();

            // 2) Recorrer agendas una sola vez y acumular DTOs de horarios por dirección
            var agendasPorProfesional = agendas.GroupBy(a => a.ProfesionalId);
            foreach (var grupoProfesional in agendasPorProfesional)
            {
                if (!profesionalPorId.TryGetValue(grupoProfesional.Key, out var profesional))
                    continue;

                foreach (var agenda in grupoProfesional)
                {
                    var direccionTexto = (agenda.Direccion ?? string.Empty).Trim();

                    // Para la vista de centro, el lugarId siempre es el del CENTRO (no del profesional)
                    if (!direccionesCentroMap.TryGetValue(direccionTexto, out var lugarId))
                    {
                        lugarId = 0;
                    }

                    if (!horariosPorAgenda.TryGetValue(agenda.Id, out var horariosDeAgenda) || horariosDeAgenda.Count == 0)
                        continue;

                    var horariosCanonicos = horariosDeAgenda
                        .GroupBy(h => new
                        {
                            inicio = h.HoraInicio.ToString("HH:mm"),
                            fin = h.HoraFin.ToString("HH:mm"),
                            dur = h.DuracionConsulta,
                            esp = h.EspecialidadId
                        })
                        .Select(g => new
                        {
                            id = g.Min(x => x.Id),
                            diasDeLaSemana = g.Select(x => ToTituloDia(x.DiaDeAtencion)).Distinct().ToList(),
                            horaInicio = g.Key.inicio,
                            horaFin = g.Key.fin,
                            duracionConsulta = (int?)g.Key.dur,
                            especialidadId = (int?)g.Key.esp,
                            prestadorId = profesional.Id
                        })
                        .Cast<object>()
                        .ToList();

                    Direccion? direccionEntidad = null;
                    if (lugarId != 0)
                        direccionEntidad = centro.Direcciones?.FirstOrDefault(d => d.Id == lugarId);

                    if (!direccionesCentro.TryGetValue(direccionTexto, out var existente))
                    {
                        direccionesCentro[direccionTexto] = (lugarId, direccionEntidad, new List<object>(horariosCanonicos));
                    }
                    else
                    {
                        existente.horariosAtencion.AddRange(horariosCanonicos);
                        var finalLugarId = existente.lugarId != 0 ? existente.lugarId : lugarId;
                        var finalDireccionEntidad = existente.direccionEntidad ?? direccionEntidad;
                        direccionesCentro[direccionTexto] = (finalLugarId, finalDireccionEntidad, existente.horariosAtencion);
                    }
                }
            }

            var direcciones = direccionesCentro
                .Select(kvp => BuildDireccionDto(
                    kvp.Key,
                    kvp.Value.lugarId,
                    kvp.Value.direccionEntidad,
                    kvp.Value.horariosAtencion))
                .ToList();

            logger.LogSuccess("Agendas de centro obtenidas exitosamente.");
            return Ok(new
            {
                centroId,
                direcciones
            });
        }
        catch (Exception ex)
        {
            logger.LogError("Error al obtener las Agendas del centro médico.", ex);
            return StatusCode(500, "Error interno del servidor.");
        }
    }

    [HttpPut("{profesionalId}/direcciones")]
    public async Task<IActionResult> ReplaceDireccionesAsync([FromRoute] int profesionalId, [FromQuery] string? strategy, [FromBody] JsonElement body)
    {
        try
        {
            PrestadorHorariosRequest request = ParseDireccionesBody(body);
            var strategyValue = string.IsNullOrWhiteSpace(strategy) ? "merge" : strategy;
            await prestadorService.UpdateHorariosAsync(profesionalId, request, strategyValue);

            // Devolver SIEMPRE el modelo normalizado canónico
            var prestador = await prestadorRepository.GetByIdWithDetailsAsync(profesionalId);
            if (prestador == null) return NotFound(new { message = "Profesional no encontrado" });
            var agendasDb = await prestadorRepository.GetAgendasByProfesionalAsync(profesionalId);

            var direccionesMap = BuildDireccionesMap(prestador.Direcciones);

            var gruposPorDireccion = agendasDb.GroupBy(a => (a.Direccion ?? string.Empty).Trim()).ToList();
            var direcciones = new List<object>();
            foreach (var grupo in gruposPorDireccion)
            {
                var direccionTexto = grupo.Key;
                if (!direccionesMap.TryGetValue(direccionTexto, out var lugarId))
                {
                    // No crear direcciones nuevas desde este endpoint.
                    // Si la dirección no existe como lugar del prestador, se devuelve lugarId = 0.
                    lugarId = 0;
                }

                var horariosAcumulados = new List<Domain.Entities.HorarioAtencion>();
                foreach (var agenda in grupo)
                {
                    var horarios = await prestadorRepository.GetHorariosByAgendaAsync(agenda.Id);
                    horariosAcumulados.AddRange(horarios);
                }

                var horariosCanonicos = horariosAcumulados
                    .GroupBy(h => new
                    {
                        inicio = h.HoraInicio.ToString("HH:mm"),
                        fin = h.HoraFin.ToString("HH:mm"),
                        dur = h.DuracionConsulta,
                        esp = h.EspecialidadId
                    })
                    .Select(g => new
                    {
                        id = g.Min(x => x.Id),
                        diasDeLaSemana = g.Select(x => ToTituloDia(x.DiaDeAtencion)).Distinct().ToList(),
                        horaInicio = g.Key.inicio,
                        horaFin = g.Key.fin,
                        duracionConsulta = (int?)g.Key.dur,
                        especialidadId = (int?)g.Key.esp,
                        prestadorId = profesionalId
                    })
                    .Cast<object>()
                    .ToList();

                var direccionEntidad = prestador.Direcciones?.FirstOrDefault(d => d.Id == lugarId);

                direcciones.Add(BuildDireccionDto(direccionTexto, lugarId, direccionEntidad, horariosCanonicos));
            }

            logger.LogSuccess("Agenda actualizada exitosamente.");
            return Ok(new { profesionalId, direcciones });
        }
        catch (Exception ex)
        {
            logger.LogError("Error al actualizar la Agenda.", ex);
            return StatusCode(500, "Error interno del servidor.");
        }
    }

	[HttpPost("{profesionalId:int}/lugares/{lugarId:int}/horarios")]
	public async Task<IActionResult> CreateHorariosForLugarAsync([FromRoute] int profesionalId, [FromRoute] int lugarId, [FromBody] JsonElement body)
	{
		try
		{
			var prestador = await prestadorRepository.GetByIdWithDetailsAsync(profesionalId);
			if (prestador == null) return NotFound(new { message = "Profesional no encontrado" });

			var direccionEntidad = prestador.Direcciones?.FirstOrDefault(d => d.Id == lugarId);
			if (direccionEntidad == null) return NotFound(new { message = "Lugar no encontrado" });
			var direccionTexto = (direccionEntidad.Calle ?? string.Empty).Trim();

			var items = ParseHorariosForLugar(body);
			foreach (var h in items)
			{
				var dias = (h.DiasDeLaSemana != null && h.DiasDeLaSemana.Any()) ? h.DiasDeLaSemana : (h.DiaSemana >= 0 ? new List<int> { h.DiaSemana } : new List<int>());
				if (dias.Count == 0) continue;

				var inicio = TimeSpan.Parse(h.HoraInicio);
				var fin = TimeSpan.Parse(h.HoraFin);
				if (fin <= inicio) continue;

				var duracion = h.DuracionMinutos ?? 30;
				var especialidades = (h.Especialidades ?? new List<int>()).Where(e => e >= 0).Distinct().ToList();
				if (especialidades.Count == 0) especialidades = new List<int> { 0 };

				foreach (var especialidadId in especialidades)
				{
					var agenda = await prestadorRepository.GetAgendaAsync(profesionalId, especialidadId, direccionTexto);
					if (agenda == null)
					{
						agenda = new Domain.Entities.Agenda
						{
							ProfesionalId = profesionalId,
							EspecialidadId = especialidadId,
							Direccion = direccionTexto,
							DuracionConsulta = duracion,
							Alta = DateTime.UtcNow
						};
						await prestadorRepository.AddAgendaAsync(agenda);
						await prestadorRepository.SaveChangesAsync();
					}

					var nuevos = new List<Domain.Entities.HorarioAtencion>();
					foreach (var dia in dias)
					{
						nuevos.Add(new Domain.Entities.HorarioAtencion
						{
							AgendaId = agenda.Id,
							DiaDeAtencion = (Domain.Enums.DiaAtencion)dia,
							HoraInicio = DateTime.Today.Date.Add(inicio),
							HoraFin = DateTime.Today.Date.Add(fin),
							EspecialidadId = especialidadId,
							DuracionConsulta = duracion,
							Alta = DateTime.UtcNow
						});
					}
					await prestadorRepository.AddHorariosAsync(nuevos);
				}
			}

			// Respuesta canónica (igual que GET/PUT)
			var agendasDb = await prestadorRepository.GetAgendasByProfesionalAsync(profesionalId);
			var direccionesMap = BuildDireccionesMap(prestador.Direcciones);
			var gruposPorDireccion = agendasDb.GroupBy(a => (a.Direccion ?? string.Empty).Trim()).ToList();
			var direcciones = new List<object>();
			foreach (var grupo in gruposPorDireccion)
			{
				var dirTxt = grupo.Key;
				if (!direccionesMap.TryGetValue(dirTxt, out var lid))
				{
                    // No crear direcciones nuevas desde este endpoint.
                    // Si la dirección no existe como lugar del prestador, se devuelve lugarId = 0.
                    lid = 0;
				}

				var horariosAcumulados = new List<Domain.Entities.HorarioAtencion>();
				foreach (var agenda in grupo)
				{
					var horarios = await prestadorRepository.GetHorariosByAgendaAsync(agenda.Id);
					horariosAcumulados.AddRange(horarios);
				}

				var horariosCanonicos = horariosAcumulados
					.GroupBy(h => new { inicio = h.HoraInicio.ToString("HH:mm"), fin = h.HoraFin.ToString("HH:mm"), dur = h.DuracionConsulta, esp = h.EspecialidadId })
					.Select(g => new
					{
						id = g.Min(x => x.Id),
						diasDeLaSemana = g.Select(x => ToTituloDia(x.DiaDeAtencion)).Distinct().ToList(),
						horaInicio = g.Key.inicio,
						horaFin = g.Key.fin,
						duracionConsulta = (int?)g.Key.dur,
						especialidadId = (int?)g.Key.esp,
						prestadorId = profesionalId
					})
					.Cast<object>()
					.ToList();

				var direccionEntidadDetalle = prestador.Direcciones?.FirstOrDefault(d => d.Id == lid);

				direcciones.Add(BuildDireccionDto(dirTxt, lid, direccionEntidadDetalle, horariosCanonicos));
			}

			logger.LogSuccess("Horario(s) creado(s) exitosamente.");
			return Ok(new { profesionalId, direcciones });
		}
		catch (Exception ex)
		{
			logger.LogError("Error al crear horarios.", ex);
			return StatusCode(500, "Error interno del servidor.");
		}
	}

	[HttpPut("{profesionalId:int}/lugares/{lugarId:int}/horarios/{horarioId:int}")]
	public async Task<IActionResult> UpdateHorarioForLugarAsync([FromRoute] int profesionalId, [FromRoute] int lugarId, [FromRoute] int horarioId, [FromBody] JsonElement body)
	{
		try
		{
			var prestador = await prestadorRepository.GetByIdWithDetailsAsync(profesionalId);
			if (prestador == null) return NotFound(new { message = "Profesional no encontrado" });

			var direccionEntidad = prestador.Direcciones?.FirstOrDefault(d => d.Id == lugarId);
			if (direccionEntidad == null) return NotFound(new { message = "Lugar no encontrado" });
			var direccionTexto = (direccionEntidad.Calle ?? string.Empty).Trim();

			var anchor = await prestadorRepository.GetHorarioByIdAsync(horarioId);
			if (anchor == null) return NotFound(new { message = "Horario no encontrado" });

			var anchorAgenda = await prestadorRepository.GetAgendaByIdAsync(anchor.AgendaId) ?? throw new Exception("Agenda no encontrada");
			if (anchorAgenda.ProfesionalId != profesionalId) return BadRequest(new { message = "Horario no pertenece al profesional" });
			if ((anchorAgenda.Direccion ?? string.Empty).Trim() != direccionTexto) return BadRequest(new { message = "Horario no pertenece al lugar indicado" });

			var datos = ParseSingleHorarioForLugar(body);

			var dias = (datos.DiasDeLaSemana != null && datos.DiasDeLaSemana.Any()) ? datos.DiasDeLaSemana : new List<int> { (int)anchor.DiaDeAtencion };
			var inicio = !string.IsNullOrWhiteSpace(datos.HoraInicio) ? TimeSpan.Parse(datos.HoraInicio) : anchor.HoraInicio.TimeOfDay;
			var fin = !string.IsNullOrWhiteSpace(datos.HoraFin) ? TimeSpan.Parse(datos.HoraFin) : anchor.HoraFin.TimeOfDay;
			if (fin <= inicio) return BadRequest(new { message = "Rango horario inválido" });
			var duracion = datos.DuracionMinutos ?? anchor.DuracionConsulta;
			var especialidades = (datos.Especialidades != null && datos.Especialidades.Any()) ? datos.Especialidades.Distinct().ToList() : new List<int> { anchor.EspecialidadId };

			// Limpiar tramo antiguo (mismo inicio/fin) excepto anchor
			var antiguos = await prestadorRepository.GetHorariosByAgendaAndTramoAsync(anchorAgenda.Id, anchor.HoraInicio.TimeOfDay, anchor.HoraFin.TimeOfDay);
			var idsAEliminar = antiguos.Where(x => x.Id != anchor.Id).Select(x => x.Id).ToList();
			await prestadorRepository.DeleteHorariosByIdsAsync(idsAEliminar);

			bool setAnchor = false;
			foreach (var espId in especialidades)
			{
				var agendaDestino = await prestadorRepository.GetAgendaAsync(profesionalId, espId, direccionTexto);
				if (agendaDestino == null)
				{
					agendaDestino = new Domain.Entities.Agenda
					{
						ProfesionalId = profesionalId,
						EspecialidadId = espId,
						Direccion = direccionTexto,
						DuracionConsulta = duracion,
						Alta = DateTime.UtcNow
					};
					await prestadorRepository.AddAgendaAsync(agendaDestino);
					await prestadorRepository.SaveChangesAsync();
				}

				if (!setAnchor)
				{
					anchor.AgendaId = agendaDestino.Id;
					anchor.DiaDeAtencion = (Domain.Enums.DiaAtencion)dias.First();
					anchor.HoraInicio = DateTime.Today.Date.Add(inicio);
					anchor.HoraFin = DateTime.Today.Date.Add(fin);
					anchor.EspecialidadId = espId;
					anchor.DuracionConsulta = duracion;
					await prestadorRepository.UpdateHorarioAsync(anchor);

					var restantesDias = dias.Skip(1).ToList();
					var nuevos = new List<Domain.Entities.HorarioAtencion>();
					foreach (var dia in restantesDias)
					{
						nuevos.Add(new Domain.Entities.HorarioAtencion
						{
							AgendaId = agendaDestino.Id,
							DiaDeAtencion = (Domain.Enums.DiaAtencion)dia,
							HoraInicio = DateTime.Today.Date.Add(inicio),
							HoraFin = DateTime.Today.Date.Add(fin),
							EspecialidadId = espId,
							DuracionConsulta = duracion,
							Alta = DateTime.UtcNow
						});
					}
					await prestadorRepository.AddHorariosAsync(nuevos);
					setAnchor = true;
				}
				else
				{
					var nuevos = new List<Domain.Entities.HorarioAtencion>();
					foreach (var dia in dias)
					{
						nuevos.Add(new Domain.Entities.HorarioAtencion
						{
							AgendaId = agendaDestino.Id,
							DiaDeAtencion = (Domain.Enums.DiaAtencion)dia,
							HoraInicio = DateTime.Today.Date.Add(inicio),
							HoraFin = DateTime.Today.Date.Add(fin),
							EspecialidadId = espId,
							DuracionConsulta = duracion,
							Alta = DateTime.UtcNow
						});
					}
					await prestadorRepository.AddHorariosAsync(nuevos);
				}
			}

			// Respuesta canónica (igual que GET/PUT)
			var agendasDb = await prestadorRepository.GetAgendasByProfesionalAsync(profesionalId);
			var direccionesMap = BuildDireccionesMap(prestador.Direcciones);
			var gruposPorDireccion = agendasDb.GroupBy(a => (a.Direccion ?? string.Empty).Trim()).ToList();
			var direcciones = new List<object>();
			foreach (var grupo in gruposPorDireccion)
			{
				var dirTxt = grupo.Key;
				if (!direccionesMap.TryGetValue(dirTxt, out var lid))
				{
                    // No crear direcciones nuevas desde este endpoint.
                    // Si la dirección no existe como lugar del prestador, se devuelve lugarId = 0.
                    lid = 0;
				}

				var horariosAcumulados = new List<Domain.Entities.HorarioAtencion>();
				foreach (var agenda in grupo)
				{
					var horarios = await prestadorRepository.GetHorariosByAgendaAsync(agenda.Id);
					horariosAcumulados.AddRange(horarios);
				}

				var horariosCanonicos = horariosAcumulados
					.GroupBy(h => new { inicio = h.HoraInicio.ToString("HH:mm"), fin = h.HoraFin.ToString("HH:mm"), dur = h.DuracionConsulta, esp = h.EspecialidadId })
					.Select(g => new
					{
						id = g.Min(x => x.Id),
						diasDeLaSemana = g.Select(x => ToTituloDia(x.DiaDeAtencion)).Distinct().ToList(),
						horaInicio = g.Key.inicio,
						horaFin = g.Key.fin,
						duracionConsulta = (int?)g.Key.dur,
						especialidadId = (int?)g.Key.esp,
						prestadorId = profesionalId
					})
					.ToList();

				direcciones.Add(new { lugarId = lid, direccion = dirTxt, horariosAtencion = horariosCanonicos });
			}

			logger.LogSuccess("Horario actualizado exitosamente.");
			return Ok(new { profesionalId, direcciones });
		}
		catch (Exception ex)
		{
			logger.LogError("Error al actualizar horario.", ex);
			return StatusCode(500, "Error interno del servidor.");
		}
	}

	[HttpDelete("{profesionalId:int}/lugares/{lugarId:int}/horarios/{horarioId:int}")]
	public async Task<IActionResult> DeleteHorarioForLugarAsync([FromRoute] int profesionalId, [FromRoute] int lugarId, [FromRoute] int horarioId)
	{
		try
		{
			var prestador = await prestadorRepository.GetByIdWithDetailsAsync(profesionalId);
			if (prestador == null) return NotFound(new { message = "Profesional no encontrado" });

			var direccionEntidad = prestador.Direcciones?.FirstOrDefault(d => d.Id == lugarId);
			if (direccionEntidad == null) return NotFound(new { message = "Lugar no encontrado" });
			var direccionTexto = (direccionEntidad.Calle ?? string.Empty).Trim();

			var anchor = await prestadorRepository.GetHorarioByIdAsync(horarioId);
			if (anchor == null) return NotFound(new { message = "Horario no encontrado" });

			var agenda = await prestadorRepository.GetAgendaByIdAsync(anchor.AgendaId) ?? throw new Exception("Agenda no encontrada");
			if (agenda.ProfesionalId != profesionalId) return BadRequest(new { message = "Horario no pertenece al profesional" });
			if ((agenda.Direccion ?? string.Empty).Trim() != direccionTexto) return BadRequest(new { message = "Horario no pertenece al lugar indicado" });

			var matching = await prestadorRepository.GetHorariosByAgendaAndTramoAsync(agenda.Id, anchor.HoraInicio.TimeOfDay, anchor.HoraFin.TimeOfDay);
			var idsToDelete = matching
				.Where(h => h.DuracionConsulta == anchor.DuracionConsulta && h.EspecialidadId == anchor.EspecialidadId)
				.Select(h => h.Id)
				.ToList();
			await prestadorRepository.DeleteHorariosByIdsAsync(idsToDelete);

			// Respuesta canónica (igual que GET/PUT)
			var agendasDb = await prestadorRepository.GetAgendasByProfesionalAsync(profesionalId);
			var direccionesMap = BuildDireccionesMap(prestador.Direcciones);
			var gruposPorDireccion = agendasDb.GroupBy(a => (a.Direccion ?? string.Empty).Trim()).ToList();
			var direcciones = new List<object>();
			foreach (var grupo in gruposPorDireccion)
			{
				var dirTxt = grupo.Key;
				if (!direccionesMap.TryGetValue(dirTxt, out var lid))
				{
                    // No crear direcciones nuevas desde este endpoint.
                    // Si la dirección no existe como lugar del prestador, se devuelve lugarId = 0.
                    lid = 0;
				}

				var horariosAcumulados = new List<Domain.Entities.HorarioAtencion>();
				foreach (var ag in grupo)
				{
					var horarios = await prestadorRepository.GetHorariosByAgendaAsync(ag.Id);
					horariosAcumulados.AddRange(horarios);
				}

				var horariosCanonicos = horariosAcumulados
					.GroupBy(h => new { inicio = h.HoraInicio.ToString("HH:mm"), fin = h.HoraFin.ToString("HH:mm"), dur = h.DuracionConsulta, esp = h.EspecialidadId })
					.Select(g => new
					{
						id = g.Min(x => x.Id),
						diasDeLaSemana = g.Select(x => ToTituloDia(x.DiaDeAtencion)).Distinct().ToList(),
						horaInicio = g.Key.inicio,
						horaFin = g.Key.fin,
						duracionConsulta = (int?)g.Key.dur,
						especialidadId = (int?)g.Key.esp
					})
					.Cast<object>()
					.ToList();

				var direccionEntidadDetalle = prestador.Direcciones?.FirstOrDefault(d => d.Id == lid);

				direcciones.Add(BuildDireccionDto(dirTxt, lid, direccionEntidadDetalle, horariosCanonicos));
			}

			logger.LogSuccess("Horario eliminado exitosamente.");
			return Ok(new { profesionalId, direcciones });
		}
		catch (Exception ex)
		{
			logger.LogError("Error al eliminar horario.", ex);
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
                // Aceptar 'lugarId' como Id canónico del lugar
                Id = dir.TryGetProperty("lugarId", out var lidProp) && lidProp.ValueKind == JsonValueKind.Number
                    ? lidProp.GetInt32()
                    : (dir.TryGetProperty("id", out var idProp) && idProp.ValueKind == JsonValueKind.Number
                    ? idProp.GetInt32()
                    : (int?)null),
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
                        // Aceptar tanto 'duracionMinutos' como 'duracionConsulta' y normalizar
                        DuracionMinutos = h.TryGetProperty("duracionConsulta", out var dcProp) && dcProp.ValueKind == JsonValueKind.Number
                            ? dcProp.GetInt32()
                            : (h.TryGetProperty("duracionMinutos", out var dmProp) && dmProp.ValueKind == JsonValueKind.Number ? dmProp.GetInt32() : (int?)null),
                        // Aceptar 'especialidades' o 'especialidadId'
                        Especialidades = h.TryGetProperty("especialidades", out var espProp) && espProp.ValueKind == JsonValueKind.Array
                            ? espProp.EnumerateArray().Where(x => x.ValueKind == JsonValueKind.Number).Select(x => x.GetInt32()).ToList()
                            : new List<int>()
                    };

                    // ProfesionalId opcional (para centros médicos)
                    if (h.TryGetProperty("profesionalId", out var profProp) && profProp.ValueKind == JsonValueKind.Number)
                    {
                        horario.ProfesionalId = profProp.GetInt32();
                    }

                    // Flag de borrado opcional
                    if (h.TryGetProperty("deleted", out var delProp) && (delProp.ValueKind == JsonValueKind.True || delProp.ValueKind == JsonValueKind.False))
                    {
                        horario.Deleted = delProp.GetBoolean();
                    }

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

        // removeIds globales opcionales
        if (body.ValueKind == JsonValueKind.Object && body.TryGetProperty("removeIds", out var removeProp) && removeProp.ValueKind == JsonValueKind.Array)
        {
            result.RemoveIds = removeProp.EnumerateArray().Where(x => x.ValueKind == JsonValueKind.Number).Select(x => x.GetInt32()).ToList();
        }
        else
        {
            result.RemoveIds = new List<int>();
        }

        return result;
    }

	private static List<HorarioEdicionDTO> ParseHorariosForLugar(JsonElement body)
	{
		var list = new List<HorarioEdicionDTO>();

		if (body.ValueKind == JsonValueKind.Array)
		{
			foreach (var h in body.EnumerateArray())
			{
				list.Add(ParseSingleHorarioForLugar(h));
			}
		}
		else if (body.ValueKind == JsonValueKind.Object)
		{
			list.Add(ParseSingleHorarioForLugar(body));
		}

		return list;
	}

	private static HorarioEdicionDTO ParseSingleHorarioForLugar(JsonElement h)
	{
		var horario = new HorarioEdicionDTO
		{
			Id = h.TryGetProperty("id", out var hIdProp) && hIdProp.ValueKind == JsonValueKind.Number ? hIdProp.GetInt32() : (int?)null,
			HoraInicio = h.TryGetProperty("horaInicio", out var hiProp) && hiProp.ValueKind == JsonValueKind.String ? hiProp.GetString() ?? "" : "",
			HoraFin = h.TryGetProperty("horaFin", out var hfProp) && hfProp.ValueKind == JsonValueKind.String ? hfProp.GetString() ?? "" : "",
			DuracionMinutos = h.TryGetProperty("duracionConsulta", out var dcProp) && dcProp.ValueKind == JsonValueKind.Number
				? dcProp.GetInt32()
				: (h.TryGetProperty("duracionMinutos", out var dmProp) && dmProp.ValueKind == JsonValueKind.Number ? dmProp.GetInt32() : (int?)null),
			Especialidades = h.TryGetProperty("especialidades", out var espProp) && espProp.ValueKind == JsonValueKind.Array
				? espProp.EnumerateArray().Where(x => x.ValueKind == JsonValueKind.Number).Select(x => x.GetInt32()).ToList()
				: new List<int>()
		};

		// Soportar especialidadId singular
		if ((horario.Especialidades == null || horario.Especialidades.Count == 0)
			&& h.TryGetProperty("especialidadId", out var espIdProp)
			&& espIdProp.ValueKind == JsonValueKind.Number)
		{
			horario.Especialidades = new List<int> { espIdProp.GetInt32() };
		}

		// Mapear días (nombres o números)
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
		else if (h.TryGetProperty("diaSemana", out var diaProp) && diaProp.ValueKind == JsonValueKind.Number)
		{
			horario.DiaSemana = diaProp.GetInt32();
		}

		return horario;
	}

    private static string ToTituloDia(Domain.Enums.DiaAtencion dia)
    {
        // Normalizar a título con mayúscula inicial
        return dia switch
        {
            Domain.Enums.DiaAtencion.Domingo => "Domingo",
            Domain.Enums.DiaAtencion.Lunes => "Lunes",
            Domain.Enums.DiaAtencion.Martes => "Martes",
            Domain.Enums.DiaAtencion.Miercoles => "Miércoles",
            Domain.Enums.DiaAtencion.Jueves => "Jueves",
            Domain.Enums.DiaAtencion.Viernes => "Viernes",
            Domain.Enums.DiaAtencion.Sabado => "Sábado",
            _ => "Domingo"
        };
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
