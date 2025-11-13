using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositorios;

public class PrestadorRepository : IPrestadorRepository
{
    private readonly ProjectContext context;
    public PrestadorRepository(ProjectContext context)
    {
        this.context = context;
    }

    public async Task AddAsync(Prestador prestador)
    {
        await context.Prestadores.AddAsync(prestador);
    }

    public async Task<IEnumerable<Prestador>> GetAllAsync()
    {
        return await context.Prestadores
            .Include(p => p.Telefonos)
            .Include(p => p.Emails)
            .Include(p => p.Documentaciones)
            .Include(p => p.Direcciones)
            .Include(p => p.Especialidades)
            .ToListAsync();
    }

    public async Task<Prestador?> GetByIdAsync(int id)
    {
        return await context.Prestadores.FindAsync(id);
    }

    public async Task<Prestador?> GetByIdWithDetailsAsync(int id)
    {
        return await context.Prestadores
            .Include(p => p.Telefonos)
            .Include(p => p.Emails)
            .Include(p => p.Documentaciones)
            .Include(p => p.Direcciones)
            .Include(p => p.Especialidades)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task UpdateAsync(Prestador prestador)
    {
        context.Prestadores.Update(prestador);
        await context.SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }

    public async Task<List<Especialidad>> GetEspecialidadesByIdsAsync(IEnumerable<int> ids)
    {
        return await context.Especialidades.Where(e => ids.Contains(e.Id)).ToListAsync();
    }

    public async Task<Agenda?> GetAgendaAsync(int profesionalId, int especialidadId, string direccion)
    {
        return await context.Agendas.FirstOrDefaultAsync(a => a.ProfesionalId == profesionalId && a.EspecialidadId == especialidadId && a.Direccion == direccion);
    }

    public async Task<Agenda?> GetAgendaByIdAsync(int agendaId)
    {
        return await context.Agendas.FirstOrDefaultAsync(a => a.Id == agendaId);
    }

    public async Task AddAgendaAsync(Agenda agenda)
    {
        await context.Agendas.AddAsync(agenda);
    }

    public async Task<List<HorarioAtencion>> GetHorariosByAgendaAsync(int agendaId)
    {
        return await context.HorariosAtencion.Where(h => h.AgendaId == agendaId).ToListAsync();
    }

    public async Task ReplaceHorariosAsync(int agendaId, List<HorarioAtencion> nuevos)
    {
        var existentes = await context.HorariosAtencion.Where(h => h.AgendaId == agendaId).ToListAsync();
        if (existentes.Count > 0)
        {
            context.HorariosAtencion.RemoveRange(existentes);
        }
        if (nuevos.Count > 0)
        {
            await context.HorariosAtencion.AddRangeAsync(nuevos);
        }
    }

    public async Task<HorarioAtencion?> GetHorarioByIdAsync(int horarioId)
    {
        return await context.HorariosAtencion.FirstOrDefaultAsync(h => h.Id == horarioId);
    }

    public async Task DeleteHorariosByTramoAsync(int agendaId, TimeSpan inicio, TimeSpan fin)
    {
        var existentes = await context.HorariosAtencion
            .Where(h => h.AgendaId == agendaId
                        && h.HoraInicio.Hour == inicio.Hours && h.HoraInicio.Minute == inicio.Minutes
                        && h.HoraFin.Hour == fin.Hours && h.HoraFin.Minute == fin.Minutes)
            .ToListAsync();
        if (existentes.Count > 0)
        {
            context.HorariosAtencion.RemoveRange(existentes);
            await context.SaveChangesAsync();
        }
    }

    public async Task AddHorariosAsync(List<HorarioAtencion> nuevos)
    {
        if (nuevos.Count == 0) return;
        await context.HorariosAtencion.AddRangeAsync(nuevos);
        await context.SaveChangesAsync();
    }

    public async Task UpdateHorarioAsync(HorarioAtencion horario)
    {
        context.HorariosAtencion.Update(horario);
        await context.SaveChangesAsync();
    }

    public async Task DeleteHorariosByIdsAsync(List<int> ids)
    {
        if (ids == null || ids.Count == 0) return;
        var existentes = await context.HorariosAtencion.Where(h => ids.Contains(h.Id)).ToListAsync();
        if (existentes.Count == 0) return;
        context.HorariosAtencion.RemoveRange(existentes);
        await context.SaveChangesAsync();
    }

    public async Task<List<HorarioAtencion>> GetHorariosByAgendaAndTramoAsync(int agendaId, TimeSpan inicio, TimeSpan fin)
    {
        return await context.HorariosAtencion
            .Where(h => h.AgendaId == agendaId
                        && h.HoraInicio.Hour == inicio.Hours && h.HoraInicio.Minute == inicio.Minutes
                        && h.HoraFin.Hour == fin.Hours && h.HoraFin.Minute == fin.Minutes)
            .ToListAsync();
    }

    public async Task<List<Agenda>> GetAgendasByProfesionalAsync(int profesionalId)
    {
        return await context.Agendas.Where(a => a.ProfesionalId == profesionalId).ToListAsync();
    }

    public async Task<List<Agenda>> GetAgendasByProfesionalAndDireccionAsync(int profesionalId, string direccion)
    {
        return await context.Agendas
            .Where(a => a.ProfesionalId == profesionalId && a.Direccion == direccion)
            .ToListAsync();
    }

    public async Task DeleteAllHorariosByAgendaIdsAsync(List<int> agendaIds)
    {
        if (agendaIds == null || agendaIds.Count == 0) return;
        var existentes = await context.HorariosAtencion.Where(h => agendaIds.Contains(h.AgendaId)).ToListAsync();
        if (existentes.Count == 0) return;
        context.HorariosAtencion.RemoveRange(existentes);
        await context.SaveChangesAsync();
    }
}


