using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
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
        if (prestador.Rol == RolMedico.ProfesionalIndependiente && (prestador as Profesional)!.CentroId != null)
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
        prestador.Direcciones.ForEach(dir => dir.PrestadorId = prestador.Id);

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
        SyncChildCollection(prestadorDB.Telefonos, prestador.Telefonos, t => t.Id);
        SyncChildCollection(prestadorDB.Emails, prestador.Emails, e => e.Id);
        SyncChildCollection(prestadorDB.Documentacion, prestador.Documentacion, d => d.Id);
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
        List<Direccion> dirsDB = prestadorDB.Direcciones;

        List<Direccion> dirsReq = prestadorRequest.Direcciones;

        // Direcciones a eliminar: las que están en la DB pero no vienen en el request
        List<Direccion> dirsToRemove = dirsDB.Where(d => !dirsReq.Any(dr => dr.Id == d.Id)).ToList();

        // Direcciones nuevas: las que vienen en el request con Id == 0
        List<Direccion> dirsToAdd = dirsReq.Where(dr => dr.Id == 0).ToList();

        // Direcciones a actualizar: las que ya existen en la DB y vienen en el request
        List<Direccion> dirsToUpdate = dirsDB.Where(d => dirsReq.Any(dr => dr.Id == d.Id)).ToList();

        // Eliminamos primero las Agendas asociadas a las direcciones a eliminar
        foreach (var dir in dirsToRemove)
        {
            if (prestadorDB is CentroMedico centro)
            {
                var agendas = centro.Agendas.Where(a => a.DireccionId == dir.Id).ToList();
                foreach (var a in agendas) context.Remove(a);
            }
            if (prestadorDB is Profesional prof)
            {
                var agendas = prof.Agendas.Where(a => a.DireccionId == dir.Id).ToList();
                foreach (var a in agendas) context.Remove(a);
            }
        }
        // Removemos las direcciones
        foreach (var dir in dirsToRemove) prestadorDB.Direcciones.Remove(dir);

        // Agregamos las nuevas direcciones
        foreach (var dir in dirsToAdd) prestadorDB.Direcciones.Add(dir);

        // Agregamos las nuevas agendas asociadas a las direcciones nuevas
        foreach (var dir in dirsToAdd)
        {
            if (prestadorDB is CentroMedico centro)
            {
                var agendasReq = (prestadorRequest as CentroMedico)!.Agendas
                    .Where(a => a.DireccionId == dir.Id)
                    .ToList();
                foreach (var a in agendasReq)
                {
                    a.DireccionAtencion = dir;
                    centro.Agendas.Add(a);
                }
            }
            if (prestadorDB is Profesional prof)
            {
                var agendasReq = (prestadorRequest as Profesional)!.Agendas
                    .Where(a => a.DireccionId == dir.Id)
                    .ToList();
                foreach (var a in agendasReq)
                {
                    a.DireccionAtencion = dir;
                    prof.Agendas.Add(a);
                }
            }
        }

        // Actualizamos las existentes
        foreach (var dir in dirsToUpdate)
        {
            var dirReq = dirsReq.First(d => d.Id == dir.Id);
            context.Entry(dir).CurrentValues.SetValues(dirReq);
        }
    }

    private void SyncChildCollection<T>(ICollection<T> trackedCollection, ICollection<T> incomingCollection, Func<T, int> keySelector) where T : class
    {
        var incomingIds = incomingCollection.Where(x => keySelector(x) != 0).Select(keySelector).ToHashSet();
        var trackedIds = trackedCollection.Where(x => keySelector(x) != 0).Select(keySelector).ToList();

        // 1. Eliminar los que ya no están
        foreach (var trackedItem in trackedCollection.ToList())
        {
            if (keySelector(trackedItem) != 0 && !incomingIds.Contains(keySelector(trackedItem)))
                trackedCollection.Remove(trackedItem);
        }

        // 2. Actualizar los existentes
        foreach (var incomingItem in incomingCollection.Where(x => keySelector(x) != 0))
        {
            var trackedItem = trackedCollection.First(x => keySelector(x) == keySelector(incomingItem));
            context.Entry(trackedItem).CurrentValues.SetValues(incomingItem);
        }

        // 3. Agregar nuevos
        foreach (var incomingItem in incomingCollection.Where(x => keySelector(x) == 0))
        {
            trackedCollection.Add(incomingItem);
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
