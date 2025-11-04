using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IPersonaRepository
    {
        Task<Persona> GetByIdAsync(int id);
        Task<Persona> AddAsync(Persona persona,Dictionary<int,DateTime?> situacionesTerapeuticas);
        Task<bool> UpdateAsync(Persona persona,Dictionary<int, DateTime?> situacionesTerapeuticas);
        Task<bool> ToggleStatusAsync(int id, DateTime? fecha);
    }
}
