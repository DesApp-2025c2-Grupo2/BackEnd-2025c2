using Application.Contracts.DTOs.Internal;
using Application.Contracts.DTOs.Request;
using Application.Contracts.DTOs.Response;
using Application.Contracts.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;

namespace Application.Services;

public class PrestadorService : IPrestadorService
{
    private readonly IPrestadorRepository prestadorRepo;

    public PrestadorService(IPrestadorRepository prestadorRepo)
    {
        this.prestadorRepo = prestadorRepo;
    }

    public async Task<PrestadorResponse> CreateAsync(PrestadorRequest request)
    {
        var prestador = new Prestador
        {
            NombreCompleto = request.NombreCompleto,
            Rol = (RolMedico)request.Rol,
            CentroMedico = request.CentroMedico,
            Alta = DateTime.UtcNow
        };

        if (!string.IsNullOrWhiteSpace(request.Documentacion))
        {
            prestador.Documentaciones = new List<Documentacion>
            {
                new Documentacion
                {
                    TipoDocumento = TipoDocumento.CUIT,
                    Numero = request.Documentacion
                }
            };
        }

        prestador.Telefonos = request.Telefonos?.Select(t => new Telefono { Numero = t }).ToList() ?? new();
        prestador.Emails = request.Emails?.Select(e => new Email { Correo = e }).ToList() ?? new();
        prestador.Direcciones = request.Direcciones?.Select(d => new Direccion { Calle = d, Altura = "S/N", ProvinciaCiudad = "S/D" }).ToList() ?? new();

        if (request.EspecialidadesIds != null && request.EspecialidadesIds.Count > 0)
        {
            var especialidades = await prestadorRepo.GetEspecialidadesByIdsAsync(request.EspecialidadesIds);
            prestador.Especialidades = especialidades;
        }

        await prestadorRepo.AddAsync(prestador);
        await prestadorRepo.SaveChangesAsync();

        var guardado = await prestadorRepo.GetByIdWithDetailsAsync(prestador.Id) ?? prestador;
        return MapPrestadorToResponse(guardado);
    }

    public async Task<IEnumerable<PrestadorResponse>> GetAllAsync()
    {
        var lista = await prestadorRepo.GetAllAsync();
        return lista.Select(MapPrestadorToResponse);
    }

    public async Task<PrestadorResponse> GetByIdAsync(int id)
    {
        var prestador = await prestadorRepo.GetByIdWithDetailsAsync(id);
        if (prestador == null) throw new Exception("Prestador no encontrado");
        return MapPrestadorToResponse(prestador);
    }

    public async Task<PrestadorResponse> UpdateAsync(int id, PrestadorRequest request)
    {
        var prestador = await prestadorRepo.GetByIdWithDetailsAsync(id) ?? throw new Exception("Prestador no encontrado");

        prestador.NombreCompleto = request.NombreCompleto;
        prestador.Rol = (RolMedico)request.Rol;
        prestador.CentroMedico = request.CentroMedico;

        // Documentación: sólo actualizar/agregar si viene en el request. No borrar lo existente.
        if (!string.IsNullOrWhiteSpace(request.Documentacion))
        {
            var docCuit = prestador.Documentaciones?.FirstOrDefault(d => d.TipoDocumento == TipoDocumento.CUIT);
            if (docCuit == null)
            {
                if (prestador.Documentaciones == null) prestador.Documentaciones = new List<Documentacion>();
                prestador.Documentaciones.Add(new Documentacion { TipoDocumento = TipoDocumento.CUIT, Numero = request.Documentacion });
            }
            else
            {
                docCuit.Numero = request.Documentacion;
            }
        }

        // Teléfonos: REEMPLAZAR completamente con lo enviado
        prestador.Telefonos = request.Telefonos?.Where(n => !string.IsNullOrWhiteSpace(n))
            .Select(n => new Telefono { Numero = n })
            .ToList() ?? new List<Telefono>();

        // Emails: REEMPLAZAR completamente con lo enviado
        prestador.Emails = request.Emails?.Where(c => !string.IsNullOrWhiteSpace(c))
            .Select(c => new Email { Correo = c })
            .ToList() ?? new List<Email>();

        // Direcciones: REEMPLAZAR completamente con lo enviado
        prestador.Direcciones = request.Direcciones?.Where(d => !string.IsNullOrWhiteSpace(d))
            .Select(d => new Direccion { Calle = d, Altura = "S/N", ProvinciaCiudad = "S/D" })
            .ToList() ?? new List<Direccion>();

        // Especialidades: REEMPLAZAR completamente con lo enviado
        var nuevasEspecialidades = await prestadorRepo.GetEspecialidadesByIdsAsync(request.EspecialidadesIds ?? new List<int>());
        prestador.Especialidades = nuevasEspecialidades;

        await prestadorRepo.UpdateAsync(prestador);
        await prestadorRepo.SaveChangesAsync();

        var actualizado = await prestadorRepo.GetByIdWithDetailsAsync(id) ?? prestador;
        return MapPrestadorToResponse(actualizado);
    }

    public async Task<PrestadorResponse> UpdateHorariosAsync(int id, PrestadorHorariosRequest request, string strategy = "merge")
    {
        var prestador = await prestadorRepo.GetByIdWithDetailsAsync(id) ?? throw new Exception("Prestador no encontrado");

        // Eliminar explícitos por removeIds
        if (request.RemoveIds != null && request.RemoveIds.Count > 0)
        {
            await prestadorRepo.DeleteHorariosByIdsAsync(request.RemoveIds.Distinct().ToList());
        }

        foreach (var dir in request.Direcciones)
        {
            var sourceHorarios = (dir.HorariosAtencion != null && dir.HorariosAtencion.Any()) ? dir.HorariosAtencion : dir.Horarios;

            // Asegurar lugar (Dirección) estable: si no existe y viene sin Id, crear en Prestador.Direcciones
            var direccionTexto = (dir.Direccion ?? string.Empty).Trim();
            if (dir.Id == null || dir.Id <= 0)
            {
                var existeLugar = prestador.Direcciones?.Any(d => (d.Calle ?? string.Empty).Trim() == direccionTexto) == true;
                if (!existeLugar && !string.IsNullOrWhiteSpace(direccionTexto))
                {
                    prestador.Direcciones.Add(new Direccion
                    {
                        Calle = direccionTexto,
                        Altura = "S/N",
                        ProvinciaCiudad = "S/D"
                    });
                    await prestadorRepo.UpdateAsync(prestador);
                }
            }

            // strategy=replace => borrar horarios actuales de todas las agendas en esa direccion para el profesional
            if (!string.IsNullOrWhiteSpace(strategy) && strategy.Equals("replace", StringComparison.OrdinalIgnoreCase))
            {
                var agendasParaDireccion = await prestadorRepo.GetAgendasByProfesionalAndDireccionAsync(prestador.Id, direccionTexto);
                var agendaIds = agendasParaDireccion.Select(a => a.Id).ToList();
                await prestadorRepo.DeleteAllHorariosByAgendaIdsAsync(agendaIds);
            }

            foreach (var h in sourceHorarios)
            {
                // Borrado por flag 'deleted' o por removeIds ya manejado arriba
                if ((h.Deleted ?? false) && h.Id.HasValue && h.Id.Value > 0)
                {
                    await prestadorRepo.DeleteHorariosByIdsAsync(new List<int> { h.Id.Value });
                    continue;
                }

                var dias = (h.DiasDeLaSemana != null && h.DiasDeLaSemana.Any()) ? h.DiasDeLaSemana : new List<int> { h.DiaSemana };
                if (dias.Count == 0) continue;

                var inicio = TimeSpan.Parse(h.HoraInicio);
                var fin = TimeSpan.Parse(h.HoraFin);
                if (fin <= inicio) continue;

                var duracion = h.DuracionMinutos ?? dir.DuracionConsulta ?? 30;
                var especialidades = (h.Especialidades ?? new List<int>()).Where(e => e > 0).Distinct().ToList();
                if (especialidades.Count == 0) especialidades = new List<int> { 0 };

                foreach (var especialidadId in especialidades)
                {
                    // Asegurar agenda por profesional+especialidad+direccion
                    var agenda = await prestadorRepo.GetAgendaAsync(prestador.Id, especialidadId, direccionTexto);
                    if (agenda == null)
                    {
                        agenda = new Agenda
                        {
                            ProfesionalId = prestador.Id,
                            EspecialidadId = especialidadId,
                            Direccion = direccionTexto,
                            DuracionConsulta = duracion,
                            Alta = DateTime.UtcNow
                        };
                        await prestadorRepo.AddAgendaAsync(agenda);
                        await prestadorRepo.SaveChangesAsync();
                    }

                    if (h.Id.HasValue && h.Id.Value > 0)
                    {
                        var anchor = await prestadorRepo.GetHorarioByIdAsync(h.Id.Value);
                        if (anchor != null)
                        {
                            // Limpiar días/especialidades del tramo antiguo excepto el anchor
                            var antiguos = await prestadorRepo.GetHorariosByAgendaAndTramoAsync(anchor.AgendaId, anchor.HoraInicio.TimeOfDay, anchor.HoraFin.TimeOfDay);
                            var idsAEliminar = antiguos.Where(x => x.Id != anchor.Id).Select(x => x.Id).ToList();
                            await prestadorRepo.DeleteHorariosByIdsAsync(idsAEliminar);

                            // Reubicar/actualizar anchor al nuevo destino (primer día, primera especialidad)
                            anchor.AgendaId = agenda.Id;
                            anchor.DiaDeAtencion = (DiaAtencion)dias.First();
                            anchor.HoraInicio = DateTime.Today.Date.Add(inicio);
                            anchor.HoraFin = DateTime.Today.Date.Add(fin);
                            anchor.EspecialidadId = especialidadId;
                            anchor.DuracionConsulta = duracion;
                            await prestadorRepo.UpdateHorarioAsync(anchor);

                            // Crear filas restantes (resto de días)
                            var restantes = dias.Skip(1).ToList();
                            var nuevos = new List<HorarioAtencion>();
                            foreach (var dia in restantes)
                            {
                                nuevos.Add(new HorarioAtencion
                                {
                                    AgendaId = agenda.Id,
                                    DiaDeAtencion = (DiaAtencion)dia,
                                    HoraInicio = DateTime.Today.Date.Add(inicio),
                                    HoraFin = DateTime.Today.Date.Add(fin),
                                    EspecialidadId = especialidadId,
                                    DuracionConsulta = duracion,
                                    Alta = DateTime.UtcNow
                                });
                            }
                            await prestadorRepo.AddHorariosAsync(nuevos);
                        }
                        else
                        {
                            // No existe: alta nueva normal
                            var nuevos = new List<HorarioAtencion>();
                            foreach (var dia in dias)
                            {
                                nuevos.Add(new HorarioAtencion
                                {
                                    AgendaId = agenda.Id,
                                    DiaDeAtencion = (DiaAtencion)dia,
                                    HoraInicio = DateTime.Today.Date.Add(inicio),
                                    HoraFin = DateTime.Today.Date.Add(fin),
                                    EspecialidadId = especialidadId,
                                    DuracionConsulta = duracion,
                                    Alta = DateTime.UtcNow
                                });
                            }
                            await prestadorRepo.AddHorariosAsync(nuevos);
                        }
                    }
                    else
                    {
                        // Alta nueva del tramo
                        var nuevos = new List<HorarioAtencion>();
                        foreach (var dia in dias)
                        {
                            nuevos.Add(new HorarioAtencion
                            {
                                AgendaId = agenda.Id,
                                DiaDeAtencion = (DiaAtencion)dia,
                                HoraInicio = DateTime.Today.Date.Add(inicio),
                                HoraFin = DateTime.Today.Date.Add(fin),
                                EspecialidadId = especialidadId,
                                DuracionConsulta = duracion,
                                Alta = DateTime.UtcNow
                            });
                        }
                        await prestadorRepo.AddHorariosAsync(nuevos);
                    }
                }
            }
        }

        var actualizado = await prestadorRepo.GetByIdWithDetailsAsync(id) ?? throw new Exception("Prestador no encontrado");
        return MapPrestadorToResponse(actualizado);
    }

    public async Task<PrestadorEstadoResponse> UpdateEstadoAsync(int id, PrestadorEstadoRequest request)
    {
        var prestador = await prestadorRepo.GetByIdAsync(id);
        if (prestador == null)
            throw new Exception("Prestador no encontrado");

        if (request.Activo)
        {
            prestador.Baja = null;
            if (prestador.Alta == default)
                prestador.Alta = DateTime.UtcNow;
        }
        else
        {
            prestador.Baja = DateTime.UtcNow;
        }

        await prestadorRepo.UpdateAsync(prestador);
       // await prestadorRepo.SaveChangesAsync();

        return new PrestadorEstadoResponse
        {
            Id = prestador.Id,
            Activo = prestador.Baja == null
        };
    }

    private PrestadorResponse MapPrestadorToResponse(Prestador p)
    {
        return new PrestadorResponse
        {
            Id = p.Id,
            NombreCompleto = p.NombreCompleto,
            Rol = (int)p.Rol,
            CentroMedico = p.CentroMedico,
            Activo = p.Baja == null || (p.Baja.HasValue && p.Baja.Value.Date > DateTime.Now.Date),
            Especialidades = p.Especialidades?.Select(e => e.Id).ToList() ?? new(),
            Documentacion = p.Documentaciones != null && p.Documentaciones.Any() ? new DocumentacionDTO
            {
                id = p.Documentaciones.First().Id,
                tipoDocumento = (int)p.Documentaciones.First().TipoDocumento,
                numero = p.Documentaciones.First().Numero
            } : null,
            Telefonos = p.Telefonos?.Select(t => new TelefonoDTO { Id = t.Id, Numero = t.Numero }).ToList() ?? new(),
            Emails = p.Emails?.Select(e => new EmailDTO { Id = e.Id, Correo = e.Correo }).ToList() ?? new(),
            Direcciones = p.Direcciones?.Select(d => new DireccionDTO
            {
                Id = d.Id,
                Calle = d.Calle,
                Altura = d.Altura,
                Piso = d.Piso,
                Departamento = d.Departamento,
                ProvinciaCiudad = d.ProvinciaCiudad
            }).ToList() ?? new()
        };
    }
}


