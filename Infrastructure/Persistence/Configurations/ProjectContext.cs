//using Dominio.Entidades;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Persistence.Configurations;

public class ProjectContext : DbContext
{
    private bool OracleProvider = false;
    public ProjectContext(DbContextOptions<ProjectContext> options) : base(options){ }
    public ProjectContext(){ }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Configuración de la cadena de conexión a la base de datos
        var conn = "";
        if (OracleProvider)
        {
            conn = Environment.GetEnvironmentVariable("STRING_CONN_DAPPS_ORACLE") ?? throw new InvalidOperationException("La cadena de conexión no está definida en la variable de entorno 'STRING_CONN_DAPPS_ORACLE'.");
            optionsBuilder.UseOracle(conn);
        }
        else
        {
             conn = Environment.GetEnvironmentVariable("STRING_CONN_DAPPS_MYSQL") ?? throw new InvalidOperationException("La cadena de conexión no está definida en la variable de entorno 'STRING_CONN_DAPPS_MYSQL'.");
            optionsBuilder.UseMySql(conn, ServerVersion.AutoDetect(conn));
        }
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuración de las entidades y sus relaciones
        base.OnModelCreating(modelBuilder);
    }

    // Definición de DbSet para cada entidad

}
