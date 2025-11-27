using Domain.Entities;
using System.Collections.Generic;

namespace Domain.Interfaces;

public interface IPrestadorRepository
{
    Task AddAsync(Prestador prestador);
    Task UpdateAsync(Prestador prestador);
    Task<IEnumerable<Prestador>> GetAllAsync();

    Task<Prestador?> GetByIdAsync(int id);
    Task<Prestador?> GetByIdWithDetailsAsync(int id);
    Task SaveChangesAsync();

    Task<List<Especialidad>> GetEspecialidadesByIdsAsync(IEnumerable<int> ids);

    Task<Agenda?> GetAgendaAsync(int profesionalId, int especialidadId, string direccion);
    Task<Agenda?> GetAgendaByIdAsync(int agendaId);
    Task AddAgendaAsync(Agenda agenda);
    Task<List<HorarioAtencion>> GetHorariosByAgendaAsync(int agendaId);
    Task ReplaceHorariosAsync(int agendaId, List<HorarioAtencion> nuevos);
    Task<List<Agenda>> GetAgendasByProfesionalAsync(int profesionalId);
    Task<List<Agenda>> GetAgendasByProfesionalesAsync(IEnumerable<int> profesionalIds);
    Task<List<HorarioAtencion>> GetHorariosByAgendasAsync(IEnumerable<int> agendaIds);
    Task<HorarioAtencion?> GetHorarioByIdAsync(int horarioId);
    Task DeleteHorariosByTramoAsync(int agendaId, TimeSpan inicio, TimeSpan fin);
    Task AddHorariosAsync(List<HorarioAtencion> nuevos);
    Task UpdateHorarioAsync(HorarioAtencion horario);
    Task DeleteHorariosByIdsAsync(List<int> ids);
    Task<List<HorarioAtencion>> GetHorariosByAgendaAndTramoAsync(int agendaId, TimeSpan inicio, TimeSpan fin);

    // Helpers por direccion
    Task<List<Agenda>> GetAgendasByProfesionalAndDireccionAsync(int profesionalId, string direccion);
    Task DeleteAllHorariosByAgendaIdsAsync(List<int> agendaIds);
    Task UpdateDireccionTextoForProfesionalAsync(int profesionalId, string oldDireccion, string newDireccion);
}
