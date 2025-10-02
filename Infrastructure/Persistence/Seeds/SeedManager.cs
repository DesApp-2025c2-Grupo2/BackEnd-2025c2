using Application.Utilities;
using Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Seeds;

public static class SeedManager
{
    public static async Task InitializeAsync(ProjectContext context, IProjectLogger logger)
    {
        await SeedSituacionTerapeutica(context);
        await SeedEspecialidades(context);
        await SeedPlanesMedicos(context);
        logger.LogInformation("Seeds inicializados correctamente.");
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
}