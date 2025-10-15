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


        public PersonaService(IPersonaRepository personaRepo)
        {
            _personaRepo = personaRepo;
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
                Baja = request.Baja
            };


            await _personaRepo.AddAsync(persona);
            await _personaRepo.SaveChangesAsync();


            return new PersonaResponse
            {
                id = persona.Id,
                numeroIntegrante = persona.NumeroIntegrante,
                nombre = persona.Nombre,
                apellido = persona.Apellido,
                fechaNacimiento = persona.FechaNacimiento,
                parentesco = persona.Parentesco,
                alta = persona.Alta,
                baja = persona.Baja,
                telefonos = new List<Application.Contracts.DTOs.Internal.TelefonoDTO>(),
                emails = new List<Application.Contracts.DTOs.Internal.EmailDTO>(),
                documentacion = null,
                direcciones = new List<Application.Contracts.DTOs.Internal.DireccionDTO>()
            };
        }

        public async Task<PersonaResponse> GetByIdAsync(int id)
        {
            // Obtenemos la persona desde el repositorio
            var persona = await _personaRepo.GetByIdAsync(id);

            // Si no existe, devolvemos null
            if (persona == null) return null;

            // Convertimos la entidad Persona a PersonaResponse
            var response = new PersonaResponse
            {
                id = persona.Id,
                numeroIntegrante = persona.NumeroIntegrante,
                nombre = persona.Nombre,
                apellido = persona.Apellido,
                fechaNacimiento = persona.FechaNacimiento,
                parentesco = persona.Parentesco,
                alta = persona.Alta,
                baja = persona.Baja,
                // Aquí opcionalmente mapear relaciones si existen
                telefonos = persona.Telefonos?.Select(t => new TelefonoDTO
                {
                    id = t.Id,
                    numero = t.Numero
                }).ToList() ?? new List<TelefonoDTO>(),
                emails = persona.Emails?.Select(e => new EmailDTO
                {
                    id = e.Id,
                    correo = e.Correo
                }).ToList() ?? new List<EmailDTO>(),
                documentacion = persona.Documentacion != null ? new DocumentacionDTO
                {
                    Id = persona.Documentacion.Id,
                    TipoDocumento = persona.Documentacion.TipoDocumento,
                    Numero = persona.Documentacion.Numero
                } : null,
                direcciones = persona.Direcciones?.Select(d => new DireccionDTO
                {
                    id = d.Id,
                    calle = d.Calle,
                    altura = d.Altura,
                    piso = d.Piso,
                    departamento = d.Departamento,
                    provinciaCiudad = d.ProvinciaCiudad
                }).ToList() ?? new List<DireccionDTO>(),
                SituacionesTerapeuticasIds = persona.SituacionesTerapeuticas?.Select(s => s.Id).ToList()
            };

            return response;
        }


        public async Task<PersonaResponse> ActualizarPersonaAsync(int id, PersonaRequest request)
        {
            var persona = await _personaRepo.GetByIdAsync(id);
            if (persona == null) throw new Exception("Persona no encontrada");

            persona.Nombre = request.Nombre;
            persona.Apellido = request.Apellido;
            persona.FechaNacimiento = request.FechaNacimiento;
            persona.Parentesco = request.Parentesco;
            persona.Baja = request.Baja;

            await _personaRepo.UpdateAsync(persona);
            await _personaRepo.SaveChangesAsync();

            // Convertimos la entidad actualizada a PersonaResponse
            return new PersonaResponse
            {
                id = persona.Id,
                numeroIntegrante = persona.NumeroIntegrante,
                nombre = persona.Nombre,
                apellido = persona.Apellido,
                fechaNacimiento = persona.FechaNacimiento,
                parentesco = persona.Parentesco,
                alta = persona.Alta,
                baja = persona.Baja,
                // mapear relaciones opcionales si existen
                telefonos = persona.Telefonos?.Select(t => new TelefonoDTO
                {
                    id = t.Id,
                    numero = t.Numero
                }).ToList() ?? new List<TelefonoDTO>(),
                emails = persona.Emails?.Select(e => new EmailDTO
                {
                    id = e.Id,
                    correo = e.Correo
                }).ToList() ?? new List<EmailDTO>(),
                documentacion = persona.Documentacion != null ? new DocumentacionDTO
                {
                    Id = persona.Documentacion.Id,
                    TipoDocumento = persona.Documentacion.TipoDocumento,
                    Numero = persona.Documentacion.Numero
                } : null,
                direcciones = persona.Direcciones?.Select(d => new DireccionDTO
                {
                    id = d.Id,
                    calle = d.Calle,
                    altura = d.Altura,
                    piso = d.Piso,
                    departamento = d.Departamento,
                    provinciaCiudad = d.ProvinciaCiudad
                }).ToList() ?? new List<DireccionDTO>(),
                SituacionesTerapeuticasIds = persona.SituacionesTerapeuticas?.Select(s => s.Id).ToList()
            };
        }

    }
}
