using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IAgendaRepository
{
    Task<Agenda> UpdateAsync(Agenda agendaMapped);
}
