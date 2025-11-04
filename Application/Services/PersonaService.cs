using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Contracts.DTOs.Internal;
using Application.Contracts.DTOs.Request;
using Application.Contracts.DTOs.Response;
using Application.Contracts.Interfaces;
using Application.Utilities;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;

namespace Application.Services
{
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
                AfiliadoId = request.AfiliadoId,
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
                    ProvinciaCiudad = d.ProvinciaCiudad
                }).ToList() ?? new List<Direccion>(),
                Documentacion = new Documentacion
                {
                    TipoDocumento = (Domain.Enums.TipoDocumento)request.Documentacion.tipoDocumento,
                    Numero = request.Documentacion.numero,
                }
            };

            await _personaRepo.AddAsync(persona,request.SituacionesTerapeuticas);
            if (persona.Id!= 0)
            {
                return EntityDTOMapper.PersonaToDTO(persona);
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
            response = EntityDTOMapper.PersonaToDTO(personaEntity);
            return response;
        }

        public Task<bool> ToggleStatusAsync(int id, DateTime? fecha)
        {
            return _personaRepo.ToggleStatusAsync(id, fecha ?? DateTime.Now.Date);
        }

        public async Task<PersonaResponse> UpdatePersonAsync(int id, PersonaRequest request)
        {
            var persona = await _personaRepo.GetByIdAsync(id);
            if (persona == null) throw new Exception("Persona no encontrada");

            persona.NumeroIntegrante = request.NumeroIntegrante;
            persona.Nombre = request.Nombre;
            persona.Apellido = request.Apellido;
            persona.FechaNacimiento = request.FechaNacimiento;
            persona.Parentesco = (Parentesco)request.Parentesco;
            persona.AfiliadoId = request.AfiliadoId;
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
                ProvinciaCiudad = d.ProvinciaCiudad
            }).ToList() ?? new List<Direccion>();
            persona.Documentacion = new Documentacion
            {
                TipoDocumento = (Domain.Enums.TipoDocumento)request.Documentacion.tipoDocumento,
                Numero = request.Documentacion.numero,
            };
            await _personaRepo.UpdateAsync(persona, request.SituacionesTerapeuticas);
            return EntityDTOMapper.PersonaToDTO(persona);
        }

    }
}
