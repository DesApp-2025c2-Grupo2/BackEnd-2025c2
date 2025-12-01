using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositorios;

public class AgendaRepository : IAgendaRepository
{
    private readonly ProjectContext context;
    public AgendaRepository(ProjectContext projectContext)
    {
        context = projectContext;
    }

    public async Task ClearAsync(int id)
    {
        List<HorarioAtencion> horariosToDelete = await context.HorariosAtencion
            .Where(h => h.AgendaId == id)
            .Include(h => h.DiasAtencion)
            .ToListAsync();
        // Eliminamos los días de atención asociados a los horarios
        foreach (var horario in horariosToDelete)
        {
            context.RemoveRange(horario.DiasAtencion);
        }
        // Eliminamos los horarios
        context.RemoveRange(horariosToDelete);
        await context.SaveChangesAsync();
    }

    public async Task<Agenda> UpdateAsync(Agenda agendaMapped)
    {
        Agenda? agendaDB;
        if (agendaMapped is AgendaProfesional)
        {
            agendaDB = await context.AgendasProfesionales
                .Include(a => a.DireccionAtencion)
                .Include(a => a.Horarios).ThenInclude(h => h.DiasAtencion)
                .Include(a => a.Horarios).ThenInclude(h => h.Especialidad)
                .FirstOrDefaultAsync(a => a.Id == agendaMapped.Id);
        }
        else if (agendaMapped is AgendaCentroMedico)
        {
            agendaDB = await context.AgendasCentrosMedicos
                .Include(a => a.Horarios).ThenInclude(h => h.DiasAtencion)
                .Include(a => a.Horarios).ThenInclude(h => h.Especialidad)
                .Include(a => a.Horarios).ThenInclude(h => h.ProfesionalAsignado)
                .FirstOrDefaultAsync(a => a.Id == agendaMapped.Id);
        }
        else
        {
            throw new InvalidOperationException("Tipo de agenda no soportado.");
        }
        foreach (var horario in agendaMapped.Horarios)
            if (horario.Id != 0)
                context.Entry(horario).State = EntityState.Detached;

        await SyncHorariosAtencion(agendaDB!, agendaMapped);

        await context.SaveChangesAsync();
        return agendaDB;
    }

    private async Task SyncHorariosAtencion(Agenda agendaDB, Agenda agendaReq)
    {
        List<HorarioAtencion> horariosToDelete = agendaDB.Horarios
            .Where(hdb => !agendaReq.Horarios.Any(hr => hr.Id == hdb.Id))
            .ToList();

        List<HorarioAtencion> horariosToAdd = agendaReq.Horarios
            .Where(hr => !agendaDB.Horarios.Any(hdb => hdb.Id == hr.Id))
            .ToList();
        
        List<HorarioAtencion> horariosToUpdate = agendaDB.Horarios
            .Where(hdb => agendaReq.Horarios.Any(hr => hr.Id == hdb.Id))
            .ToList();

        // Eliminamos primero los dias de atencion de los horarios a eliminar
        foreach (var horario in horariosToDelete)
        {
            context.RemoveRange(horario.DiasAtencion);
        }
        // Eliminamos los horarios que ya no están en la solicitud
        context.RemoveRange(horariosToDelete);
        // Actualizamos los horarios existentes
        foreach (var horario in horariosToUpdate)
        {
            var horarioReq = agendaReq.Horarios.First(hr => hr.Id == horario.Id);
            horarioReq.AgendaId = agendaDB.Id;
            context.Entry(horario).CurrentValues.SetValues(horarioReq);
            // Eliminamos los días de atención existentes
            context.RemoveRange(horario.DiasAtencion);
            // Actualizamos los días de atención
            horarioReq.DiasAtencion.ForEach(dia => horario.DiasAtencion.Add(new HorarioDia { Dia = dia.Dia }));
        }

        // Agregamos los nuevos horarios
        foreach (var horario in horariosToAdd)
        {
            horario.AgendaId = agendaDB.Id;
            context.HorariosAtencion.Add(horario);
        }

    }

}
