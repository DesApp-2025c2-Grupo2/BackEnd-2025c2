using Domain.Entities;

namespace Domain.Interfaces;

public interface IPlanMedicoRepository
{
    Task<List<PlanMedico>> GetAllAsync();
    Task<PlanMedico> AddAsync(PlanMedico planMedico);
    Task<PlanMedico> UpdateAsync(PlanMedico planMedico);
    Task<bool> ToggleStatusAsync(int id);
}
