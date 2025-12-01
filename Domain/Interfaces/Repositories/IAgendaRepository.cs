using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IAgendaRepository
{
    Task ClearAsync(int id);
    Task<Agenda> UpdateAsync(Agenda agendaMapped);
}
