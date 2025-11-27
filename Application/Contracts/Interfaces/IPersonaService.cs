using Application.Contracts.DTOs.Request;
using Application.Contracts.DTOs.Response;
using Domain.Entities;

namespace Application.Contracts.Interfaces
{
    public interface IPersonaService
    {
        Task<PersonaResponse> AddPersonAsync(PersonaRequest request);
        Task<PersonaResponse> UpdatePersonAsync(PersonaRequest request);
        Task<PersonaResponse> GetByIdAsync(int id);
        Task<bool> ToggleStatusAsync(int id, bool activo, DateTime? fecha);
    }
}
