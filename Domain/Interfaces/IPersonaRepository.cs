using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IPersonaRepository
    {
        Task<Persona> GetByIdAsync(int id);
        Task<List<Persona>> GetAllAsync();
        Task AddAsync(Persona persona);
        Task UpdateAsync(Persona persona);
        Task SaveChangesAsync();
    }
}
