using Application.Contracts.DTOs.Request;
using Application.Contracts.DTOs.Response;

namespace Application.Contracts.Interfaces;

public interface IAfiliadoService
{
    Task<AfiliadoResponse> CreateAsync(AfiliadoRequest request);
    Task<AfiliadosResponse> GetAllAsync();
    Task<bool> UpdateAsync(int id, AfiliadoRequest request);
    Task<bool> ToggleStatus(int afiliadoID, bool activo, DateTime? fecha);
    Task<AfiliadoResponse> GetByNumeroAsync(int numeroAfiliado);
}
