using Domain.Entities;

namespace Domain.Interfaces;

public interface IEspecialidadRepository
{
    Task<List<Especialidad>> GetAllAsync();
    Task<Especialidad> AddAsync(Especialidad especialidad);
    Task<Especialidad> UpdateAsync(Especialidad especialidad);
    Task<bool> ToggleStatusAsync(int id);
}
