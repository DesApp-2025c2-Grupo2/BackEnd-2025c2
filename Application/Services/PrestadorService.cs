using Application.Contracts.DTOs.Request;
using Application.Contracts.DTOs.Response;
using Application.Contracts.Interfaces;
using Application.Utilities;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Enums;

namespace Application.Services;

//public class PrestadorService : IPrestadorService
//{
//    private readonly IPrestadorRepository prestadorRepo;

//    public PrestadorService(IPrestadorRepository prestadorRepo)
//    {
//        this.prestadorRepo = prestadorRepo;
//    }

//    public async Task<PrestadorResponse> CreateAsync(PrestadorResponse request)
//    {
//        var prestador = new Prestador
//        {
//            NombreCompleto = request.NombreCompleto,
//            Rol = (RolMedico)request.Rol,
//            CentroMedico = request.CentroMedico,
//            CentroId = request.CentroId,
//            Alta = DateTime.UtcNow
//        };

//        // Documentación
//        if (request.Documentacion != null && !string.IsNullOrWhiteSpace(request.Documentacion.numero))
//        {
//            prestador.Documentaciones = new List<Documentacion>
//            {
//                new Documentacion
//                {
//                    TipoDocumento = (TipoDocumento)request.Documentacion.tipoDocumento,
//                    Numero = request.Documentacion.numero
//                }
//            };
//        }

//        // Teléfonos
//        prestador.Telefonos = request.Telefonos?
//            .Where(t => !string.IsNullOrWhiteSpace(t.Numero))
//            .Select(t => new Telefono { Numero = t.Numero })
//            .ToList() ?? new();

//        // Emails
//        prestador.Emails = request.Emails?
//            .Where(e => !string.IsNullOrWhiteSpace(e.Correo))
//            .Select(e => new Email { Correo = e.Correo })
//            .ToList() ?? new();

//        // Direcciones
//        prestador.Direcciones = request.Direcciones?
//            .Where(d => !string.IsNullOrWhiteSpace(d.Calle))
//            .Select(d => new Direccion
//            {
//                Calle = d.Calle,
//                Altura = string.IsNullOrWhiteSpace(d.Altura) ? "S/N" : d.Altura,
//                Piso = d.Piso,
//                Departamento = d.Departamento,
//                ProvinciaCiudad = string.IsNullOrWhiteSpace(d.ProvinciaCiudad) ? "S/D" : d.ProvinciaCiudad,
//                CodigoPostal = d.CodigoPostal
//            })
//            .ToList() ?? new();

//        if (request.Especialidades != null && request.Especialidades.Count > 0)
//        {
//            var especialidades = await prestadorRepo.GetEspecialidadesByIdsAsync(request.Especialidades);
//            prestador.Especialidades = especialidades;
//        }

//        await prestadorRepo.AddAsync(prestador);
//        await prestadorRepo.SaveChangesAsync();

//        var guardado = await prestadorRepo.GetByIdWithDetailsAsync(prestador.Id) ?? prestador;
//        return MapPrestadorToResponse(guardado);
//    }

//    public async Task<IEnumerable<PrestadorResponse>> GetAllAsync()
//    {
//        var lista = await prestadorRepo.GetAllAsync();
//        return lista.Select(MapPrestadorToResponse);
//    }

//    public async Task<PrestadorResponse> GetByIdAsync(int id)
//    {
//        var prestador = await prestadorRepo.GetByIdWithDetailsAsync(id);
//        if (prestador == null) throw new Exception("Prestador no encontrado");
//        return MapPrestadorToResponse(prestador);
//    }

//    public async Task<PrestadorResponse> UpdateAsync(int id, PrestadorResponse request)
//    {
//        var prestador = await prestadorRepo.GetByIdWithDetailsAsync(id) ?? throw new Exception("Prestador no encontrado");

//        prestador.NombreCompleto = request.NombreCompleto;
//        prestador.Rol = (RolMedico)request.Rol;
//        prestador.CentroMedico = request.CentroMedico;
//        prestador.CentroId = request.CentroId;

//        // Documentación: actualizar/agregar si viene en el request. No borrar si no viene.
//        if (request.Documentacion != null && !string.IsNullOrWhiteSpace(request.Documentacion.numero))
//        {
//            var docCuit = prestador.Documentaciones?.FirstOrDefault(d => d.TipoDocumento == TipoDocumento.CUIT);
//            if (docCuit == null)
//            {
//                if (prestador.Documentaciones == null) prestador.Documentaciones = new List<Documentacion>();
//                prestador.Documentaciones.Add(new Documentacion
//                {
//                    TipoDocumento = (TipoDocumento)request.Documentacion.tipoDocumento,
//                    Numero = request.Documentacion.numero
//                });
//            }
//            else
//            {
//                docCuit.Numero = request.Documentacion.numero;
//            }
//        }

//        // Teléfonos: sincronizar con IDs
//        var nuevosTelefonos = new List<Telefono>();
//        var telefonosExistentes = prestador.Telefonos?.ToDictionary(t => t.Id) ?? new Dictionary<int, Telefono>();
//        foreach (var tDto in request.Telefonos ?? new List<TelefonoDTO>())
//        {
//            if (tDto.Id > 0 && telefonosExistentes.TryGetValue(tDto.Id, out var existente))
//            {
//                existente.Numero = tDto.Numero;
//                nuevosTelefonos.Add(existente);
//            }
//            else if (!string.IsNullOrWhiteSpace(tDto.Numero))
//            {
//                nuevosTelefonos.Add(new Telefono { Numero = tDto.Numero, PrestadorId = prestador.Id });
//            }
//        }
//        prestador.Telefonos = nuevosTelefonos;

//        // Emails: sincronizar con IDs
//        var nuevosEmails = new List<Email>();
//        var emailsExistentes = prestador.Emails?.ToDictionary(e => e.Id) ?? new Dictionary<int, Email>();
//        foreach (var eDto in request.Emails ?? new List<EmailDTO>())
//        {
//            if (eDto.Id > 0 && emailsExistentes.TryGetValue(eDto.Id, out var existente))
//            {
//                existente.Correo = eDto.Correo;
//                nuevosEmails.Add(existente);
//            }
//            else if (!string.IsNullOrWhiteSpace(eDto.Correo))
//            {
//                nuevosEmails.Add(new Email { Correo = eDto.Correo, PrestadorId = prestador.Id });
//            }
//        }
//        prestador.Emails = nuevosEmails;

//        // Direcciones: sincronizar con IDs
//        var nuevasDirecciones = new List<Direccion>();
//        var direccionesExistentes = prestador.Direcciones?.ToDictionary(d => d.Id) ?? new Dictionary<int, Direccion>();
//        foreach (var dDto in request.Direcciones ?? new List<DireccionDTO>())
//        {
//            if (dDto.Id.HasValue && dDto.Id.Value > 0 && direccionesExistentes.TryGetValue(dDto.Id.Value, out var existente))
//            {
//                // Guardar el texto anterior para poder propagar cambios a las agendas
//                var oldCalle = (existente.Calle ?? string.Empty).Trim();

//                existente.Calle = dDto.Calle;
//                existente.Altura = string.IsNullOrWhiteSpace(dDto.Altura) ? "S/N" : dDto.Altura;
//                existente.Piso = dDto.Piso;
//                existente.Departamento = dDto.Departamento;
//                existente.ProvinciaCiudad = string.IsNullOrWhiteSpace(dDto.ProvinciaCiudad) ? "S/D" : dDto.ProvinciaCiudad;
//                existente.CodigoPostal = dDto.CodigoPostal;
//                nuevasDirecciones.Add(existente);

//                // Si cambió el nombre de la calle, actualizar también las Agendas que usaban el texto viejo
//                var newCalle = (dDto.Calle ?? string.Empty).Trim();
//                if (!string.Equals(oldCalle, newCalle, StringComparison.OrdinalIgnoreCase))
//                {
//                    await prestadorRepo.UpdateDireccionTextoForProfesionalAsync(prestador.Id, oldCalle, newCalle);
//                }
//            }
//            else if (!string.IsNullOrWhiteSpace(dDto.Calle))
//            {
//                nuevasDirecciones.Add(new Direccion
//                {
//                    Calle = dDto.Calle,
//                    Altura = string.IsNullOrWhiteSpace(dDto.Altura) ? "S/N" : dDto.Altura,
//                    Piso = dDto.Piso,
//                    Departamento = dDto.Departamento,
//                    ProvinciaCiudad = string.IsNullOrWhiteSpace(dDto.ProvinciaCiudad) ? "S/D" : dDto.ProvinciaCiudad,
//                    CodigoPostal = dDto.CodigoPostal,
//                    PrestadorId = prestador.Id
//                });
//            }
//        }
//        prestador.Direcciones = nuevasDirecciones;

//        // Especialidades: REEMPLAZAR completamente con lo enviado
//        var nuevasEspecialidades = await prestadorRepo.GetEspecialidadesByIdsAsync(request.Especialidades ?? new List<int>());
//        prestador.Especialidades = nuevasEspecialidades;

//        await prestadorRepo.UpdateAsync(prestador);
//        await prestadorRepo.SaveChangesAsync();

//        var actualizado = await prestadorRepo.GetByIdWithDetailsAsync(id) ?? prestador;
//        return MapPrestadorToResponse(actualizado);
//    }

//    public async Task<PrestadorResponse> UpdateHorariosAsync(int id, PrestadorHorariosRequest request, string strategy = "merge")
//    {
//        var prestador = await prestadorRepo.GetByIdWithDetailsAsync(id) ?? throw new Exception("Prestador no encontrado");
//        var esCentro = prestador.Rol == RolMedico.CentroMedico;

//        // Eliminar explícitos por removeIds
//        if (request.RemoveIds != null && request.RemoveIds.Count > 0)
//        {
//            await prestadorRepo.DeleteHorariosByIdsAsync(request.RemoveIds.Distinct().ToList());
//        }

//        foreach (var dir in request.Direcciones)
//        {
//            var sourceHorarios = (dir.HorariosAtencion != null && dir.HorariosAtencion.Any()) ? dir.HorariosAtencion : dir.Horarios;

//            // Obtener siempre la dirección desde el lugar existente del prestador (no crear lugares al vuelo)
//            if (dir.Id == null || dir.Id <= 0)
//                throw new Exception("Debe especificar un lugarId (Id de dirección) válido para cada grupo de horarios.");

//            var direccionEntidad = prestador.Direcciones?.FirstOrDefault(d => d.Id == dir.Id.Value)
//                                   ?? throw new Exception($"Lugar con Id {dir.Id.Value} no encontrado para el prestador.");

//            var direccionTexto = (direccionEntidad.Calle ?? string.Empty).Trim();

//            // strategy=replace => borrar horarios actuales de todas las agendas en esa direccion para el profesional
//            if (!string.IsNullOrWhiteSpace(strategy) && strategy.Equals("replace", StringComparison.OrdinalIgnoreCase))
//            {
//                var agendasParaDireccion = await prestadorRepo.GetAgendasByProfesionalAndDireccionAsync(prestador.Id, direccionTexto);
//                var agendaIds = agendasParaDireccion.Select(a => a.Id).ToList();
//                await prestadorRepo.DeleteAllHorariosByAgendaIdsAsync(agendaIds);
//            }

//            foreach (var h in sourceHorarios)
//            {
//                // Borrado por flag 'deleted' o por removeIds ya manejado arriba
//                if ((h.Deleted ?? false) && h.Id.HasValue && h.Id.Value > 0)
//                {
//                    await prestadorRepo.DeleteHorariosByIdsAsync(new List<int> { h.Id.Value });
//                    continue;
//                }

//                // Determinar el profesional al que se le asigna el horario:
//                // - Si el prestador es un Centro Médico => usar h.ProfesionalId (obligatorio)
//                // - Si es un profesional independiente => usar el propio prestador.Id
//                var profesionalId = esCentro
//                    ? h.ProfesionalId ?? throw new Exception("Debe especificar ProfesionalId para horarios de un centro médico.")
//                    : prestador.Id;

//                var dias = (h.DiasDeLaSemana != null && h.DiasDeLaSemana.Any()) ? h.DiasDeLaSemana : new List<int> { h.DiaSemana };
//                if (dias.Count == 0) continue;

//                var inicio = TimeSpan.Parse(h.HoraInicio);
//                var fin = TimeSpan.Parse(h.HoraFin);
//                if (fin <= inicio) continue;

//                var duracion = h.DuracionMinutos ?? dir.DuracionConsulta ?? 30;
//                var especialidades = (h.Especialidades ?? new List<int>()).Where(e => e > 0).Distinct().ToList();
//                if (especialidades.Count == 0) especialidades = new List<int> { 0 };

//                foreach (var especialidadId in especialidades)
//                {
//                    // Asegurar agenda por profesional+especialidad+direccion
//                    var agenda = await prestadorRepo.GetAgendaAsync(profesionalId, especialidadId, direccionTexto);
//                    if (agenda == null)
//                    {
//                        agenda = new Agenda
//                        {
//                            ProfesionalId = profesionalId,
//                            EspecialidadId = especialidadId,
//                            Direccion = direccionTexto,
//                            DuracionConsulta = duracion,
//                            Alta = DateTime.UtcNow
//                        };
//                        await prestadorRepo.AddAgendaAsync(agenda);
//                        await prestadorRepo.SaveChangesAsync();
//                    }

//                    if (h.Id.HasValue && h.Id.Value > 0)
//                    {
//                        var anchor = await prestadorRepo.GetHorarioByIdAsync(h.Id.Value);
//                        if (anchor != null)
//                        {
//                            // Limpiar días/especialidades del tramo antiguo excepto el anchor
//                            var antiguos = await prestadorRepo.GetHorariosByAgendaAndTramoAsync(anchor.AgendaId, anchor.HoraInicio.TimeOfDay, anchor.HoraFin.TimeOfDay);
//                            var idsAEliminar = antiguos.Where(x => x.Id != anchor.Id).Select(x => x.Id).ToList();
//                            await prestadorRepo.DeleteHorariosByIdsAsync(idsAEliminar);

//                            // Reubicar/actualizar anchor al nuevo destino (primer día, primera especialidad)
//                            anchor.AgendaId = agenda.Id;
//                            anchor.DiaDeAtencion = (DiaAtencion)dias.First();
//                            anchor.HoraInicio = DateTime.Today.Date.Add(inicio);
//                            anchor.HoraFin = DateTime.Today.Date.Add(fin);
//                            anchor.EspecialidadId = especialidadId;
//                            anchor.DuracionConsulta = duracion;
//                            await prestadorRepo.UpdateHorarioAsync(anchor);

//                            // Crear filas restantes (resto de días)
//                            var restantes = dias.Skip(1).ToList();
//                            var nuevos = new List<HorarioAtencion>();
//                            foreach (var dia in restantes)
//                            {
//                                nuevos.Add(new HorarioAtencion
//                                {
//                                    AgendaId = agenda.Id,
//                                    DiaDeAtencion = (DiaAtencion)dia,
//                                    HoraInicio = DateTime.Today.Date.Add(inicio),
//                                    HoraFin = DateTime.Today.Date.Add(fin),
//                                    EspecialidadId = especialidadId,
//                                    DuracionConsulta = duracion,
//                                    Alta = DateTime.UtcNow
//                                });
//                            }
//                            await prestadorRepo.AddHorariosAsync(nuevos);
//                        }
//                        else
//                        {
//                            // No existe: alta nueva normal
//                            var nuevos = new List<HorarioAtencion>();
//                            foreach (var dia in dias)
//                            {
//                                nuevos.Add(new HorarioAtencion
//                                {
//                                    AgendaId = agenda.Id,
//                                    DiaDeAtencion = (DiaAtencion)dia,
//                                    HoraInicio = DateTime.Today.Date.Add(inicio),
//                                    HoraFin = DateTime.Today.Date.Add(fin),
//                                    EspecialidadId = especialidadId,
//                                    DuracionConsulta = duracion,
//                                    Alta = DateTime.UtcNow
//                                });
//                            }
//                            await prestadorRepo.AddHorariosAsync(nuevos);
//                        }
//                    }
//                    else
//                    {
//                        // Alta nueva del tramo
//                        var nuevos = new List<HorarioAtencion>();
//                        foreach (var dia in dias)
//                        {
//                            nuevos.Add(new HorarioAtencion
//                            {
//                                AgendaId = agenda.Id,
//                                DiaDeAtencion = (DiaAtencion)dia,
//                                HoraInicio = DateTime.Today.Date.Add(inicio),
//                                HoraFin = DateTime.Today.Date.Add(fin),
//                                EspecialidadId = especialidadId,
//                                DuracionConsulta = duracion,
//                                Alta = DateTime.UtcNow
//                            });
//                        }
//                        await prestadorRepo.AddHorariosAsync(nuevos);
//                    }
//                }
//            }
//        }

//        var actualizado = await prestadorRepo.GetByIdWithDetailsAsync(id) ?? throw new Exception("Prestador no encontrado");
//        return MapPrestadorToResponse(actualizado);
//    }

//    public async Task<PrestadorEstadoResponse> UpdateEstadoAsync(int id, PrestadorEstadoRequest request)
//    {
//        var prestador = await prestadorRepo.GetByIdAsync(id);
//        if (prestador == null)
//            throw new Exception("Prestador no encontrado");

//        if (request.Activo)
//        {
//            prestador.Baja = null;
//            if (prestador.Alta == default)
//                prestador.Alta = DateTime.UtcNow;
//        }
//        else
//        {
//            prestador.Baja = DateTime.UtcNow;
//        }

//        await prestadorRepo.UpdateAsync(prestador);
//       // await prestadorRepo.SaveChangesAsync();

//        return new PrestadorEstadoResponse
//        {
//            Id = prestador.Id,
//            Activo = prestador.Baja == null
//        };
//    }

//    private PrestadorResponse MapPrestadorToResponse(Prestador p)
//    {
//        return new PrestadorResponse
//        {
//            Id = p.Id,
//            NombreCompleto = p.NombreCompleto,
//            Rol = (int)p.Rol,
//            CentroMedico = p.CentroMedico,
//            ProfesionalesIds = p.Profesionales?.Select(pr => pr.Id).ToList() ?? new List<int>(),
//            Activo = p.Baja == null || (p.Baja.HasValue && p.Baja.Value.Date > DateTime.Now.Date),
//            Especialidades = p.Especialidades?.Select(e => e.Id).ToList() ?? new(),
//            Documentacion = p.Documentaciones != null && p.Documentaciones.Any() ? new DocumentacionDTO
//            {
//                id = p.Documentaciones.First().Id,
//                tipoDocumento = (int)p.Documentaciones.First().TipoDocumento,
//                numero = p.Documentaciones.First().Numero
//            } : null,
//            Telefonos = p.Telefonos?.Select(t => new TelefonoDTO { Id = t.Id, Numero = t.Numero }).ToList() ?? new(),
//            Emails = p.Emails?.Select(e => new EmailDTO { Id = e.Id, Correo = e.Correo }).ToList() ?? new(),
//            Direcciones = p.Direcciones?.Select(d => new DireccionDTO
//            {
//                Id = d.Id,
//                Calle = d.Calle,
//                Altura = d.Altura,
//                Piso = d.Piso,
//                Departamento = d.Departamento,
//                ProvinciaCiudad = d.ProvinciaCiudad,
//                CodigoPostal = d.CodigoPostal
//            }).ToList() ?? new()
//        };
//    }
//}


public class PrestadorService : IPrestadorService
{
    private readonly IPrestadorRepository prestadorRepo;

    public PrestadorService(IPrestadorRepository prestadorRepo)
    {
        this.prestadorRepo = prestadorRepo;
    }

    public async Task<PrestadoresResponse> GetAllAsync()
    {
        PrestadoresResponse response = new();
        List<Prestador> prestadoresE = await prestadorRepo.GetAllAsync();
        prestadoresE.ForEach(pr => response.Add(DTOMapper.PrestadorToResponse(pr)));
        return response;
    }

    public async Task<bool> ToggleStatusAsync(int id)
    {
        return await prestadorRepo.ToggleStatusAsync(id);
    }

    public async Task<PrestadorResponse> SaveAsync(PrestadorRequest prestadorRequest)
    {
        PrestadorResponse response;
        Prestador prestadorMapped = DTOMapper.PrestadorToEntity(prestadorRequest);
        Prestador prestadorDB;
        if (prestadorRequest.Id.HasValue && prestadorRequest.Id.Value > 0) prestadorDB = await prestadorRepo.UpdateAsync(prestadorMapped, prestadorRequest.Especialidades);
        else prestadorDB = await prestadorRepo.CreateAsync(prestadorMapped, prestadorRequest.Especialidades);
        response = DTOMapper.PrestadorToResponse(prestadorDB);
        return response;
    }
}