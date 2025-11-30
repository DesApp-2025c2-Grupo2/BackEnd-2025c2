using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IPrestadorRepository
{
    Task<Prestador> CreateAsync(Prestador prestador, List<int> especialidadesIds);
    Task<Prestador> UpdateAsync(Prestador prestador, List<int> especialidadesIds);
    Task<List<Prestador>> GetAllAsync();
    Task<bool> ToggleStatusAsync(int id);
}
