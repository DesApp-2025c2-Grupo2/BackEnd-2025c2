using Application.Contracts.DTOs.Request;
using Application.Contracts.DTOs.Response;

namespace Application.Contracts.Interfaces;

public interface ISituacionTerapeuticaService
{
    Task<SituacionesTerapeuticasResponse> GetAllAsync();
    Task<bool> ToggleStatusAsync(int id);
    Task<SituacionTerapeuticaResponse> UpdateAsync(int id, SituacionTerapeuticaRequest request);
    Task<SituacionTerapeuticaResponse> AddAsync(SituacionTerapeuticaRequest request);
}
