using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Contracts.DTOs.Request;
using Application.Contracts.DTOs.Response;
using Application.Contracts.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class AfiliadoService : IAfiliadoService
    {
        private readonly IAfiliadoRepository _afiliadoRepo;
        private readonly IPersonaRepository _personaRepo;
        private readonly ISituacionTerapeuticaRepository _situacionRepo;

        public AfiliadoService(
            IAfiliadoRepository afiliadoRepo,
            IPersonaRepository personaRepo,
            ISituacionTerapeuticaRepository situacionRepo)
        {
            _afiliadoRepo = afiliadoRepo;
            _personaRepo = personaRepo;
            _situacionRepo = situacionRepo;
        }

        public async Task<AfiliadoResponse> CreateAsync(AfiliadoRequest request)
        {
            if (request.Integrantes == null || !request.Integrantes.Any())
                throw new Exception("Debe proporcionarse al menos un integrante titular.");

            // --- 1. Crear Afiliado sin TitularID ---
            var afiliado = new Afiliado
            {
                NumeroAfiliado = request.NumeroAfiliado,
                PlanMedicoId = request.PlanMedicoId,
                Alta = request.Alta,
                Baja = request.Baja
            };

            await _afiliadoRepo.AddAsync(afiliado);
            await _afiliadoRepo.SaveChangesAsync(); // Afiliado.Id disponible

            // --- 2. Crear Titular ---
            var titularRequest = request.Integrantes.First();
            List<SituacionTerapeutica> situacionesTitular = new();
            if (titularRequest.SituacionesTerapeuticasIds != null && titularRequest.SituacionesTerapeuticasIds.Any())
                situacionesTitular = await _situacionRepo.GetByIdsAsync(titularRequest.SituacionesTerapeuticasIds);

            var titular = new Persona
            {
                Nombre = titularRequest.Nombre,
                Apellido = titularRequest.Apellido,
                FechaNacimiento = titularRequest.FechaNacimiento,
                Parentesco = titularRequest.Parentesco,
                NumeroIntegrante = titularRequest.NumeroIntegrante,
                AfiliadoId = afiliado.Id,
                Alta = titularRequest.Alta,
                Baja = titularRequest.Baja,
                Documentacion = titularRequest.Documentacion != null ? new Documentacion
                {
                    TipoDocumento = titularRequest.Documentacion.TipoDocumento,
                    Numero = titularRequest.Documentacion.Numero
                } : null,
                Telefonos = titularRequest.Telefonos?.Select(t => new Telefono { Numero = t.Numero }).ToList() ?? new(),
                Emails = titularRequest.Emails?.Select(e => new Email { Correo = e.Correo }).ToList() ?? new(),
                Direcciones = titularRequest.Direcciones?.Select(d => new Direccion
                {
                    Calle = d.Calle,
                    Altura = d.Altura,
                    Piso = d.Piso,
                    Departamento = d.Departamento,
                    ProvinciaCiudad = d.ProvinciaCiudad
                }).ToList() ?? new(),
                SituacionesTerapeuticas = situacionesTitular
            };

            await _personaRepo.AddAsync(titular);
            await _personaRepo.SaveChangesAsync(); // titular.Id disponible

            // --- 3. Actualizar Afiliado con TitularID ---
            afiliado.TitularID = titular.Id;
            await _afiliadoRepo.UpdateAsync(afiliado);
            await _afiliadoRepo.SaveChangesAsync();

            // --- 4. Crear resto de integrantes ---
            if (request.Integrantes.Count > 1)
            {
                foreach (var pReq in request.Integrantes.Skip(1))
                {
                    List<SituacionTerapeutica> situaciones = new();
                    if (pReq.SituacionesTerapeuticasIds != null && pReq.SituacionesTerapeuticasIds.Any())
                        situaciones = await _situacionRepo.GetByIdsAsync(pReq.SituacionesTerapeuticasIds);

                    var persona = new Persona
                    {
                        Nombre = pReq.Nombre,
                        Apellido = pReq.Apellido,
                        FechaNacimiento = pReq.FechaNacimiento,
                        Parentesco = pReq.Parentesco,
                        NumeroIntegrante = pReq.NumeroIntegrante,
                        AfiliadoId = afiliado.Id,
                        Alta = pReq.Alta,
                        Baja = pReq.Baja,
                        Documentacion = pReq.Documentacion != null ? new Documentacion
                        {
                            TipoDocumento = pReq.Documentacion.TipoDocumento,
                            Numero = pReq.Documentacion.Numero
                        } : null,
                        Telefonos = pReq.Telefonos?.Select(t => new Telefono { Numero = t.Numero }).ToList() ?? new(),
                        Emails = pReq.Emails?.Select(e => new Email { Correo = e.Correo }).ToList() ?? new(),
                        Direcciones = pReq.Direcciones?.Select(d => new Direccion
                        {
                            Calle = d.Calle,
                            Altura = d.Altura,
                            Piso = d.Piso,
                            Departamento = d.Departamento,
                            ProvinciaCiudad = d.ProvinciaCiudad
                        }).ToList() ?? new(),
                        SituacionesTerapeuticas = situaciones
                    };

                    await _personaRepo.AddAsync(persona);
                }

                await _personaRepo.SaveChangesAsync();
            }

            // --- 5. Mapear a response ---
            var afiliadoGuardado = await _afiliadoRepo.GetByIdAsync(afiliado.Id);
            return new AfiliadoResponse
            {
                Id = afiliadoGuardado.Id,
                NumeroAfiliado = afiliadoGuardado.NumeroAfiliado,
                TitularID = afiliadoGuardado.TitularID,
                PlanMedicoId = afiliadoGuardado.PlanMedicoId,
                Alta = afiliadoGuardado.Alta,
                Baja = afiliadoGuardado.Baja,
                Integrantes = afiliadoGuardado.Integrantes?.Select(MapPersonaToResponse).ToList() ?? new()
            };
        }

        public async Task DeleteAsync(int id)
        {
            var entidad = await _afiliadoRepo.GetByIdAsync(id);
            if (entidad == null) throw new Exception("Afiliado no encontrado");

            entidad.Baja = DateTime.UtcNow;
            await _afiliadoRepo.UpdateAsync(entidad);
            await _afiliadoRepo.SaveChangesAsync();
        }

        public async Task<IEnumerable<AfiliadoResponse>> GetAllAsync()
        {
            var lista = await _afiliadoRepo.GetAllAsync();
            return lista.Select(a => new AfiliadoResponse
            {
                Id = a.Id,
                NumeroAfiliado = a.NumeroAfiliado,
                TitularID = a.TitularID,
                PlanMedicoId = a.PlanMedicoId,
                Alta = a.Alta,
                Baja = a.Baja,
                Integrantes = a.Integrantes?.Select(MapPersonaToResponse).ToList() ?? new()
            }).ToList();
        }

        public async Task<AfiliadoResponse> GetByNumeroAsync(int numeroAfiliado)
        {
            var entidad = await _afiliadoRepo.GetByNumeroAsync(numeroAfiliado);
            if (entidad == null) return null;

            return new AfiliadoResponse
            {
                Id = entidad.Id,
                NumeroAfiliado = entidad.NumeroAfiliado,
                TitularID = entidad.TitularID,
                PlanMedicoId = entidad.PlanMedicoId,
                Alta = entidad.Alta,
                Baja = entidad.Baja,
                Integrantes = entidad.Integrantes?.Select(MapPersonaToResponse).ToList() ?? new()
            };
        }

        public async Task UpdateAsync(int id, AfiliadoRequest request)
        {
            var entidad = await _afiliadoRepo.GetByIdAsync(id);
            if (entidad == null) throw new Exception("Afiliado no encontrado");

            entidad.TitularID = request.TitularID;
            entidad.PlanMedicoId = request.PlanMedicoId;
            entidad.Alta = request.Alta;
            entidad.Baja = request.Baja;

            await _afiliadoRepo.UpdateAsync(entidad);
            await _afiliadoRepo.SaveChangesAsync();

            if (request.Integrantes != null && request.Integrantes.Any())
            {
                foreach (var integranteReq in request.Integrantes)
                {
                    // Si el integrante no tiene Id (nuevo)
                    if (integranteReq.Id == 0)
                    {
                        var persona = new Persona
                        {
                            Nombre = integranteReq.Nombre,
                            Apellido = integranteReq.Apellido,
                            FechaNacimiento = integranteReq.FechaNacimiento,
                            Parentesco = integranteReq.Parentesco,
                            NumeroIntegrante = integranteReq.NumeroIntegrante,
                            Alta = integranteReq.Alta,
                            Baja = integranteReq.Baja,
                            Documentacion = integranteReq.Documentacion != null ? new Documentacion
                            {
                                TipoDocumento = integranteReq.Documentacion.TipoDocumento,
                                Numero = integranteReq.Documentacion.Numero
                            } : null,
                            Telefonos = integranteReq.Telefonos?.Select(t => new Telefono { Numero = t.Numero }).ToList() ?? new(),
                            Emails = integranteReq.Emails?.Select(e => new Email { Correo = e.Correo }).ToList() ?? new(),
                            Direcciones = integranteReq.Direcciones?.Select(d => new Direccion
                            {
                                Calle = d.Calle,
                                Altura = d.Altura,
                                Piso = d.Piso,
                                Departamento = d.Departamento,
                                ProvinciaCiudad = d.ProvinciaCiudad
                            }).ToList() ?? new()
                        };

                        await _personaRepo.AddAsync(persona);
                    }
                }
            }
        }
        // --- Auxiliar: mapear Persona -> PersonaResponse ---
        private PersonaResponse MapPersonaToResponse(Persona p)
        {
            return new PersonaResponse
            {
                Id = p.Id,
                NumeroIntegrante = p.NumeroIntegrante,
                Nombre = p.Nombre,
                Apellido = p.Apellido,
                FechaNacimiento = p.FechaNacimiento,
                Parentesco = p.Parentesco,
                Alta = p.Alta,
                Baja = p.Baja,
                Telefonos = p.Telefonos?.Select(t => new Application.Contracts.DTOs.Internal.TelefonoDTO
                {
                    Id = t.Id,
                    Numero = t.Numero
                }).ToList(),
                Emails = p.Emails?.Select(e => new Application.Contracts.DTOs.Internal.EmailDTO
                {
                    Id = e.Id,
                    Correo = e.Correo
                }).ToList(),
                Documentacion = p.Documentacion == null ? null : new Application.Contracts.DTOs.Internal.DocumentacionDTO
                {
                    Id = p.Documentacion.Id,
                    TipoDocumento = p.Documentacion.TipoDocumento,
                    Numero = p.Documentacion.Numero
                },
                Direcciones = p.Direcciones?.Select(d => new Application.Contracts.DTOs.Internal.DireccionDTO
                {
                    Id = d.Id,
                    Calle = d.Calle,
                    Altura = d.Altura,
                    Piso = d.Piso,
                    Departamento = d.Departamento,
                    ProvinciaCiudad = d.ProvinciaCiudad
                }).ToList(),
                SituacionesTerapeuticas = p.SituacionesTerapeuticas?.Select(s => new SituacionTerapeuticaResponse
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
