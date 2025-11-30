using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IPersonaRepository
{
    Task<Persona> GetByIdAsync(int id);
    Task<Persona> AddAsync(Persona persona,Dictionary<int,DateTime?> situacionesTerapeuticas);
    Task<bool> UpdateAsync(Persona persona,Dictionary<int, DateTime?> situacionesTerapeuticas);
    Task<bool> ToggleStatusAsync(int id, bool activo, DateTime? fecha);
}
