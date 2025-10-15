using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Contracts.DTOs.Request;
using Application.Contracts.DTOs.Response;
using Domain.Entities;
using Application.Contracts.Interfaces;
using Domain.Interfaces;


namespace Application.Services
{
    public class AfiliadoService : IAfiliadoService
    {
        private readonly IAfiliadoRepository _afiliadoRepo;
        private readonly IPersonaRepository _personaRepo;


        public AfiliadoService(IAfiliadoRepository afiliadoRepo, IPersonaRepository personaRepo)
        {
            _afiliadoRepo = afiliadoRepo;
            _personaRepo = personaRepo;
        }

        public async Task<AfiliadoResponse> CreateAsync(AfiliadoRequest request)
        {
            // Mapear request -> entidad
            var entidad = new Afiliado
            {
                NumeroAfiliado = request.NumeroAfiliado,
                PlanMedicoId = request.PlanMedicoId,
                Alta = request.Alta,
                Baja = request.Baja
            };


            await _afiliadoRepo.AddAsync(entidad);
            await _afiliadoRepo.SaveChangesAsync();


            // Si vienen integrantes anidados, guardarlos
            if (request.Integrantes != null && request.Integrantes.Any())
            {
                foreach (var pReq in request.Integrantes)
                {
                    var persona = new Persona
                    {
                        Nombre = pReq.Nombre,
                        Apellido = pReq.Apellido,
                        FechaNacimiento = pReq.FechaNacimiento,
                        Parentesco = pReq.Parentesco,
                        NumeroIntegrante = pReq.NumeroIntegrante,
                        AfiliadoId = entidad.Id,
                        Alta = pReq.Alta,
                        Baja = pReq.Baja
                    };


                    await _personaRepo.AddAsync(persona);
                }
                await _personaRepo.SaveChangesAsync();
            }


            // Mapear entidad -> response
            var response = new AfiliadoResponse
            {
                NumeroAfiliado = entidad.NumeroAfiliado,
                TitularID = entidad.TitularID,
                PlanMedicoId = entidad.PlanMedicoId,
                PlanMedicoNombre = null,
                Alta = entidad.Alta,
                Baja = entidad.Baja,
                Integrantes = new List<PersonaResponse>()
            };


            return response;
        }

        public async Task DeleteAsync(int id)
        {
            var entidad = await _afiliadoRepo.GetByIdAsync(id);
            if (entidad == null) throw new Exception("Afiliado no encontrado");


            // lógica de borrado: marcar como baja o eliminar físicamente según tu política
            entidad.Baja = DateTime.UtcNow;


            await _afiliadoRepo.UpdateAsync(entidad);
            await _afiliadoRepo.SaveChangesAsync();
        }

        public async Task<IEnumerable<AfiliadoResponse>> GetAllAsync()
        {
            var lista = await _afiliadoRepo.GetAllAsync();
            return lista.Select(a => new AfiliadoResponse
            {
                NumeroAfiliado = a.NumeroAfiliado,
                TitularID = a.TitularID,
                PlanMedicoId = a.PlanMedicoId,
                PlanMedicoNombre = null,
                Alta = a.Alta,
                Baja = a.Baja,
                Integrantes = a.Integrantes?.Select(p => new PersonaResponse
                {
                    id = p.Id,
                    numeroIntegrante = p.NumeroIntegrante,
                    nombre = p.Nombre,
                    apellido = p.Apellido,
                    fechaNacimiento = p.FechaNacimiento,
                    parentesco = p.Parentesco,
                    alta = p.Alta,
                    baja = p.Baja,
                    telefonos = new System.Collections.Generic.List<Application.Contracts.DTOs.Internal.TelefonoDTO>(),
                    emails = new System.Collections.Generic.List<Application.Contracts.DTOs.Internal.EmailDTO>(),
                    documentacion = null,
                    direcciones = new System.Collections.Generic.List<Application.Contracts.DTOs.Internal.DireccionDTO>()
                }).ToList()
            }).ToList();
        }

        public async Task<AfiliadoResponse> GetByNumeroAsync(int numeroAfiliado)
        {
            var entidad = await _afiliadoRepo.GetByNumeroAsync(numeroAfiliado);
            if (entidad == null) return null;


            return new AfiliadoResponse
            {
                NumeroAfiliado = entidad.NumeroAfiliado,
                TitularID = entidad.TitularID,
                PlanMedicoId = entidad.PlanMedicoId,
                PlanMedicoNombre = null,
                Alta = entidad.Alta,
                Baja = entidad.Baja,
                Integrantes = entidad.Integrantes?.Select(p => new PersonaResponse
                {
                    id = p.Id,
                    numeroIntegrante = p.NumeroIntegrante,
                    nombre = p.Nombre,
                    apellido = p.Apellido,
                    fechaNacimiento = p.FechaNacimiento,
                    parentesco = p.Parentesco,
                    alta = p.Alta,
                    baja = p.Baja,
                    telefonos = new System.Collections.Generic.List<Application.Contracts.DTOs.Internal.TelefonoDTO>(),
                    emails = new System.Collections.Generic.List<Application.Contracts.DTOs.Internal.EmailDTO>(),
                    documentacion = null,
                    direcciones = new System.Collections.Generic.List<Application.Contracts.DTOs.Internal.DireccionDTO>()
                }).ToList()
            };
        }

        public async Task UpdateAsync(int id, AfiliadoRequest request)
        {
            var entidad = await _afiliadoRepo.GetByIdAsync(id);
            if (entidad == null) throw new Exception("Afiliado no encontrado");


            entidad.PlanMedicoId = request.PlanMedicoId;
            entidad.Alta = request.Alta;
            entidad.Baja = request.Baja;


            await _afiliadoRepo.UpdateAsync(entidad);
            await _afiliadoRepo.SaveChangesAsync();


            // Actualizar/crear integrantes según request.Integrantes (lógica simplificada)
        }
    }
}
