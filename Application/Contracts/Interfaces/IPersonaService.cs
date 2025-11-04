using Application.Contracts.DTOs.Request;
using Application.Contracts.DTOs.Response;

namespace Application.Contracts.Interfaces
{
    public interface IPersonaService
    {
        Task<PersonaResponse> AddPersonAsync(PersonaRequest request);
        Task<PersonaResponse> UpdatePersonAsync(int id, PersonaRequest request);
        Task<PersonaResponse> GetByIdAsync(int id);
        Task<bool> ToggleStatusAsync(int id, DateTime? fecha);
    }
}
