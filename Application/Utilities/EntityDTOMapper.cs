using Application.Contracts.DTOs.Response;
using Domain.Entities;
using Application.Contracts.DTOs.Internal;

namespace Application.Utilities;

public static class EntityDTOMapper
{
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
            Integrantes = afiliado.Integrantes?.Select(p => new PersonaResponse
            {
                Id = p.Id,
                NumeroIntegrante = p.NumeroIntegrante,
                Nombre = p.Nombre,
                Apellido = p.Apellido,
                FechaNacimiento = p.FechaNacimiento,
                Parentesco = p.Parentesco,
                Alta = p.Alta,
                Baja = p.Baja,
                Documentacion = p.Documentacion != null ? new DocumentacionDTO
                {
                    id = p.Documentacion.Id,
                    tipoDocumento = (int)p.Documentacion.TipoDocumento,
                    numero = p.Documentacion.Numero
                } : null,
                Telefonos = p.Telefonos?.Select(t => new TelefonoDTO
                {
                    Id = t.Id,
                    Numero = t.Numero
                }).ToList(),
                Emails = p.Emails?.Select(e => new EmailDTO
                {
                    Id = e.Id,
                    Correo = e.Correo
                }).ToList(),
                Direcciones = p.Direcciones?.Select(d => new DireccionDTO
                {
                    Id = d.Id,
                    Calle = d.Calle,
                    Altura = d.Altura,
                    Piso = d.Piso,
                    Departamento = d.Departamento,
                    ProvinciaCiudad = d.ProvinciaCiudad
                }).ToList(),
                SituacionesTerapeuticas = (HistorialTerapeuticoResponse)(p.SituacionesTerapeuticas == null || p.SituacionesTerapeuticas.Count == 0 ? new HistorialTerapeuticoResponse() :
                p.SituacionesTerapeuticas.Select(reg => new RegistroTerapeuticoResponse
                {
                    id = reg.Id,
                    nombre = reg.SituacionTerapeutica.Nombre,
                    fechaFin = reg.FechaFin
                }).ToList())

            }).ToList() ?? new()
        };
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
            Parentesco = persona.Parentesco,
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
                ProvinciaCiudad = d.ProvinciaCiudad
            }).ToList(),
            Documentacion = persona.Documentacion != null ? new DocumentacionDTO
            {
                id = persona.Documentacion.Id,
                tipoDocumento = (int)persona.Documentacion.TipoDocumento,
                numero = persona.Documentacion.Numero
            } : null,
            SituacionesTerapeuticas = (HistorialTerapeuticoResponse)(persona.SituacionesTerapeuticas?.Select(s => new RegistroTerapeuticoResponse
            {
                id = s.Id,
                nombre = s.SituacionTerapeutica.Nombre,
                fechaFin = s.FechaFin
            }).ToList()?? new List<RegistroTerapeuticoResponse>())
        };
        persona.SituacionesTerapeuticas?.ForEach(s =>
            dto.SituacionesTerapeuticas.Add(new RegistroTerapeuticoResponse
            {
                id = s.Id,
                nombre = s.SituacionTerapeutica.Nombre,
                fechaFin = s.FechaFin
            })
        );
        return dto;
    }
}
