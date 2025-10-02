using Application.Contracts.DTOs.Request;
using Application.Contracts.DTOs.Response;
using Application.Contracts.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public class EspecialidadService : IEspecialidadService
{
    private readonly IEspecialidadRepository repository;
    public EspecialidadService(IEspecialidadRepository repository)
    {
        this.repository = repository;
    }
    public async Task<EspecialidadResponse> AddAsync(EspecialidadRequest especialidadRequest)
    {
        Especialidad especialidad = new Especialidad
        {
            Nombre = especialidadRequest.nombre,
            Descripcion = especialidadRequest.descripcion,
            Alta = DateTime.Now.Date,
            Baja = especialidadRequest.activa ? null : DateTime.Now.Date
        };
        Especialidad result = await repository.AddAsync(especialidad);
        EspecialidadResponse response = new EspecialidadResponse
        {
            id = result.Id,
            nombre = result.Nombre,
            descripcion = result.Descripcion,
            activa = result.Baja == null
        };
        return response;
    }

    public async Task<EspecialidadesResponse> GetAllAsync()
    {
        List<Especialidad> especialidades = await repository.GetAllAsync();
        EspecialidadesResponse response = new EspecialidadesResponse();
        especialidades.ForEach(especialidad =>
        {
            response.Add(new EspecialidadResponse
            {
                id = especialidad.Id,
                nombre = especialidad.Nombre,
                descripcion = especialidad.Descripcion,
                activa = especialidad.Baja == null
            });
        });
        return response;
    }

    public async Task<bool> ToggleStatusAsync(int id)
    {
        return await repository.ToggleStatusAsync(id);
    }

    public async Task<EspecialidadResponse> UpdateAsync(int id, EspecialidadRequest especialidadRequest)
    {
        Especialidad especialidad = new Especialidad
        {
            Id = id,
            Nombre = especialidadRequest.nombre,
            Descripcion = especialidadRequest.descripcion,
            Baja = especialidadRequest.activa ? null : DateTime.Now.Date
        };
        Especialidad result = await repository.UpdateAsync(especialidad);
        EspecialidadResponse response = new EspecialidadResponse
        {
            id = result.Id,
            nombre = result.Nombre,
            descripcion = result.Descripcion,
            activa = result.Baja == null
        };
        return response;
    }
}
