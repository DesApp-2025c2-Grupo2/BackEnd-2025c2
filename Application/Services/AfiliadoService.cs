using Application.Contracts.DTOs.Internal;
using Application.Contracts.DTOs.Request;
using Application.Contracts.DTOs.Response;
using Application.Contracts.Interfaces;
using Application.Utilities;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;

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
        PersonaRequest? titularReq = request.Integrantes?.FirstOrDefault(p => p.Parentesco == (int)Parentesco.Titular);
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
        AfiliadoResponse response = DTOMapper.AfiliadoToDTO(afiliadoEntity);
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
        AfiliadosResponse response = new AfiliadosResponse();

        var afiliadosEntities = await afiliadoRepository.GetAllAsync();
        afiliadosEntities.ForEach(afiliado =>
        {
            var afiliadoDTO = DTOMapper.AfiliadoToDTO(afiliado);
            response.Add(afiliadoDTO);
        });
        return response;
    }

    public async Task<AfiliadoResponse> GetByNumeroAsync(int numeroAfiliado)
    {
        AfiliadoResponse response;
        var afiliadoEntity = await afiliadoRepository.GetByNumeroAsync(numeroAfiliado);
        if (afiliadoEntity == null) throw new KeyNotFoundException("Afiliado no encontrado.");
        
        response = DTOMapper.AfiliadoToDTO(afiliadoEntity);
        return response;
    }

    public Task<bool> ToggleStatus(int afiliadoID, bool activo, DateTime? fecha)
    {
        return afiliadoRepository.ToggleStatus(afiliadoID, activo, (fecha ?? DateTime.Now.Date));
    }

}
