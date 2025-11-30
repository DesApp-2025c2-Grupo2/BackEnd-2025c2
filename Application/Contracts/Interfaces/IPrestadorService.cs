using Application.Contracts.DTOs.Request;
using Application.Contracts.DTOs.Response;

namespace Application.Contracts.Interfaces;

public interface IPrestadorService
{
    Task<PrestadoresResponse> GetAllAsync();
    Task<bool> ToggleStatusAsync(int id);
    Task<PrestadorResponse> SaveAsync(PrestadorRequest request);
    Task<AgendaResponse> UpdateAgendaAsync(AgendaRequest request);
}


