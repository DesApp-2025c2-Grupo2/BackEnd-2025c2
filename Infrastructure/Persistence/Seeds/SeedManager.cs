using Application.Utilities;
using Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Seeds;

public static class SeedManager
{
    public static async Task InitializeAsync(ProjectContext context, IProjectLogger logger)
    {
        //await SeedCategorias(context);
        logger.LogInformation("Seeds inicializados correctamente.");
    }

    //private static async Task SeedCategorias(ProjectContext context)
    //{
    //    if (!await context.Categorias.AnyAsync()) // Validar si ya existen
    //    {
    //        var categorias = TableSeeds.Categorias();
    //        await context.Categorias.AddRangeAsync(categorias);
    //        await context.SaveChangesAsync();
    //    }
    //}

}