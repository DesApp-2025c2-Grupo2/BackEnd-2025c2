using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositorios;

public class PlanMedicoRepository : IPlanMedicoRepository
{
    private readonly ProjectContext context;

    public PlanMedicoRepository(ProjectContext projectContext)
    {
        context = projectContext;
    }

    public async Task<PlanMedico> AddAsync(PlanMedico planMedico)
    {
        try
        {
            await context.PlanesMedicos.AddAsync(planMedico);
            await context.SaveChangesAsync();
            return planMedico;
        }
        catch (Exception ex)
        {
            throw new Exception("Error al agregar el Plan Medico", ex);
        }
    }

    public async Task<List<PlanMedico>> GetAllAsync()
    {
        try
        {
            List<PlanMedico> list = await context.PlanesMedicos.ToListAsync();
            return list;
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener los Planes Medicos", ex);
        }
    }

    public async Task<bool> ToggleStatusAsync(int id)
    {
        try
        {
            PlanMedico? plan = await context.PlanesMedicos.FindAsync(id);
            if (plan == null)
            {
                throw new Exception("Plan Medico no encontrado");
            }
            if (plan.Baja == null)
            {
                plan.Baja = DateTime.Now.Date;
            }
            else
            {
                plan.Baja = null;
                plan.Alta = DateTime.Now.Date;
            }
            context.PlanesMedicos.Update(plan);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception("Error al actualizar el estado del Plan Medico", ex);
        }
    }

    public async Task<PlanMedico> UpdateAsync(PlanMedico planMedico)
    {
        try
        {
            PlanMedico? plan = await context.PlanesMedicos.FindAsync(planMedico.Id);
            if (plan == null)
            {
                throw new Exception("Plan Medico no encontrado");
            }
            plan.Nombre = planMedico.Nombre;
            plan.Descripcion = planMedico.Descripcion;
            plan.CostoMensual = planMedico.CostoMensual;
            plan.Moneda = planMedico.Moneda;
            context.PlanesMedicos.Update(plan);
            await context.SaveChangesAsync();
            return plan;
        }
        catch (Exception ex)
        {
            throw new Exception("Error al actualizar el Plan Medico", ex);
        }
    }
}
