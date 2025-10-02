using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositorios;

public class SituacionTerapeuticaRepository : ISituacionTerapeuticaRepository
{
    private readonly ProjectContext dbContext;

    public SituacionTerapeuticaRepository(ProjectContext projectContext)
    {
        dbContext = projectContext;
    }


    public async Task<SituacionTerapeutica> AddAsync(SituacionTerapeutica entity)
    {
        try
        {
            await dbContext.SituacionesTerapeuticas.AddAsync(entity);
            await dbContext.SaveChangesAsync();
            return entity;
        }
        catch (Exception ex)
        {
            throw new Exception("Error al agregar la Situacion Terapeutica", ex);
        }
    }

    public async Task<List<SituacionTerapeutica>> GetAllAsync()
    {
        try
        {
            List<SituacionTerapeutica> list = await dbContext.SituacionesTerapeuticas.ToListAsync();
            return list;
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener las Situaciones Terapeuticas", ex);
        }
    }

    public async Task<bool> ToggleStatusAsync(int id)
    {
        try
        {
            SituacionTerapeutica? situacion = await dbContext.SituacionesTerapeuticas.FindAsync(id);
            if (situacion == null)
            {
                throw new Exception("Situacion Terapeutica no encontrada");
            }
            if (situacion.Baja == null)
            {
                situacion.Baja = DateTime.Now.Date;
            }
            else
            {
                situacion.Baja = null;
                situacion.Alta = DateTime.Now.Date;
            }
            dbContext.SituacionesTerapeuticas.Update(situacion);
            await dbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception("Error al actualizar el estado de la Situacion Terapeutica", ex);
        }
    }

    public async Task<SituacionTerapeutica> UpdateAsync(SituacionTerapeutica entity)
    {
        try
        {
            SituacionTerapeutica? situacion = await dbContext.SituacionesTerapeuticas.FindAsync(entity.Id);
            if (situacion == null)
            {
                throw new Exception("Situacion Terapeutica no encontrada");
            }
            situacion.Nombre = entity.Nombre;
            situacion.Descripcion = entity.Descripcion;
            dbContext.SituacionesTerapeuticas.Update(situacion);
            await dbContext.SaveChangesAsync();
            return situacion;
        }
        catch (Exception ex)
        {
            throw new Exception("Error al actualizar la Situacion Terapeutica", ex);
        }
    }
}
