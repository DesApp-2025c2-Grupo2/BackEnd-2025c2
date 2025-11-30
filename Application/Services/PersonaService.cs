using Application.Contracts.DTOs.Request;
using Application.Contracts.DTOs.Response;
using Application.Contracts.Interfaces;
using Application.Utilities;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;

namespace Application.Services;

public class PersonaService : IPersonaService
{
    private readonly IPersonaRepository _personaRepo;

    public PersonaService(IPersonaRepository personaRepo)
    {
        _personaRepo = personaRepo;
    }

    public async Task<PersonaResponse> AddPersonAsync(PersonaRequest request)
    {
        Persona persona = new Persona
        {
            NumeroIntegrante = request.NumeroIntegrante,
            Nombre = request.Nombre,
            Apellido = request.Apellido,
            FechaNacimiento = request.FechaNacimiento,
            Parentesco = (Parentesco)request.Parentesco,
            AfiliadoId = request.AfiliadoId.HasValue ? (int)request.AfiliadoId.Value : 0, // Manejo de null
            Alta = request.Alta,
            Baja = request.Baja,
            Telefonos = request.Telefonos?.Select(t => new Telefono
            {
                Numero = t.Numero,
            }).ToList() ?? new List<Telefono>(),
            Emails = request.Emails?.Select(e => new Email
            {
                Correo = e.Correo,
            }).ToList() ?? new List<Email>(),
            Direcciones = request.Direcciones?.Select(d => new Direccion
            {
                Calle = d.Calle,
                Altura = d.Altura,
                Piso = d.Piso,
                Departamento = d.Departamento,
                ProvinciaCiudad = d.ProvinciaCiudad,
                CodigoPostal = d.CodigoPostal
            }).ToList() ?? new List<Direccion>(),
            Documentacion = new Documentacion
            {
                TipoDocumento = (Domain.Enums.TipoDocumento)request.Documentacion.tipoDocumento,
                Numero = request.Documentacion.numero,
            }
        };

        await _personaRepo.AddAsync(persona, request.SituacionesTerapeuticas);
        if (persona.Id != 0)
        {
            return DTOMapper.PersonaToDTO(persona);
        }
        else
        {
            throw new Exception("Error al guardar la persona");
        }
    }

    public async Task<PersonaResponse> GetByIdAsync(int id)
    {
        PersonaResponse response;
        var personaEntity = await _personaRepo.GetByIdAsync(id);
        if (personaEntity == null) throw new KeyNotFoundException("Familiar no encontrado.");
        response = DTOMapper.PersonaToDTO(personaEntity);
        return response;
    }

    public Task<bool> ToggleStatusAsync(int id, bool activo, DateTime? fecha)
    {
        return _personaRepo.ToggleStatusAsync(id, activo, fecha);
    }

    public async Task<PersonaResponse> UpdatePersonAsync(PersonaRequest request)
    {
        var persona = await _personaRepo.GetByIdAsync(request.Id ?? 0);
        if (persona == null) throw new Exception("Persona no encontrada");

        persona.NumeroIntegrante = request.NumeroIntegrante;
        persona.Nombre = request.Nombre;
        persona.Apellido = request.Apellido;
        persona.FechaNacimiento = request.FechaNacimiento;
        persona.Parentesco = (Parentesco)request.Parentesco;
        persona.AfiliadoId = request.AfiliadoId.HasValue ? (int)request.AfiliadoId.Value : 0; // Manejo de null
        persona.Alta = request.Alta;
        persona.Baja = request.Baja;
        persona.Telefonos = request.Telefonos?.Select(t => new Telefono { Numero = t.Numero }).ToList() ?? new List<Telefono>();
        persona.Emails = request.Emails?.Select(e => new Email { Correo = e.Correo }).ToList() ?? new List<Email>();
        persona.Direcciones = request.Direcciones?.Select(d => new Direccion
        {
            Calle = d.Calle,
            Altura = d.Altura,
            Piso = d.Piso,
            Departamento = d.Departamento,
            ProvinciaCiudad = d.ProvinciaCiudad,
            CodigoPostal = d.CodigoPostal
        }).ToList() ?? new List<Direccion>();
        persona.Documentacion = new Documentacion
        {
            TipoDocumento = (Domain.Enums.TipoDocumento)request.Documentacion.tipoDocumento,
            Numero = request.Documentacion.numero,
        };
        var personaActualizada = await _personaRepo.UpdateAsync(persona, request.SituacionesTerapeuticas);
        if (personaActualizada)
        {
            return DTOMapper.PersonaToDTO(persona);
        }
        else
        {
            throw new Exception("Error al actualizar la persona");
        }
    }

}
