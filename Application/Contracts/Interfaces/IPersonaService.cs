using Application.Contracts.DTOs.Request;
using Application.Contracts.DTOs.Response;

namespace Application.Contracts.Interfaces
{
    public interface IPersonaService
    {
        Task<PersonaResponse> CrearPersonaAsync(PersonaRequest request);
        Task<PersonaResponse> ActualizarPersonaAsync(int id, PersonaRequest request);
        Task<PersonaResponse> GetByIdAsync(int id);
    }
}
