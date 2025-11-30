using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IEspecialidadRepository
{
    Task<List<Especialidad>> GetAllAsync();
    Task<Especialidad> AddAsync(Especialidad especialidad);
    Task<Especialidad> UpdateAsync(Especialidad especialidad);
    Task<bool> ToggleStatusAsync(int id);
}
