using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Contracts.DTOs.Internal;
using Application.Contracts.DTOs.Request;
using Application.Contracts.DTOs.Response;
using Application.Contracts.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class PersonaService : IPersonaService
    {
        private readonly IPersonaRepository _personaRepo;
        private readonly ISituacionTerapeuticaRepository _situacionRepo;

        public PersonaService(IPersonaRepository personaRepo, ISituacionTerapeuticaRepository situacionRepo)
        {
            _personaRepo = personaRepo;
            _situacionRepo = situacionRepo;
        }

        public async Task<PersonaResponse> CrearPersonaAsync(PersonaRequest request)
        {
            var persona = new Persona
            {
                NumeroIntegrante = request.NumeroIntegrante,
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                FechaNacimiento = request.FechaNacimiento,
                Parentesco = request.Parentesco,
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

            // Asociar las situaciones terapéuticas
            if (request.SituacionesTerapeuticasIds != null && request.SituacionesTerapeuticasIds.Any())
            {
                var situaciones = await _situacionRepo.GetByIdsAsync(request.SituacionesTerapeuticasIds);
                persona.SituacionesTerapeuticas = situaciones.ToList();
            }

            try
            {
                await _personaRepo.AddAsync(persona);
                await _personaRepo.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al guardar la persona", ex);
            }

            return MapToResponse(persona);
        }

        public async Task<PersonaResponse> GetByIdAsync(int id)
        {
            var persona = await _personaRepo.GetByIdAsync(id);
            if (persona == null) return null;

            return MapToResponse(persona);
        }

        public async Task<PersonaResponse> ActualizarPersonaAsync(int id, PersonaRequest request)
        {
            var persona = await _personaRepo.GetByIdAsync(id);
            if (persona == null) throw new Exception("Persona no encontrada");

            persona.NumeroIntegrante = request.NumeroIntegrante;
            persona.Nombre = request.Nombre;
            persona.Apellido = request.Apellido;
            persona.FechaNacimiento = request.FechaNacimiento;
            persona.Parentesco = request.Parentesco;
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

            // Asociar las situaciones terapéuticas correctamente
            if (request.SituacionesTerapeuticasIds != null && request.SituacionesTerapeuticasIds.Any())
            {
                var situaciones = await _situacionRepo.GetByIdsAsync(request.SituacionesTerapeuticasIds);
                persona.SituacionesTerapeuticas = situaciones.ToList();
            }
            else
            {
                persona.SituacionesTerapeuticas = new List<SituacionTerapeutica>();
            }

            await _personaRepo.UpdateAsync(persona);
            await _personaRepo.SaveChangesAsync();

            return MapToResponse(persona);
        }

        // Método privado para mapear Persona -> PersonaResponse
        private PersonaResponse MapToResponse(Persona persona)
        {
            return new PersonaResponse
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
                Documentacion = persona.Documentacion == null ? null : new DocumentacionDTO
                {
                    id = persona.Documentacion.Id,
                    tipoDocumento = (int)persona.Documentacion.TipoDocumento,
                    numero = persona.Documentacion.Numero
                },
                SituacionesTerapeuticas = persona.SituacionesTerapeuticas?.Select(s => new SituacionTerapeuticaResponse
                {
                    id = s.Id,
                    nombre = s.Nombre,
                    descripcion = s.Descripcion,
                    activa = s.Baja == null || s.Baja > DateTime.Now.Date
                }).ToList()
            };
        }
    }
}
