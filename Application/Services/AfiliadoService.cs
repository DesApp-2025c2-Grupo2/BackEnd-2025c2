using Application.Contracts.DTOs.Request;
using Application.Contracts.DTOs.Response;
using Application.Contracts.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Enums;
using Application.Utilities;

namespace Application.Services;

public class AfiliadoService : IAfiliadoService
{
    private readonly IAfiliadoRepository afiliadoRepository;

    public AfiliadoService(IAfiliadoRepository afiliadoRepo)
    {
        afiliadoRepository = afiliadoRepo;
    }

    public async Task<AfiliadoResponse> CreateAsync(AfiliadoRequest request)
    {
        // Obtenemos el titular
        PersonaRequest? titularReq = request.Integrantes.FirstOrDefault(p => p.Parentesco == (int)Parentesco.Titular);
        if (titularReq == null)
            throw new Exception("Debe proporcionarse un titular.");
        // Creamos la entidad Persona para el titular
        Persona titularEntity = new Persona()
        {
            Nombre = titularReq.Nombre,
            Apellido = titularReq.Apellido,
            FechaNacimiento = titularReq.FechaNacimiento,
            Parentesco = Parentesco.Titular,
            NumeroIntegrante = 1,
            Alta = titularReq.Alta,
            Baja = titularReq.Baja,
            Documentacion = titularReq.Documentacion != null ? new Documentacion
            {
                TipoDocumento = (TipoDocumento)titularReq.Documentacion.tipoDocumento,
                Numero = titularReq.Documentacion.numero
            } : null,
            Telefonos = titularReq.Telefonos?.Select(t => new Telefono { Numero = t.Numero }).ToList() ?? new(),
            Emails = titularReq.Emails?.Select(e => new Email { Correo = e.Correo }).ToList() ?? new(),
            Direcciones = titularReq.Direcciones?.Select(d => new Direccion
            {
                Calle = d.Calle,
                Altura = d.Altura,
                Piso = d.Piso,
                Departamento = d.Departamento,
                ProvinciaCiudad = d.ProvinciaCiudad
            }).ToList() ?? new(),
        };
        // Creamos la entidad Afiliado
        Afiliado afiliadoEntity = new Afiliado()
        {
            NumeroAfiliado = 0,
            PlanMedicoId = request.PlanMedicoId,
            Alta = request.Alta,
            Baja = request.Baja,
            Integrantes = new List<Persona>() { titularEntity }
        };
        // Guardamos el afiliado (y el titular asociado)
        await afiliadoRepository.AddAsync(afiliadoEntity, titularReq.SituacionesTerapeuticas ?? new());
        // Verificamos que se haya guardado correctamente
        if (afiliadoEntity.Id == 0)
            throw new Exception("Error al crear el afiliado.");
        // Devolvemos el response
        AfiliadoResponse response = new AfiliadoResponse()
        {
            Id = afiliadoEntity.Id,
            NumeroAfiliado = afiliadoEntity.NumeroAfiliado,
            TitularID = titularEntity.Id,
            PlanMedicoId = afiliadoEntity.PlanMedicoId,
            Alta = afiliadoEntity.Alta,
            Baja = afiliadoEntity.Baja,
            Integrantes = new List<PersonaResponse>()
            {
                new PersonaResponse()
                {
                    Id = titularEntity.Id,
                    NumeroIntegrante = titularEntity.NumeroIntegrante,
                    Nombre = titularEntity.Nombre,
                    Apellido = titularEntity.Apellido,
                    FechaNacimiento = titularEntity.FechaNacimiento,
                    Parentesco = titularEntity.Parentesco,
                    Alta = titularEntity.Alta,
                    Baja = titularEntity.Baja,
                    Documentacion = titularEntity.Documentacion != null ? new Application.Contracts.DTOs.Internal.DocumentacionDTO
                    {
                        id = titularEntity.Documentacion.Id,
                        tipoDocumento = (int)titularEntity.Documentacion.TipoDocumento,
                        numero = titularEntity.Documentacion.Numero
                    } : null,
                    Telefonos = titularEntity.Telefonos?.Select(t => new Application.Contracts.DTOs.Internal.TelefonoDTO
                    {
                        Id = t.Id,
                        Numero = t.Numero
                    }).ToList(),
                    Emails = titularEntity.Emails?.Select(e => new Application.Contracts.DTOs.Internal.EmailDTO
                    {
                        Id = e.Id,
                        Correo = e.Correo
                    }).ToList(),
                    Direcciones = titularEntity.Direcciones?.Select(d => new Application.Contracts.DTOs.Internal.DireccionDTO
                    {
                        Id = d.Id,
                        Calle = d.Calle,
                        Altura = d.Altura,
                        Piso = d.Piso,
                        Departamento = d.Departamento,
                        ProvinciaCiudad = d.ProvinciaCiudad
                    }).ToList()
                }
            }
            };
        return response;

    }

    public async Task<bool> UpdateAsync(int id, AfiliadoRequest request)
    {
        Afiliado afiliado = new Afiliado()
        {
            Id = id,
            NumeroAfiliado = request.NumeroAfiliado,
            TitularID = request.TitularID,
            PlanMedicoId = request.PlanMedicoId,
            Alta = request.Alta,
            Baja = request.Baja
        };
        await afiliadoRepository.UpdateAsync(afiliado);
        return true;
    }
    
    public async Task<AfiliadosResponse> GetAllAsync()
    {
        AfiliadosResponse response;

        var afiliadosEntities = await afiliadoRepository.GetAllAsync();
        response = afiliadosEntities == null || !afiliadosEntities.Any() ? new AfiliadosResponse() : (AfiliadosResponse)afiliadosEntities.Select(a => EntityDTOMapper.AfiliadoToDTO(a)).ToList();
        return response;
    }

    public async Task<AfiliadoResponse> GetByNumeroAsync(int numeroAfiliado)
    {
        AfiliadoResponse response;
        var afiliadoEntity = await afiliadoRepository.GetByNumeroAsync(numeroAfiliado);
        if (afiliadoEntity == null) throw new KeyNotFoundException("Afiliado no encontrado.");
        
        response = EntityDTOMapper.AfiliadoToDTO(afiliadoEntity);
        return response;
    }

    public Task<bool> ToggleStatus(int numeroAfiliado, bool activo, DateTime? fecha)
    {
        return afiliadoRepository.ToggleStatus(numeroAfiliado, activo, (fecha ?? DateTime.Now.Date));
    }

    /*
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

await afiliadoRepository.AddAsync(afiliado);
await afiliadoRepository.SaveChangesAsync(); // Afiliado.Id disponible

// --- 2. Crear Titular ---
var titularRequest = request.Integrantes.First();
List<HistorialTerapeutico> situacionesTitular = new();
if (titularRequest.SituacionesTerapeuticasIds != null && titularRequest.SituacionesTerapeuticasIds.Any())
  situacionesTitular = await situacionRepository.GetByIdsAsync(titularRequest.SituacionesTerapeuticasIds);

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
      TipoDocumento = (Domain.Enums.TipoDocumento)titularRequest.Documentacion.tipoDocumento,
      Numero = titularRequest.Documentacion.numero
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

await personaRepository.AddAsync(titular);
await personaRepository.SaveChangesAsync(); // titular.Id disponible

// --- 3. Actualizar Afiliado con TitularID ---
afiliado.TitularID = titular.Id;
await afiliadoRepository.UpdateAsync(afiliado);
await afiliadoRepository.SaveChangesAsync();

// --- 4. Crear resto de integrantes ---
if (request.Integrantes.Count > 1)
{
  foreach (var pReq in request.Integrantes.Skip(1))
  {
      List<SituacionTerapeutica> situaciones = new();
      if (pReq.SituacionesTerapeuticasIds != null && pReq.SituacionesTerapeuticasIds.Any())
          situaciones = await situacionRepository.GetByIdsAsync(pReq.SituacionesTerapeuticasIds);

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
              TipoDocumento = (Domain.Enums.TipoDocumento)pReq.Documentacion.tipoDocumento,
              Numero = pReq.Documentacion.numero
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

      await personaRepository.AddAsync(persona);
  }

  await personaRepository.SaveChangesAsync();
}

// --- 5. Mapear a response ---
var afiliadoGuardado = await afiliadoRepository.GetByIdAsync(afiliado.Id);
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
var entidad = await afiliadoRepository.GetByIdAsync(id);
if (entidad == null) throw new Exception("Afiliado no encontrado");

entidad.Baja = DateTime.UtcNow;
await afiliadoRepository.UpdateAsync(entidad);
await afiliadoRepository.SaveChangesAsync();
}

public async Task<IEnumerable<AfiliadoResponse>> GetAllAsync()
{
var lista = await afiliadoRepository.GetAllAsync();
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
var entidad = await afiliadoRepository.GetByNumeroAsync(numeroAfiliado);
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

public async Task<AfiliadoResponse> UpdateAsync(int id, AfiliadoRequest request)
{
var entidad = await afiliadoRepository.GetByIdAsync(id);
if (entidad == null) throw new Exception("Afiliado no encontrado");

entidad.TitularID = request.TitularID;
entidad.PlanMedicoId = request.PlanMedicoId;
entidad.Alta = request.Alta;
entidad.Baja = request.Baja;

await afiliadoRepository.UpdateAsync(entidad);
await afiliadoRepository.SaveChangesAsync();

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
              AfiliadoId = entidad.Id,
              Alta = integranteReq.Alta,
              Baja = integranteReq.Baja,
              Documentacion = integranteReq.Documentacion != null ? new Documentacion
              {
                  TipoDocumento = (Domain.Enums.TipoDocumento)integranteReq.Documentacion.tipoDocumento,
                  Numero = integranteReq.Documentacion.numero
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

          await personaRepository.AddAsync(persona);
          await personaRepository.SaveChangesAsync();
      }
  }
}

// devolver el afiliado actualizado (incluyendo integrantes)
var actualizado = await afiliadoRepository.GetByIdAsync(id);
return new AfiliadoResponse
{
  Id = actualizado.Id,
  NumeroAfiliado = actualizado.NumeroAfiliado,
  TitularID = actualizado.TitularID,
  PlanMedicoId = actualizado.PlanMedicoId,
  Alta = actualizado.Alta,
  Baja = actualizado.Baja,
  Integrantes = actualizado.Integrantes?.Select(MapPersonaToResponse).ToList() ?? new()
};
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
      id = p.Documentacion.Id,
      tipoDocumento = (int)p.Documentacion.TipoDocumento,
      numero = p.Documentacion.Numero
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
*/



}
