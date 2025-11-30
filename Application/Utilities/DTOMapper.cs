using Application.Contracts.DTOs.Internal;
using Application.Contracts.DTOs.Request;
using Application.Contracts.DTOs.Response;
using Domain.DataModels;
using Domain.Entities;
using Domain.Enums;
using System.Data;

namespace Application.Utilities;

public static class DTOMapper
{
    public static ReportDataList<T> ToReportDataList<T>(this DataTable table) where T : ReportDataRow, new()
    {
        var list = new ReportDataList<T>();

        foreach (DataRow row in table.Rows)
        {
            var item = new T();

            // Mapeo automático por nombre de propiedad
            foreach (var prop in typeof(T).GetProperties())
            {
                if (!table.Columns.Contains(prop.Name)) continue;

                var value = row[prop.Name];

                if (value == DBNull.Value) value = null;

                // Asignación reflejada
                prop.SetValue(item, value);
            }

            list.Add(item);
        }

        return list;
    }

    #region Responses
    public static AfiliadoResponse AfiliadoToDTO(Afiliado afiliado)
    {
        AfiliadoResponse dto = new AfiliadoResponse
        {
            Id = afiliado.Id,
            NumeroAfiliado = afiliado.NumeroAfiliado,
            TitularID = afiliado.TitularID,
            PlanMedicoId = afiliado.PlanMedicoId,
            Alta = afiliado.Alta,
            Baja = afiliado.Baja,
            Integrantes = new PersonasResponse()
        };
        afiliado.Integrantes?.ForEach(p =>
            dto.Integrantes.Add(PersonaToDTO(p))
        );
        return dto;
    }

    public static PersonaResponse PersonaToDTO(Persona persona)
    {
        PersonaResponse dto = new PersonaResponse
        {
            Id = persona.Id,
            NumeroIntegrante = persona.NumeroIntegrante,
            Nombre = persona.Nombre,
            Apellido = persona.Apellido,
            FechaNacimiento = persona.FechaNacimiento,
            Parentesco = (int)persona.Parentesco,
            Alta = persona.Alta,
            Baja = persona.Baja,
            Telefonos = persona.Telefonos?.Select(t => new TelefonoDTO
            {
                Id = t.Id,
                Numero = t.Numero,
            }).ToList(),
            Emails = persona.Emails?.Select(e => new EmailDTO
            {
                Id = e.Id,
                Correo = e.Correo,
            }).ToList(),
            Direcciones = persona.Direcciones?.Select(d => new DireccionDTO
            {
                Id = d.Id,
                Calle = d.Calle,
                Altura = d.Altura,
                Piso = d.Piso,
                Departamento = d.Departamento,
                ProvinciaCiudad = d.ProvinciaCiudad,
                CodigoPostal = d.CodigoPostal,
            }).ToList(),
            Documentacion = new DocumentacionDTO
            {
                id = persona.Documentacion.Id,
                tipoDocumento = persona.Documentacion.TipoDocumento,
                numero = persona.Documentacion.Numero
            },
            SituacionesTerapeuticas = new HistorialTerapeuticoResponse()
        };
        persona.SituacionesTerapeuticas?.ForEach(s =>
        dto.SituacionesTerapeuticas.Add(new RegistroTerapeuticoResponse
        {
            id = s.SituacionTerapeutica?.Id ?? 0, // or another default value
            nombre = s.SituacionTerapeutica?.Nombre,
            fechaFin = s.FechaFin
        }));
        return dto;
    }

    public static PrestadorResponse PrestadorToResponse(Prestador prestador)
    {
        Documentacion? firstDoc = prestador.Documentacion.FirstOrDefault();
        PrestadorResponse dto = new PrestadorResponse
        {
            Id = prestador.Id,
            NombreCompleto = prestador.NombreCompleto,
            Documentacion = firstDoc != null ? DocumentacionToDTO(firstDoc) : throw new Exception("El prestador no tiene documentacion asignada"),
            Emails = prestador.Emails.Select(e => EmailToDTO(e)).ToList(),
            Telefonos = prestador.Telefonos.Select(t => TelefonoToDTO(t)).ToList(),
            Especialidades = new(),
            Direcciones = prestador.Direcciones.Select(d => DireccionToDTO(d)).ToList(),
            Alta = DateOnly.FromDateTime(prestador.Alta),
            Baja = prestador.Baja.HasValue ? DateOnly.FromDateTime(prestador.Baja.Value) : null,
            Rol = prestador is CentroMedico ? RolMedico.CentroMedico : RolMedico.ProfesionalIndependiente,
            Agendas = new AgendasResponse()
        };

        prestador.Especialidades?.ForEach(es => dto.Especialidades.Add(EspecialidadToDTOSimple(es)));
        (prestador as Profesional)?.Agendas?.ForEach(ag => dto.Agendas.Add(AgendaToDTO(ag)));
        (prestador as CentroMedico)?.Agendas?.ForEach(ag => dto.Agendas.Add(AgendaToDTO(ag)));

        return dto;
    }

    public static AgendaResponse AgendaToDTO(Agenda a)
    {
        return new AgendaResponse
        {
            Id = a.Id,
            DireccionAtencion = $"{a.DireccionAtencion.Calle} {a.DireccionAtencion.Altura}, CP: {a.DireccionAtencion.CodigoPostal}, {a.DireccionAtencion.ProvinciaCiudad}",
            Horarios = a.Horarios.Select(h => HorarioAtencionToDTO(h)).ToList()
        };
    }

    private static HorarioAtencionDTO HorarioAtencionToDTO(HorarioAtencion h)
    {
        return new HorarioAtencionDTO
        {
            Id = h.Id,
            DiasAtencion = DiasAtencionToDTO(h.DiasAtencion),
            HoraInicio = TimeOnly.FromDateTime(h.HoraInicio),
            HoraFin = TimeOnly.FromDateTime(h.HoraFin),
            DuracionMinutos = h.DuracionConsultaMinutos,
            Especialidad = EspecialidadToDTOSimple(h.Especialidad),
            ProfesionalAsignado = ProfesionalToDTO(h.ProfesionalAsignado)
        };
    }

    public static List<HorarioDiaDTO> DiasAtencionToDTO(List<HorarioDia> diasAtencion)
    {
        return diasAtencion.Select(dia => new HorarioDiaDTO
        {
            Id = dia.Id,
            Dia = dia.Dia
        }).ToList();
    }

    public static ProfesionalDTO? ProfesionalToDTO(Profesional? profesionalAsignado)
    {
        ProfesionalDTO? dto = null;
        if (profesionalAsignado != null)
        {
            dto = new ProfesionalDTO
            {
                Id = profesionalAsignado.Id,
                NombreCompleto = profesionalAsignado.NombreCompleto
            };
        }
        return dto;
    }

    public static EspecialidadDTO EspecialidadToDTOSimple(Especialidad es)
    {
        return new EspecialidadDTO
        {
            Id = es.Id,
            Nombre = es.Nombre
        };
    }

    public static TelefonoDTO TelefonoToDTO(Telefono t)
    {
        return new TelefonoDTO
        {
            Id = t.Id,
            Numero = t.Numero
        };
    }

    public static DireccionDTO DireccionToDTO(Direccion d)
    {
        return new DireccionDTO
        {
            Id = d.Id,
            Calle = d.Calle,
            Altura = d.Altura,
            Piso = d.Piso,
            Departamento = d.Departamento,
            ProvinciaCiudad = d.ProvinciaCiudad,
            CodigoPostal = d.CodigoPostal
        };
    }

    public static EmailDTO EmailToDTO(Email e)
    {
        return new EmailDTO
        {
            Id = e.Id,
            Correo = e.Correo
        };
    }

    public static DocumentacionDTO DocumentacionToDTO(Documentacion doc)
    {
        return new DocumentacionDTO
        {
            id = doc.Id,
            tipoDocumento = doc.TipoDocumento,
            numero = doc.Numero
        };
    }

    #endregion

    #region Entities
    public static Prestador PrestadorToEntity(PrestadorRequest prestadorRequest)
    {
        Prestador prestador;
        if (prestadorRequest.Rol == RolMedico.CentroMedico)
        {
            prestador = CentroToEntity(prestadorRequest);
        }
        else
        {
            prestador = ProfesionalToEntity(prestadorRequest);
        }
        prestador.Id = prestadorRequest.Id ?? 0;
        if (prestador is Profesional prof)
            prof.Agendas.ForEach(ag => ag.ProfesionalId = prestadorRequest.Id!.Value);
        else if (prestador is CentroMedico centro)
            centro.Agendas.ForEach(ag => ag.CentroMedicoId = prestadorRequest.Id!.Value);
        return prestador;
    }

    public static CentroMedico CentroToEntity(PrestadorRequest prestadorRequest)
    {
        CentroMedico centro = new CentroMedico()
        {
            NombreCompleto = prestadorRequest.NombreCompleto,
            RazonSocial = prestadorRequest.RazonSocial,
            Alta = prestadorRequest.Alta?.ToDateTime(TimeOnly.MinValue) ?? DateTime.Today,
            Direcciones = prestadorRequest.Direcciones.Select(dir => DireccionToEntity(dir)).ToList(),
            Emails = prestadorRequest.Emails.Select(e => EmailToEntity(e)).ToList(),
            Telefonos = prestadorRequest.Telefonos.Select(tel => TelefonoToEntity(tel)).ToList(),
            Documentacion = new List<Documentacion>() { DocumentacionToEntity(prestadorRequest.Documentacion) }
        };
        centro.Direcciones.ForEach(d => centro.Agendas.Add(new AgendaCentroMedico() 
        {
            DireccionId = d.Id,
            DireccionAtencion = d 
        }));
        return centro;
    }
    
    public static Profesional ProfesionalToEntity(PrestadorRequest prestadorRequest)
    {
        Profesional profesional = new Profesional
        {
            NombreCompleto = prestadorRequest.NombreCompleto,
            Matricula = prestadorRequest.Matricula ?? "N/D",
            CentroId = prestadorRequest.CentroId,
            Alta = prestadorRequest.Alta?.ToDateTime(TimeOnly.MinValue) ?? DateTime.Today,
            Direcciones = prestadorRequest.Direcciones.Select(d => DireccionToEntity(d)).ToList(),
            Emails = prestadorRequest.Emails.Select(e => EmailToEntity(e)).ToList(),
            Telefonos = prestadorRequest.Telefonos.Select(t => TelefonoToEntity(t)).ToList(),
            Documentacion = new List<Documentacion>() { DocumentacionToEntity(prestadorRequest.Documentacion) }
        };
        profesional.Direcciones.ForEach(d => profesional.Agendas.Add(new AgendaProfesional()
        {
            DireccionId = d.Id,
            DireccionAtencion = d 
        }));
        return profesional;
    }

    public static Documentacion DocumentacionToEntity(DocumentacionDTO documentacion) => new Documentacion()
    {
        Id = documentacion.id ?? 0,
        TipoDocumento = documentacion.tipoDocumento,
        Numero = documentacion.numero
    };

    public static Telefono TelefonoToEntity(TelefonoDTO tel) => new Telefono()
    {
        Id = tel.Id ?? 0,
        Numero = tel.Numero 
    };


    public static Email EmailToEntity(EmailDTO e) => new Email()
    {
        Id = e.Id ?? 0,
        Correo = e.Correo 
    };

    public static Direccion DireccionToEntity(DireccionDTO dir) => new Direccion()
    {
        Id = dir.Id ?? 0,
        Calle = dir.Calle,
        Altura = dir.Altura,
        Piso = dir.Piso,
        Departamento = dir.Departamento,
        ProvinciaCiudad = dir.ProvinciaCiudad,
        CodigoPostal = dir.CodigoPostal
    };

    public static Agenda AgendaToEntity(AgendaRequest request)
    {
        Agenda agenda;
        RolMedico rol = request.HorariosAtencion[0].ProfesionalAsignado == null ? RolMedico.ProfesionalIndependiente : RolMedico.CentroMedico;
        if (rol == RolMedico.ProfesionalIndependiente)
        {
            agenda = new AgendaProfesional
            {
                Id = request.Id,
                Horarios = request.HorariosAtencion.Select(h => HorarioAtencionToEntity(h)).ToList()
            };
        }
        else
        {
            agenda = new AgendaCentroMedico
            {
                Id = request.Id,
                Horarios = request.HorariosAtencion.Select(h => HorarioAtencionToEntity(h)).ToList()
            };
        }
        agenda.Horarios.ForEach(h => h.AgendaId = request.Id);
        return agenda;
    }

    public static HorarioAtencion HorarioAtencionToEntity(HorarioAtencionDTO h)
    {
        HorarioAtencion horario = new HorarioAtencion
        {
            Id = h.Id ?? 0,
            DiasAtencion = h.DiasAtencion.Select(d => new HorarioDia
            {
                Id = d.Id ?? 0,
                HorarioId = h.Id ?? 0,
                Dia = d.Dia
            }).ToList(),
            HoraInicio = new DateTime(1,1,1,h.HoraInicio.Hour,h.HoraInicio.Minute,h.HoraInicio.Second),
            HoraFin = new DateTime(1,1,1,h.HoraFin.Hour,h.HoraFin.Minute,h.HoraFin.Second),
            DuracionConsultaMinutos = h.DuracionMinutos,
            EspecialidadId = h.Especialidad.Id,
            ProfesionalAsignadoId = h.ProfesionalAsignado?.Id
        };
        return horario;
    }

    #endregion

}
