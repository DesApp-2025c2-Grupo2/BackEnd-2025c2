using Aplicacion.Utilidades;
using Infraestructura.Persistencia.Configuraciones;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Persistencia.StoredProcedures;

public static class SPManager
{
    public static async Task InitializeAsync(ProjectContext context, IProjectLogger logger)
    {
        var connection = context.Database.GetDbConnection();
        var spFolderPath = Path.Combine(AppContext.BaseDirectory, "Persistencia", "StoredProcedures", "Scripts");


        if (!Directory.Exists(spFolderPath))
            throw new DirectoryNotFoundException($"No se encontró la carpeta de SP: {spFolderPath}");

        var sqlFiles = Directory.GetFiles(spFolderPath, "*.sql", SearchOption.AllDirectories);

        await connection.OpenAsync();
        foreach (var sqlFile in sqlFiles)
        {
            var sqlContent = await File.ReadAllTextAsync(sqlFile);
            var commands = sqlContent.Split(new[] { "-- separador de comandos --" }, StringSplitOptions.RemoveEmptyEntries);
            int i = 0;
            foreach (var commandText in commands)
            {
                if (string.IsNullOrWhiteSpace(commandText)) continue;
                using var command = connection.CreateCommand();
                command.CommandText = commandText.Trim();
                command.CommandType = System.Data.CommandType.Text;
                try
                {
                    await command.ExecuteNonQueryAsync();
                    logger.LogSuccess($"Se ejecuto {(i == 0 ? "Drop" : "Create")} de {Path.GetFileNameWithoutExtension(sqlFile)}");
                }
                catch (Exception ex)
                {
                    logger.LogError($"Error al ejecutar {(i == 0 ? "Drop" : "Create")} de {Path.GetFileNameWithoutExtension(sqlFile)}", ex);
                }
                i++;
            }

        }
        await connection.CloseAsync();
        logger.LogInformation("Scripts procesados.");
    }
}