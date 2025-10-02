using Application.Contracts.DTOs.Request;
using Application.Contracts.DTOs.Response;

namespace Application.Contracts.Interfaces;

public interface IPlanMedicoService
{
    Task<PlanesMedicosResponse> GetAllAsync();
    Task<PlanMedicoResponse> AddAsync(PlanMedicoRequest planMedicoCreateDto);
    Task<PlanMedicoResponse> UpdateAsync(int id, PlanMedicoRequest planMedicoUpdateDto);
    Task<bool> ToggleStatusAsync(int id);
}
