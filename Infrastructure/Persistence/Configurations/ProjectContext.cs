//using Dominio.Entidades;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Persistence.Configurations;

public class ProjectContext : DbContext
{
    public ProjectContext(DbContextOptions<ProjectContext> options) : base(options){ }
    public ProjectContext(){ }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Configuración de la cadena de conexión a la base de datos
        var conn = Environment.GetEnvironmentVariable("STRING_CONN_DAPPS_ORACLE") ?? throw new Exception("STRING_CONN_DAPPS_ORACLE no definida");
        //optionsBuilder.UseMySql(conn, ServerVersion.AutoDetect(conn));
        optionsBuilder.UseOracle(conn);

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuración de las entidades y sus relaciones
        modelBuilder.Entity<PlanMedico>(entity => entity.ToTable("PLANES_MEDICOS"));
        modelBuilder.Entity<GruporFamiliar>(entity => entity.ToTable("GRUPOS_FAMILIARES"));
        modelBuilder.Entity<Parentesco>(entity => entity.ToTable("PARENTESCOS"));
        base.OnModelCreating(modelBuilder);
    }

    // Definición de DbSet para cada entidad
    public DbSet<PlanMedico> PlanesMedicos { get; set; }
    public DbSet<GruporFamiliar> GruposFamiliares { get; set; }
    public DbSet<Parentesco> Parentescos { get; set; }

}
