using Dominio.Entidades;
using Microsoft.EntityFrameworkCore;


namespace Infraestructura.Persistencia.Configuraciones;

public class ProjectContext : DbContext
{
    public ProjectContext(DbContextOptions<ProjectContext> options) : base(options){ }
    public ProjectContext(){ }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Configuración de la cadena de conexión a la base de datos
        var conn = Environment.GetEnvironmentVariable("STRING_CONN_DAPPS") ?? throw new Exception("STRING_CONN_DAPPS no definida");
        optionsBuilder.UseMySql(conn, ServerVersion.AutoDetect(conn));
        
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuración de relaciones y pks
        modelBuilder.Entity<Estado>().HasKey(e => e.Id); // Definir la clave primaria para la entidad Estado
        modelBuilder.Entity<Estado_His>().HasKey(e => e.Id); // Definir la clave primaria para la entidad Estado_His
        modelBuilder.Entity<Estado_His>()
            .HasOne(e => e.Estado) // Configurar la relación uno a muchos
            .WithMany() // Configurar la relación inversa
            .HasForeignKey(e => e.EstadoId); // Definir la clave foránea
    }


    public DbSet<Estado> Estados { get; set; } // DbSet para la entidad Estado
    public DbSet<Estado_His> Estados_His { get; set; } // DbSet para la entidad Estados_His
}
