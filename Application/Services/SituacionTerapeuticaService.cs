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

    public SituacionTerapeuticaResponse Add(SituacionTerapeuticaRequest request)
    {
        SituacionTerapeuticaResponse response;
        SituacionTerapeutica entity = new SituacionTerapeutica
        {
            Nombre = request.nombre,
            Descripcion = request.descripcion,
            Alta = DateTime.Now.Date,
            Baja = request.activa ? null : DateTime.Now.Date
        };
        entity = repository.AddAsync(entity).Result;
        response = new SituacionTerapeuticaResponse
        {
            id = entity.Id,
            nombre = entity.Nombre,
            descripcion = entity.Descripcion,
            activa = entity.Baja == null || entity.Baja > DateTime.Now.Date
        };
        return response;
    }

    public SituacionesTerapeuticasResponse GetAll()
    {
        SituacionesTerapeuticasResponse response = new();
        List<SituacionTerapeutica> entities = repository.GetAllAsync().Result;
        entities.ForEach(entity =>
        {
            response.Add(new SituacionTerapeuticaResponse
            {
                id = entity.Id,
                nombre = entity.Nombre,
                descripcion = entity.Descripcion,
                activa = entity.Baja == null || entity.Baja > DateTime.Now.Date
            });
        }); 
        return response;
    }

    public SituacionTerapeuticaResponse ToggleStatus(int id)
    {
        SituacionTerapeuticaResponse? response;
        SituacionTerapeutica? entity = repository.ToggleStatusAsync(new SituacionTerapeutica { Id = id }).Result;
        if (entity == null)
        {
            response = null;
        }
        else
        {
            response = new SituacionTerapeuticaResponse
            {
                id = entity.Id,
                nombre = entity.Nombre,
                descripcion = entity.Descripcion,
                activa = entity.Baja == null || entity.Baja > DateTime.Now.Date
            };
        }
        return response;
    }

    public SituacionTerapeuticaResponse Update(int id, SituacionTerapeuticaRequest request)
    {
        SituacionTerapeuticaResponse response;
        SituacionTerapeutica? entity = new SituacionTerapeutica
        {
            Id = id,
            Nombre = request.nombre,
            Descripcion = request.descripcion,
            Alta = DateTime.Now.Date,
            Baja = request.activa ? null : DateTime.Now.Date
        };
        entity = repository.UpdateAsync(entity).Result;
        response = new SituacionTerapeuticaResponse
        {
            id = entity.Id,
            nombre = entity.Nombre,
            descripcion = entity.Descripcion,
            activa = entity.Baja == null || entity.Baja > DateTime.Now.Date
        };
        return response;
    }
}
