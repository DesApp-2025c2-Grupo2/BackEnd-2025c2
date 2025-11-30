using Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Seeds;

public static class SeedManager
{
    public static async Task InitializeAsync(ProjectContext context)
    {
        await SeedSituacionTerapeutica(context);
        await SeedEspecialidades(context);
        await SeedPlanesMedicos(context);

        await SeedAfiliados(context);
        await SeedPersonas(context);
    }
    
    
    private static async Task SeedSituacionTerapeutica(ProjectContext context)
    {
        int cantidad = await context.SituacionesTerapeuticas.CountAsync();
        if (cantidad == 0)
        {
            var situaciones = TableSeeds.SituacionesTerapeuticas();
            await context.SituacionesTerapeuticas.AddRangeAsync(situaciones);
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedEspecialidades(ProjectContext context)
    {
        int cantidad = await context.Especialidades.CountAsync();
        if (cantidad == 0)
        {
            var especialidades = TableSeeds.Especialidades();
            await context.Especialidades.AddRangeAsync(especialidades);
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedPlanesMedicos(ProjectContext context)
    {
        int cantidad = await context.PlanesMedicos.CountAsync();
        if (cantidad == 0)
        {
            var planes = TableSeeds.PlanesMedicos();
            await context.PlanesMedicos.AddRangeAsync(planes);
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedAfiliados(ProjectContext context)
    {
        int cantidad = await context.Afiliados.CountAsync();
        if (cantidad == 0)
        {
            var afiliados = TableSeeds.Afiliados();
            await context.Afiliados.AddRangeAsync(afiliados);
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedPersonas(ProjectContext context)
    {
        int cantidad = await context.Personas.CountAsync();
        if (cantidad == 0)
        {
            var personas = TableSeeds.Personas();
            await context.Personas.AddRangeAsync(personas);
            await context.SaveChangesAsync();
        }
    }
}