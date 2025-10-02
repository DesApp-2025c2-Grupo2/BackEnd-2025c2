using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence.Configurations;

namespace Infrastructure.Persistence.Repositorios;

public class SituacionTerapeuticaRepository : ISituacionTerapeuticaRepository
{
    private readonly ProjectContext dbContext;

    public SituacionTerapeuticaRepository(ProjectContext projectContext)
    {
        dbContext = projectContext;
    }


    public Task<SituacionTerapeutica> AddAsync(SituacionTerapeutica entity)
    {
        try
        {
            dbContext.SituacionesTerapeuticas.Add(entity);
            dbContext.SaveChanges();
            return Task.FromResult(entity);
        }
        catch (Exception ex)
        {
            // Aquí podríamos loguear el error si tuviéramos un logger
            throw new Exception("Error al agregar la Situacion Terapeutica", ex);
        }
    }

    public Task<List<SituacionTerapeutica>> GetAllAsync()
    {
        try
        {
            List<SituacionTerapeutica> list = dbContext.SituacionesTerapeuticas.ToList();
            return Task.FromResult(list);
        }
        catch (Exception ex)
        {
            // Aquí podríamos loguear el error si tuviéramos un logger
            throw new Exception("Error al obtener las Situaciones Terapeuticas", ex);
        }
    }

    public Task<SituacionTerapeutica> ToggleStatusAsync(SituacionTerapeutica entity)
    {
        SituacionTerapeutica? situacion = dbContext.SituacionesTerapeuticas.Find(entity.Id);
        if (situacion == null)
        {
            throw new Exception("Situacion Terapeutica no encontrada");
        }
        try
        {
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
            dbContext.SaveChangesAsync();
            return Task.FromResult(situacion);
        }
        catch (Exception ex)
        {
            // Aquí podríamos loguear el error si tuviéramos un logger
            throw new Exception("Error al actualizar el estado de la Situacion Terapeutica", ex);
        }
    }

    public Task<SituacionTerapeutica> UpdateAsync(SituacionTerapeutica entity)
    {
        SituacionTerapeutica? situacion = dbContext.SituacionesTerapeuticas.Find(entity.Id);
        if (situacion == null)
        {
            throw new Exception("Situacion Terapeutica no encontrada");
        }
        try
        {
            situacion.Nombre = entity.Nombre;
            situacion.Descripcion = entity.Descripcion;
            dbContext.SituacionesTerapeuticas.Update(situacion);
            dbContext.SaveChangesAsync();
            return Task.FromResult(situacion);
        }
        catch (Exception ex)
        {
            // Aquí podríamos loguear el error si tuviéramos un logger
            throw new Exception("Error al actualizar la Situacion Terapeutica", ex);
        }
    }
}
