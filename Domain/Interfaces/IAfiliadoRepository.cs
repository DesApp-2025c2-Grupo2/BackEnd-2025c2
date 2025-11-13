using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IAfiliadoRepository
    {
        Task<Afiliado> GetByNumeroAsync(int numeroAfiliado);
        Task<List<Afiliado>> GetAllAsync();
        Task AddAsync(Afiliado afiliado,Dictionary<int,DateTime?> situacionesTerapeuticasTitular);
        Task UpdateAsync(Afiliado afiliado);
        Task<bool> ToggleStatus(int afiliadoID, bool activo, DateTime? fecha);

    }
}
