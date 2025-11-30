using Domain.Entities;

namespace Domain.Interfaces;

public interface IAgendaRepository
{
    Task<Agenda> UpdateAsync(Agenda agendaMapped);
}
