using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IAfiliadoRepository
    {
        Task<Afiliado> GetByIdAsync(int id);
        Task<Afiliado> GetByNumeroAsync(int numeroAfiliado);
        Task<IEnumerable<Afiliado>> GetAllAsync();
        Task AddAsync(Afiliado afiliado);
        Task UpdateAsync(Afiliado afiliado);
        Task SaveChangesAsync();
    }
}
