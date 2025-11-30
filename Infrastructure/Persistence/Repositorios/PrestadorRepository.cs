using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositorios;

//public class PrestadorRepository : IPrestadorRepository
//{
//    private readonly ProjectContext context;
//    public PrestadorRepository(ProjectContext context)
//    {
//        this.context = context;
//    }

//    public async Task AddAsync(Prestador prestador)
//    {
//        await context.Prestadores.AddAsync(prestador);
//    }

//    public async Task<IEnumerable<Prestador>> GetAllAsync()
//    {
//        return await context.Prestadores
//            .Include(p => p.Profesionales)
//            .Include(p => p.Telefonos)
//            .Include(p => p.Emails)
//            .Include(p => p.Documentaciones)
//            .Include(p => p.Direcciones)
//            .Include(p => p.Especialidades)
//            .ToListAsync();
//    }

//    public async Task<Prestador?> GetByIdAsync(int id)
//    {
//        return await context.Prestadores.FindAsync(id);
//    }

//    public async Task<Prestador?> GetByIdWithDetailsAsync(int id)
//    {
//        return await context.Prestadores
//            .Include(p => p.Profesionales)
//            .Include(p => p.Telefonos)
//            .Include(p => p.Emails)
//            .Include(p => p.Documentaciones)
//            .Include(p => p.Direcciones)
//            .Include(p => p.Especialidades)
//            .FirstOrDefaultAsync(p => p.Id == id);
//    }



//    public async Task UpdateAsync(Prestador prestador)
//    {
//        context.Prestadores.Update(prestador);
//        await context.SaveChangesAsync();
//    }

//    public async Task SaveChangesAsync()
//    {
//        await context.SaveChangesAsync();
//    }

//    public async Task<List<Especialidad>> GetEspecialidadesByIdsAsync(IEnumerable<int> ids)
//    {
//        return await context.Especialidades.Where(e => ids.Contains(e.Id)).ToListAsync();
//    }

//    public async Task<Agenda?> GetAgendaAsync(int profesionalId, int especialidadId, string direccion)
//    {
//        return await context.Agendas.FirstOrDefaultAsync(a => a.ProfesionalId == profesionalId && a.EspecialidadId == especialidadId && a.Direccion == direccion);
//    }

//    public async Task<Agenda?> GetAgendaByIdAsync(int agendaId)
//    {
//        return await context.Agendas.FirstOrDefaultAsync(a => a.Id == agendaId);
//    }

//    public async Task AddAgendaAsync(Agenda agenda)
//    {
//        await context.Agendas.AddAsync(agenda);
//    }

//    public async Task<List<HorarioAtencion>> GetHorariosByAgendaAsync(int agendaId)
//    {
//        return await context.HorariosAtencion.Where(h => h.AgendaId == agendaId).ToListAsync();
//    }

//    public async Task ReplaceHorariosAsync(int agendaId, List<HorarioAtencion> nuevos)
//    {
//        var existentes = await context.HorariosAtencion.Where(h => h.AgendaId == agendaId).ToListAsync();
//        if (existentes.Count > 0)
//        {
//            context.HorariosAtencion.RemoveRange(existentes);
//        }
//        if (nuevos.Count > 0)
//        {
//            await context.HorariosAtencion.AddRangeAsync(nuevos);
//        }
//    }

//    public async Task<HorarioAtencion?> GetHorarioByIdAsync(int horarioId)
//    {
//        return await context.HorariosAtencion.FirstOrDefaultAsync(h => h.Id == horarioId);
//    }

//    public async Task DeleteHorariosByTramoAsync(int agendaId, TimeSpan inicio, TimeSpan fin)
//    {
//        var existentes = await context.HorariosAtencion
//            .Where(h => h.AgendaId == agendaId
//                        && h.HoraInicio.Hour == inicio.Hours && h.HoraInicio.Minute == inicio.Minutes
//                        && h.HoraFin.Hour == fin.Hours && h.HoraFin.Minute == fin.Minutes)
//            .ToListAsync();
//        if (existentes.Count > 0)
//        {
//            context.HorariosAtencion.RemoveRange(existentes);
//            await context.SaveChangesAsync();
//        }
//    }

//    public async Task AddHorariosAsync(List<HorarioAtencion> nuevos)
//    {
//        if (nuevos.Count == 0) return;
//        await context.HorariosAtencion.AddRangeAsync(nuevos);
//        await context.SaveChangesAsync();
//    }

//    public async Task UpdateHorarioAsync(HorarioAtencion horario)
//    {
//        context.HorariosAtencion.Update(horario);
//        await context.SaveChangesAsync();
//    }

//    public async Task DeleteHorariosByIdsAsync(List<int> ids)
//    {
//        if (ids == null || ids.Count == 0) return;
//        var existentes = await context.HorariosAtencion.Where(h => ids.Contains(h.Id)).ToListAsync();
//        if (existentes.Count == 0) return;
//        context.HorariosAtencion.RemoveRange(existentes);
//        await context.SaveChangesAsync();
//    }

//    public async Task<List<HorarioAtencion>> GetHorariosByAgendaAndTramoAsync(int agendaId, TimeSpan inicio, TimeSpan fin)
//    {
//        return await context.HorariosAtencion
//            .Where(h => h.AgendaId == agendaId
//                        && h.HoraInicio.Hour == inicio.Hours && h.HoraInicio.Minute == inicio.Minutes
//                        && h.HoraFin.Hour == fin.Hours && h.HoraFin.Minute == fin.Minutes)
//            .ToListAsync();
//    }

//    public async Task<List<Agenda>> GetAgendasByProfesionalAsync(int profesionalId)
//    {
//        return await context.Agendas.Where(a => a.ProfesionalId == profesionalId).ToListAsync();
//    }

//    public async Task<List<Agenda>> GetAgendasByProfesionalesAsync(IEnumerable<int> profesionalIds)
//    {
//        var ids = profesionalIds?.Distinct().ToList() ?? new List<int>();
//        if (ids.Count == 0) return new List<Agenda>();
//        return await context.Agendas.Where(a => ids.Contains(a.ProfesionalId)).ToListAsync();
//    }

//    public async Task<List<HorarioAtencion>> GetHorariosByAgendasAsync(IEnumerable<int> agendaIds)
//    {
//        var ids = agendaIds?.Distinct().ToList() ?? new List<int>();
//        if (ids.Count == 0) return new List<HorarioAtencion>();
//        return await context.HorariosAtencion.Where(h => ids.Contains(h.AgendaId)).ToListAsync();
//    }

//    public async Task<List<Agenda>> GetAgendasByProfesionalAndDireccionAsync(int profesionalId, string direccion)
//    {
//        return await context.Agendas
//            .Where(a => a.ProfesionalId == profesionalId && a.Direccion == direccion)
//            .ToListAsync();
//    }

//    public async Task DeleteAllHorariosByAgendaIdsAsync(List<int> agendaIds)
//    {
//        if (agendaIds == null || agendaIds.Count == 0) return;
//        var existentes = await context.HorariosAtencion.Where(h => agendaIds.Contains(h.AgendaId)).ToListAsync();
//        if (existentes.Count == 0) return;
//        context.HorariosAtencion.RemoveRange(existentes);
//        await context.SaveChangesAsync();
//    }

//    public async Task UpdateDireccionTextoForProfesionalAsync(int profesionalId, string oldDireccion, string newDireccion)
//    {
//        var oldTxt = (oldDireccion ?? string.Empty).Trim();
//        var newTxt = (newDireccion ?? string.Empty).Trim();
//        if (string.IsNullOrWhiteSpace(oldTxt) || oldTxt == newTxt) return;

//        var agendas = await context.Agendas
//            .Where(a => a.ProfesionalId == profesionalId && a.Direccion == oldTxt)
//            .ToListAsync();

//        if (agendas.Count == 0) return;

//        foreach (var agenda in agendas)
//        {
//            agenda.Direccion = newTxt;
//        }

//        await context.SaveChangesAsync();
//    }
//}


public class PrestadorRepository : IPrestadorRepository
{
    private readonly ProjectContext context;
    public PrestadorRepository(ProjectContext context)
    {
        this.context = context;
    }

    public async Task<Prestador> CreateAsync(Prestador prestador, List<int> especialidadesIds)
    {
        // Asignar las especialidades al prestador
        if (especialidadesIds != null && especialidadesIds.Count > 0)
        {
            var especialidades = await context.Especialidades.Where(e => especialidadesIds.Contains(e.Id)).ToListAsync();
            prestador.Especialidades = especialidades;
        }
        if ((prestador as Profesional)!.CentroId != null)
        {
            CentroMedico? centro = await context.Prestadores
                .OfType<CentroMedico>()
                .FirstOrDefaultAsync(c => c.Id == (prestador as Profesional)!.CentroId) ?? throw new Exception("El centro médico asignado no existe.");
            (prestador as Profesional)!.Centro = centro;
        }
        await context.Prestadores.AddAsync(prestador);
        await context.SaveChangesAsync();
        return prestador;
    }

    public Task<List<Prestador>> GetAllAsync() => context.Prestadores
        .Include(p => p.Telefonos)
        .Include(p => p.Emails)
        .Include(p => p.Documentacion)
        .Include(p => p.Direcciones)
        .Include(p => p.Especialidades)

        .Include(p => (p as CentroMedico)!.Profesionales)
        .Include(p => (p as CentroMedico)!.Agendas).ThenInclude(a => a.DireccionAtencion)
        .Include(p => (p as CentroMedico)!.Agendas).ThenInclude(a => a.Horarios).ThenInclude(h => h.Especialidad)
        .Include(p => (p as CentroMedico)!.Agendas).ThenInclude(a => a.Horarios).ThenInclude(h => h.ProfesionalAsignado)

        .Include(p => (p as Profesional)!.Centro)
        .Include(p => (p as Profesional)!.Agendas).ThenInclude(a => a.DireccionAtencion)
        .Include(p => (p as Profesional)!.Agendas).ThenInclude(a => a.Horarios).ThenInclude(h => h.Especialidad)
        .AsSplitQuery()
        .ToListAsync();

    public async Task<Prestador?> GetByIdAsync(int prestadorId)
    {
        Prestador? prestador = await context.Prestadores
            .Include(p => p.Telefonos)
            .Include(p => p.Emails)
            .Include(p => p.Documentacion)
            .Include(p => p.Direcciones)
            .Include(p => p.Especialidades)
            .Include(p => (p as CentroMedico)!.Profesionales).ThenInclude(pr => pr.Especialidades)
            .Include(p => (p as CentroMedico)!.Agendas).ThenInclude(a => a.DireccionAtencion)
            .Include(p => (p as CentroMedico)!.Agendas).ThenInclude(a => a.Horarios).ThenInclude(h => h.Especialidad)
            .Include(p => (p as CentroMedico)!.Agendas).ThenInclude(a => a.Horarios).ThenInclude(h => h.ProfesionalAsignado)
            .Include(p => (p as Profesional)!.Centro)
            .Include(p => (p as Profesional)!.Agendas).ThenInclude(a => a.DireccionAtencion)
            .Include(p => (p as Profesional)!.Agendas).ThenInclude(a => a.Horarios).ThenInclude(h => h.Especialidad)
            .AsSplitQuery()
            .FirstOrDefaultAsync(p => p.Id == prestadorId);
        return prestador;
    }

    public async Task<bool> ToggleStatusAsync(int id)
    {

        Prestador? prestador = await context.Prestadores.FirstOrDefaultAsync(p => p.Id == id);
        
        if (prestador == null) throw new KeyNotFoundException();
        DateTime? fechaBaja = prestador.Baja;
        // Si la baja es null, se da de baja
        if (fechaBaja == null)
        {
            prestador.Baja = DateTime.Today;
        }
        else
        {
            prestador.Baja = null;
            prestador.Alta = DateTime.Today;
        }

        await context.SaveChangesAsync();
        
        return prestador.Baja == null;
    }

    public async Task<Prestador> UpdateAsync(Prestador prestador, List<int> especialidadesIds)
    {
        Prestador? prestadorDB = await context.Prestadores
            .Include(p => p.Telefonos)
            .Include(p => p.Emails)
            .Include(p => p.Documentacion)
            .Include(p => p.Direcciones)
            .Include(p => p.Especialidades)
            .Include(p => (p as CentroMedico)!.Profesionales).ThenInclude(pr => pr.Especialidades)
            .Include(p => (p as CentroMedico)!.Agendas).ThenInclude(a => a.DireccionAtencion)
            .Include(p => (p as CentroMedico)!.Agendas).ThenInclude(a => a.Horarios).ThenInclude(h => h.Especialidad)
            .Include(p => (p as CentroMedico)!.Agendas).ThenInclude(a => a.Horarios).ThenInclude(h => h.ProfesionalAsignado)
            .Include(p => (p as Profesional)!.Centro)
            .Include(p => (p as Profesional)!.Agendas).ThenInclude(a => a.DireccionAtencion)
            .Include(p => (p as Profesional)!.Agendas).ThenInclude(a => a.Horarios).ThenInclude(h => h.Especialidad)
            .AsSplitQuery()
            .FirstOrDefaultAsync(p => p.Id == prestador.Id);
        if (prestadorDB == null) throw new KeyNotFoundException();
        // Actualizar los valores simples
        context.Entry(prestadorDB).CurrentValues.SetValues(prestador);
        // Preparo colecciones del request para que EF no las trackee
        foreach (var dir in prestador.Direcciones)
            if (dir.Id != 0)
                context.Entry(dir).State = EntityState.Detached;

        foreach (var tel in prestador.Telefonos)
            if (tel.Id != 0)
                context.Entry(tel).State = EntityState.Detached;

        foreach (var mail in prestador.Emails)
            if (mail.Id != 0)
                context.Entry(mail).State = EntityState.Detached;

        // Actualizar las colecciones relacionadas
        SyncCollection(prestadorDB.Telefonos, prestador.Telefonos, t => t.Id);
        SyncCollection(prestadorDB.Emails, prestador.Emails, e => e.Id);
        SyncCollection(prestadorDB.Documentacion, prestador.Documentacion, d => d.Id);
        await SyncDireccionesAgendas(prestadorDB, prestador);
        await SyncEspecialidades(prestadorDB, especialidadesIds);
        
        await context.SaveChangesAsync();
        return prestadorDB;
    }

    private async Task SyncDireccionesAgendas(Prestador prestadorDB, Prestador prestadorRequest)
    {
        var dirsDB = prestadorDB.Direcciones.ToDictionary(d => d.Id);
        prestadorDB.Direcciones.Clear();

        foreach (var dirReq in prestadorRequest.Direcciones)
        {
            if (dirReq.Id == 0)
            {
                // Nueva
                prestadorDB.Direcciones.Add(dirReq);
            }
            else
            {
                // Ya existe → usar instancia trackeada
                var dirDB = dirsDB[dirReq.Id];

                context.Entry(dirDB).CurrentValues.SetValues(dirReq);

                prestadorDB.Direcciones.Add(dirDB);
            }
        }

        // Borrar direcciones que ya no están
        var idsReq = prestadorRequest.Direcciones.Where(d => d.Id != 0).Select(d => d.Id).ToHashSet();
        var dirsToDelete = dirsDB.Keys.Where(id => !idsReq.Contains(id)).ToList();

        foreach (var id in dirsToDelete)
        {
            var d = dirsDB[id];

            // Borrar agendas
            if (prestadorDB is CentroMedico centro)
            {
                var agendas = centro.Agendas.Where(a => a.DireccionId == id).ToList();
                foreach (var a in agendas) context.Remove(a);
            }
            if (prestadorDB is Profesional prof)
            {
                var agendas = prof.Agendas.Where(a => a.DireccionId == id).ToList();
                foreach (var a in agendas) context.Remove(a);
            }

            context.Remove(d);
        }



    }

    private void SyncCollection<T>(ICollection<T> dbCollection, ICollection<T> newCollection, Func<T, int> getId)
    where T : class
    {
        // 1. Borrar los que no vienen en el request
        var toRemove = dbCollection
            .Where(dbItem => !newCollection.Any(n => getId(n) == getId(dbItem)))
            .ToList();

        foreach (var item in toRemove)
            dbCollection.Remove(item);

        // 2. Insertar los nuevos (id == 0)
        var toAdd = newCollection
            .Where(n => getId(n) == 0)
            .ToList();

        foreach (var item in toAdd)
            dbCollection.Add(item);

        // 3. Actualizar los existentes
        foreach (var newItem in newCollection.Where(n => getId(n) != 0))
        {
            var dbItem = dbCollection.First(x => getId(x) == getId(newItem));
            context.Entry(dbItem).CurrentValues.SetValues(newItem);
        }
    }

    private async Task SyncEspecialidades(Prestador prestadorDB, List<int> newIds)
    {
        var actuales = prestadorDB.Especialidades.Select(e => e.Id).ToList();

        // Quitar las que no están
        var aEliminar = prestadorDB.Especialidades
            .Where(e => !newIds.Contains(e.Id))
            .ToList();

        foreach (var esp in aEliminar)
            prestadorDB.Especialidades.Remove(esp);

        // Agregar las nuevas
        var idsAAgregar = newIds.Except(actuales).ToList();

        if (idsAAgregar.Count > 0)
        {
            var nuevas = await context.Especialidades
                .Where(e => idsAAgregar.Contains(e.Id))
                .ToListAsync();

            foreach (var esp in nuevas)
                prestadorDB.Especialidades.Add(esp);
        }
    }
}
