using Application.Contracts.DTOs.Request;
using Application.Contracts.DTOs.Response;

namespace Application.Contracts.Interfaces;

public interface IEspecialidadService
{
    Task<EspecialidadesResponse> GetAllAsync();
    Task<EspecialidadResponse> AddAsync(EspecialidadRequest especialidadRequest);
    Task<EspecialidadResponse> UpdateAsync(int id, EspecialidadRequest especialidadRequest);
    Task<bool> ToggleStatusAsync(int id);
}
