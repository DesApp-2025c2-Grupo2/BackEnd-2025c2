using Domain.Entities;
using Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Domain.Interfaces;

namespace Infrastructure.Persistence.Repositorios;

public class AfiliadoRepository : IAfiliadoRepository
{
    private readonly ProjectContext _context;
    public AfiliadoRepository(ProjectContext context)
    {
        _context = context;
    }

    // REMOVED //
    //public async Task<Afiliado> GetByIdAsync(int id)
    //{
    //    return await _context.Afiliados
    //        .Include(a => a.Integrantes)
    //            .ThenInclude(p => p.Telefonos)
    //        .Include(a => a.Integrantes)
    //            .ThenInclude(p => p.Emails)
    //        .Include(a => a.Integrantes)
    //            .ThenInclude(p => p.Documentacion)
    //        .Include(a => a.Integrantes)
    //            .ThenInclude(p => p.Direcciones)
    //        .FirstOrDefaultAsync(a => a.Id == id);
    //}
    //public async Task<Afiliado> GetByNumeroAsync(int numeroAfiliado)
    //{
    //    return await _context.Afiliados
    //        .Include(a => a.Integrantes)
    //            .ThenInclude(p => p.Telefonos)
    //        .Include(a => a.Integrantes)
    //            .ThenInclude(p => p.Emails)
    //        .Include(a => a.Integrantes)
    //            .ThenInclude(p => p.Documentacion)
    //        .Include(a => a.Integrantes)
    //            .ThenInclude(p => p.Direcciones)
    //        .FirstOrDefaultAsync(a => a.NumeroAfiliado == numeroAfiliado);
    //}

    public async Task<List<Afiliado>> GetAllAsync()
    {
        return await _context.Afiliados
            .Include(a => a.Integrantes)
                .ThenInclude(p => p.Telefonos)
            .Include(a => a.Integrantes)
                .ThenInclude(p => p.Emails)
            .Include(a => a.Integrantes)
                .ThenInclude(p => p.Documentacion)
            .Include(a => a.Integrantes)
                .ThenInclude(p => p.Direcciones)
            .Include(a => a.Integrantes)
                .ThenInclude(p => p.SituacionesTerapeuticas.Where(st => st.FechaFin == null || st.FechaFin > DateTime.Now.Date)).ThenInclude(st => st.SituacionTerapeutica)
            .ToListAsync();
    }

    // OLD VERSION //
    //public async Task AddAsync(Afiliado afiliado)
    //{
    //    await _context.Afiliados.AddAsync(afiliado);
    //}
    
    /// <summary>
    /// Agrega un nuevo afiliado junto con su titular y asigna un número de afiliado único.
    /// </summary>
    /// <param name="afiliado"></param>
    /// <returns></returns>
    public async Task AddAsync(Afiliado afiliado, Dictionary<int,DateTime?> situacionesTerapeuticasTitular)
    {
        // extraemos el integrante 1 de la lista
        Persona titular = afiliado.Integrantes[0];
        afiliado.Integrantes.Clear();
        var maxNumeroAfiliado = await _context.Afiliados.MaxAsync(a => (int?)a.NumeroAfiliado) ?? 0;
        afiliado.NumeroAfiliado = maxNumeroAfiliado + 1;
        await _context.Afiliados.AddAsync(afiliado);
        await _context.SaveChangesAsync();
        afiliado.Integrantes.Add(titular);
        List<SituacionTerapeutica> situaciones = await _context.SituacionesTerapeuticas
            .Where(st => situacionesTerapeuticasTitular.Keys.Contains(st.Id))
            .ToListAsync();
        afiliado.Integrantes[0].SituacionesTerapeuticas = situaciones.Select(st => new RegistroTerapeutico
        {
            SituacionTerapeuticaId = st.Id,
            FechaInicio = DateTime.Now,
            FechaFin = situacionesTerapeuticasTitular[st.Id]
        }).ToList();
        await _context.Personas.AddAsync(afiliado.Integrantes[0]);
        await _context.SaveChangesAsync();

        afiliado.TitularID = afiliado.Integrantes[0].Id;
        _context.Afiliados.Update(afiliado);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Afiliado afiliado)
    {
        var afiliadoEntity = await _context.Afiliados.FirstOrDefaultAsync(a => a.Id == afiliado.Id);
        if (afiliadoEntity == null)
        {
            throw new KeyNotFoundException($"Afiliado no encontrado.");
        }
        _context.Entry(afiliadoEntity).CurrentValues.SetValues(afiliado);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ToggleStatus(int afiliadoID, bool activo, DateTime? fecha)
    {
        var afiliado = await _context.Afiliados.Include(a => a.Integrantes).FirstOrDefaultAsync(a => a.Id == afiliadoID);
        if (afiliado == null)
        {
            throw new KeyNotFoundException($"Afiliado no encontrado.");
        }


        // DEBUG: Ver qué valor está recibiendo realmente
        Console.WriteLine($"DEBUG: afiliadoID={afiliadoID}, activo={activo}, fecha={fecha}, fecha.HasValue={fecha.HasValue}");
        if (fecha.HasValue)
        {
            Console.WriteLine($"DEBUG: fecha.Value={fecha.Value}");
        }
        // Para BAJA: si activo es false y fecha es null, establecer baja como null
        if (!activo && fecha == null)
        {
            afiliado.Baja = null;
            foreach (var integrante in afiliado.Integrantes)
            {
                integrante.Baja = null;
            }
        }
        else
        {
            afiliado.Baja = activo ? afiliado.Baja : fecha;
            afiliado.Alta = activo ? (fecha ?? afiliado.Alta) : afiliado.Alta;

            foreach (var integrante in afiliado.Integrantes)
            {
                integrante.Baja = activo ? integrante.Baja : fecha;
                integrante.Alta = activo ? (fecha ?? integrante.Alta) : integrante.Alta;
            }
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Afiliado> GetByNumeroAsync(int numeroAfiliado)
    {
        var afiliado = await _context.Afiliados
            .Include(a => a.Integrantes)
                .ThenInclude(p => p.Telefonos)
            .Include(a => a.Integrantes)
                .ThenInclude(p => p.Emails)
            .Include(a => a.Integrantes)
                .ThenInclude(p => p.Documentacion)
            .Include(a => a.Integrantes)
                .ThenInclude(p => p.Direcciones)
            .Include(a => a.Integrantes)
                .ThenInclude(p => p.SituacionesTerapeuticas).ThenInclude(st => st.SituacionTerapeutica)
            .FirstOrDefaultAsync(a => a.NumeroAfiliado == numeroAfiliado);
        return afiliado;
    }
}
