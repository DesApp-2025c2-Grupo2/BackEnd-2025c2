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
        .Include(p => (p as CentroMedico)!.Agendas).ThenInclude(a => a.Horarios).ThenInclude(h => h.DiasAtencion)

        .Include(p => (p as Profesional)!.Centro)
        .Include(p => (p as Profesional)!.Agendas).ThenInclude(a => a.DireccionAtencion)
        .Include(p => (p as Profesional)!.Agendas).ThenInclude(a => a.Horarios).ThenInclude(h => h.Especialidad)
        .Include(p => (p as Profesional)!.Agendas).ThenInclude(a => a.Horarios).ThenInclude(h => h.DiasAtencion)
        .AsSplitQuery()
        .ToListAsync();

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
        Prestador? prestadorDB = await GetByIdAsync(prestador.Id);
        if (prestadorDB == null) throw new KeyNotFoundException();

        // Actualizar los valores simples
        context.Entry(prestadorDB).CurrentValues.SetValues(prestador);
        prestador.Documentacion.ForEach(doc => doc.PrestadorId = prestador.Id);
        prestador.Telefonos.ForEach(tel => tel.PrestadorId = prestador.Id);
        prestador.Emails.ForEach(mail => mail.PrestadorId = prestador.Id);

        // Preparo colecciones del request para que EF no las trackee
        foreach (var doc in prestador.Documentacion)
            if (doc.Id != 0)
                context.Entry(doc).State = EntityState.Detached;

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



    #region Privates
    private async Task<Prestador?> GetByIdAsync(int prestadorId)
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
            .Include(p => (p as CentroMedico)!.Agendas).ThenInclude(a => a.Horarios).ThenInclude(h => h.DiasAtencion)
            .Include(p => (p as Profesional)!.Centro)
            .Include(p => (p as Profesional)!.Agendas).ThenInclude(a => a.DireccionAtencion)
            .Include(p => (p as Profesional)!.Agendas).ThenInclude(a => a.Horarios).ThenInclude(h => h.Especialidad)
            .Include(p => (p as Profesional)!.Agendas).ThenInclude(a => a.Horarios).ThenInclude(h => h.DiasAtencion)
            .AsSplitQuery()
            .FirstOrDefaultAsync(p => p.Id == prestadorId);
        return prestador;
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

        // Ahora cargamos las agendas que no estaban en la db pero vienen en el request
        if (prestadorRequest is CentroMedico centroReq && prestadorDB is CentroMedico centroDB)
        {
            var agendasDB = centroDB.Agendas.ToDictionary(a => a.Id);
            centroDB.Agendas.Clear();
            foreach (var agendaReq in centroReq.Agendas)
            {
                if (agendaReq.Id == 0)
                {
                    // Nueva
                    centroDB.Agendas.Add(agendaReq);
                }
                else
                {
                    // Ya existe → usar instancia trackeada
                    var agendaDB = agendasDB[agendaReq.Id];
                    context.Entry(agendaDB).CurrentValues.SetValues(agendaReq);
                    centroDB.Agendas.Add(agendaDB);
                }
            }
            // Borrar agendas que ya no están
            var idsAgendasReq = centroReq.Agendas.Where(a => a.Id != 0).Select(a => a.Id).ToHashSet();
            var agendasToDelete = agendasDB.Keys.Where(id => !idsAgendasReq.Contains(id)).ToList();
            foreach (var id in agendasToDelete)
            {
                var a = agendasDB[id];
                context.Remove(a);
            }
        }

        if (prestadorRequest is Profesional profReq && prestadorDB is Profesional profDB)
        {
            // puede que profReq.Agendas tenga agendas que ya existen en la db, pero no tiene el atributo escalar de EF cargado
            // es decir que la agenda del req no tiene el id, apesar de que ya existe en la db, y que debe seguir existiendo
            var agendasDirReq = profReq.Agendas.ToDictionary(ag => ag.DireccionId);
            profDB.Agendas.ForEach(agdb =>
            {
                if (agendasDirReq.ContainsKey(agdb.DireccionId))
                {
                    var agReq = agendasDirReq[agdb.DireccionId];
                    agReq.Id = agdb.Id;
                }
            });

            var agendasDB = profDB.Agendas.ToDictionary(a => a.Id);
            profDB.Agendas.Clear();
            foreach (var agendaReq in profReq.Agendas)
            {
                if (agendaReq.Id == 0)
                {
                    // Nueva
                    profDB.Agendas.Add(agendaReq);
                }
                else
                {
                    // Ya existe → usar instancia trackeada
                    var agendaDB = agendasDB[agendaReq.Id];
                    context.Entry(agendaDB).CurrentValues.SetValues(agendaReq);
                    profDB.Agendas.Add(agendaDB);
                }
            }
            // Borrar agendas que ya no están
            var idsAgendasReq = profReq.Agendas.Where(a => a.Id != 0).Select(a => a.Id).ToHashSet();
            var agendasToDelete = agendasDB.Keys.Where(id => !idsAgendasReq.Contains(id)).ToList();
            foreach (var id in agendasToDelete)
            {
                var a = agendasDB[id];
                context.Remove(a);
            }
        }
        else
        {
            // No es ni CentroMedico ni Profesional
            throw new Exception("El prestador no es ni CentroMedico ni Profesional.");
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

    #endregion

}
