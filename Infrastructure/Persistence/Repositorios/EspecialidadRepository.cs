using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositorios;

public class EspecialidadRepository : IEspecialidadRepository
{
    private readonly ProjectContext context;
    public EspecialidadRepository(ProjectContext projectContext)
    {
        context = projectContext;
    }

    public async Task<Especialidad> AddAsync(Especialidad especialidad)
    {
        try
        {
            await context.Especialidades.AddAsync(especialidad);
            await context.SaveChangesAsync();
            return especialidad;
        }
        catch (Exception ex)
        {
            throw new Exception("Error al agregar la Especialidad", ex);
        }
    }

    public async Task<List<Especialidad>> GetAllAsync()
    {
        try
        {
            List<Especialidad> list = await context.Especialidades.ToListAsync();
            return list;
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener las Especialidades", ex);
        }

    }

    public async Task<bool> ToggleStatusAsync(int id)
    {
        try
        {
            Especialidad? especialidad = await context.Especialidades.FindAsync(id);
            if (especialidad == null)
            {
                throw new Exception("Especialidad no encontrada");
            }
            if (especialidad.Baja == null)
            {
                especialidad.Baja = DateTime.Now.Date;
            }
            else
            {
                especialidad.Baja = null;
                especialidad.Alta = DateTime.Now.Date;
            }
            context.Especialidades.Update(especialidad);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception("Error al actualizar el estado de la Especialidad", ex);
        }
    }

    public async Task<Especialidad> UpdateAsync(Especialidad especialidad)
    {
        try
        {
            Especialidad? existingEspecialidad = await context.Especialidades.FindAsync(especialidad.Id);
            if (existingEspecialidad == null)
            {
                throw new Exception("Especialidad no encontrada");
            }
            existingEspecialidad.Nombre = especialidad.Nombre;
            existingEspecialidad.Descripcion = especialidad.Descripcion;
            existingEspecialidad.Baja = especialidad.Baja;
            context.Especialidades.Update(existingEspecialidad);
            await context.SaveChangesAsync();
            return existingEspecialidad;
        }
        catch (Exception ex)
        {
            throw new Exception("Error al actualizar la Especialidad", ex);
        }
    }
}
