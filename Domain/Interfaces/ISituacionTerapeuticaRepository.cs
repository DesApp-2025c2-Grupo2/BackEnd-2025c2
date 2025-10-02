using Domain.Entities;

namespace Domain.Interfaces;

public interface ISituacionTerapeuticaRepository
{
    Task<List<SituacionTerapeutica>> GetAllAsync();
    Task<SituacionTerapeutica> AddAsync(SituacionTerapeutica entity);
    Task<SituacionTerapeutica> UpdateAsync(SituacionTerapeutica entity);
    Task<SituacionTerapeutica> ToggleStatusAsync(SituacionTerapeutica entity);

}
