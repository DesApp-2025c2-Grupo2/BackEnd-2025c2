using Domain.Entities;

namespace Domain.Interfaces;

public interface IPrestadorRepository
{
    Task<Prestador> AddAsync(Prestador prestador);
    Task<Prestador> UpdateAsync(Prestador prestador);
    Task<List<Prestador>> GetAllAsync();
    Task<bool> ToggleStatusAsync(int id,DateTime? fechaEfecto);
}
