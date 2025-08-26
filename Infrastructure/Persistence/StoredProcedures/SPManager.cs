using Application.Utilities;
using Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.StoredProcedures;

public static class SPManager
{
    public static async Task InitializeAsync(ProjectContext context, IProjectLogger logger)
    {
        var connection = context.Database.GetDbConnection();
        var spFolderPath = Path.Combine(AppContext.BaseDirectory, "Persistence", "StoredProcedures", "Scripts");

        if (!Directory.Exists(spFolderPath))
            throw new DirectoryNotFoundException($"No se encontró la carpeta de SP: {spFolderPath}");

        var sqlFiles = Directory.GetFiles(spFolderPath, "*.sql", SearchOption.AllDirectories);

        await connection.OpenAsync();
        try
        {
            foreach (var sqlFile in sqlFiles)
            {
                var sqlContent = await File.ReadAllTextAsync(sqlFile);
                var commands = sqlContent.Split(new[] { "-- separador de comandos --" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var commandText in commands.Where(c => !string.IsNullOrWhiteSpace(c)))
                {
                    using var command = connection.CreateCommand();
                    command.CommandText = commandText.Trim();
                    command.CommandType = System.Data.CommandType.Text;

                    try
                    {
                        await command.ExecuteNonQueryAsync();

                        // Detectar si es DROP o CREATE por contenido
                        var action = command.CommandText.StartsWith("DROP", StringComparison.OrdinalIgnoreCase)
                            ? "Drop"
                            : command.CommandText.StartsWith("CREATE", StringComparison.OrdinalIgnoreCase)
                                ? "Create"
                                : "Comando";

                        logger.LogSuccess($"Se ejecutó {action} de {Path.GetFileNameWithoutExtension(sqlFile)}");
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(
                            $"Error al ejecutar {Path.GetFileNameWithoutExtension(sqlFile)}. " +
                            $"Comando: {command.CommandText.Substring(0, Math.Min(80, command.CommandText.Length))}...",
                            ex);
                    }
                }
            }
        }
        finally
        {
            await connection.CloseAsync();
            logger.LogInformation("Scripts procesados.");
        }
    }

}