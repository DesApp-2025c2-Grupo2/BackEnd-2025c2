using Application.Contracts.DTOs.Response;
using Domain.Entities;
using Application.Contracts.DTOs.Internal;
using Application.Contracts.DTOs.Internal.ReportData;
using System.Data;
using Domain.Enums;

namespace Application.Utilities;

public static class DTOMapper
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
                ProvinciaCiudad = d.ProvinciaCiudad
            }).ToList(),
            Documentacion = new DocumentacionDTO
            {
                id = persona.Documentacion.Id,
                tipoDocumento = (int)persona.Documentacion.TipoDocumento,
                numero = persona.Documentacion.Numero
            },
            SituacionesTerapeuticas = new HistorialTerapeuticoResponse()
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

}
