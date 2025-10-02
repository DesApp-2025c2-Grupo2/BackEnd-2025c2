using Application.Contracts.DTOs.Request;
using Application.Contracts.DTOs.Response;
using Application.Contracts.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public class SituacionTerapeuticaService : ISituacionTerapeuticaService
{
    private readonly ISituacionTerapeuticaRepository repository;
    public SituacionTerapeuticaService(ISituacionTerapeuticaRepository repository)
    {
        this.repository = repository;
    }

    public async Task<SituacionTerapeuticaResponse> AddAsync(SituacionTerapeuticaRequest request)
    {
        SituacionTerapeutica entity = new SituacionTerapeutica
        {
            Nombre = request.nombre,
            Descripcion = request.descripcion,
            Alta = DateTime.Now.Date,
            Baja = request.activa ? null : DateTime.Now.Date
        };
        entity = await repository.AddAsync(entity);
        SituacionTerapeuticaResponse response = new SituacionTerapeuticaResponse
        {
            id = entity.Id,
            nombre = entity.Nombre,
            descripcion = entity.Descripcion,
            activa = entity.Baja == null
        };
        return response;
    }

    public async Task<SituacionesTerapeuticasResponse> GetAllAsync()
    {
        List<SituacionTerapeutica> situaciones = await repository.GetAllAsync();
        SituacionesTerapeuticasResponse response = new SituacionesTerapeuticasResponse();
        situaciones.ForEach(situacion =>
        {
            response.Add(new SituacionTerapeuticaResponse
            {
                id = situacion.Id,
                nombre = situacion.Nombre,
                descripcion = situacion.Descripcion,
                activa = situacion.Baja == null || situacion.Baja > DateTime.Now.Date
            });
        });
        return response;
    }

    public async Task<bool> ToggleStatusAsync(int id)
    {
        bool opExitosa = await repository.ToggleStatusAsync(id);
        if (!opExitosa)
        {
            throw new Exception("No se pudo cambiar el estado de la Situacion Terapeutica");
        }
        return opExitosa;
    }

    public async Task<SituacionTerapeuticaResponse> UpdateAsync(int id, SituacionTerapeuticaRequest request)
    {
        SituacionTerapeutica entity = new SituacionTerapeutica
        {
            Id = id,
            Nombre = request.nombre,
            Descripcion = request.descripcion,
            Baja = request.activa ? null : DateTime.Now.Date
        };
        entity = await repository.UpdateAsync(entity);
        SituacionTerapeuticaResponse response = new SituacionTerapeuticaResponse
        {
            id = entity.Id,
            nombre = entity.Nombre,
            descripcion = entity.Descripcion,
            activa = entity.Baja == null
        };
        return response;
    }
}
