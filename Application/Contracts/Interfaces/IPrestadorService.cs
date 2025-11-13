using Application.Contracts.DTOs.Request;
using Application.Contracts.DTOs.Response;

namespace Application.Contracts.Interfaces;

public interface IPrestadorService
{
    Task<PrestadorResponse> CreateAsync(PrestadorRequest request);
    Task<PrestadorResponse> UpdateAsync(int id, PrestadorRequest request);
    Task<PrestadorEstadoResponse> UpdateEstadoAsync(int id, PrestadorEstadoRequest request);
    Task<PrestadorResponse> UpdateHorariosAsync(int id, PrestadorHorariosRequest request, string strategy = "merge");
    Task<PrestadorResponse> GetByIdAsync(int id);
    Task<IEnumerable<PrestadorResponse>> GetAllAsync();
}


