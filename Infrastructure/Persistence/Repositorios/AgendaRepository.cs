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
        // separamos las listas de horarios en 3 categorias: a eliminar, a actualizar y a agregar
        var horariosToDelete = agendaDB.Horarios
            .Where(hdb => !agendaReq.Horarios.Any(hr => hr.Id == hdb.Id))
            .ToList();
        var horariosToUpdate = agendaDB.Horarios
            .Where(hdb => agendaReq.Horarios.Any(hr => hr.Id == hdb.Id))
            .ToList();
        var horariosToAdd = agendaReq.Horarios
            .Where(hr => !agendaDB.Horarios.Any(hdb => hdb.Id == hr.Id))
            .ToList();

        // Eliminamos los horarios que ya no están en la solicitud
        context.RemoveRange(horariosToDelete);

        // Actualizamos los horarios existentes
        foreach (var horario in horariosToUpdate)
        {
            // Al actualizar los horarios, también debemos actualizar sus días de atención
            // evitando que queden registros huérfanos, lo que haremos será eliminar los días
            // y luego agregar los nuevos
            var horarioReq = agendaReq.Horarios.First(hr => hr.Id == horario.Id);
            context.Entry(horario).CurrentValues.SetValues(horarioReq);
            // Eliminamos los días de atención existentes
            context.RemoveRange(horario.DiasAtencion);
            // Agregamos los nuevos días de atención
            horario.DiasAtencion = horarioReq.DiasAtencion;

        }

        // Agregamos los nuevos horarios
        agendaDB.Horarios.AddRange(horariosToAdd);
    }

}
