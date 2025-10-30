using Application.Contracts.DTOs.Request;
using Application.Contracts.DTOs.Response;

namespace Application.Contracts.Interfaces
{
    public interface IAfiliadoService
    {
        Task<AfiliadoResponse> CreateAsync(AfiliadoRequest request);
        Task<AfiliadoResponse> GetByNumeroAsync(int numeroAfiliado);
        Task<IEnumerable<AfiliadoResponse>> GetAllAsync();
        Task<AfiliadoResponse> UpdateAsync(int id, AfiliadoRequest request);
        Task DeleteAsync(int id);
    }
}
