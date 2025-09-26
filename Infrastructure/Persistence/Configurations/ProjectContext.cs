using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;


namespace Infrastructure.Persistence.Configurations;

public class ProjectContext : DbContext
{
    private bool OracleProvider = true;
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
        modelBuilder.Entity<PlanMedico>(entity => entity.ToTable("PLANES_MEDICOS"));
        modelBuilder.Entity<Afiliado>(entity => entity.ToTable("AFILIADOS"));
        modelBuilder.Entity<Persona>(entity => entity.ToTable("PERSONAS"));
        modelBuilder.Entity<Documentacion>(entity => entity.ToTable("DOCUMENTACIONES"));
        modelBuilder.Entity<Direccion>(entity => entity.ToTable("DIRECCIONES"));
        modelBuilder.Entity<Telefono>(entity => entity.ToTable("TELEFONOS"));
        modelBuilder.Entity<Email>(entity => entity.ToTable("EMAILS"));
        modelBuilder.Entity<Especialidad>(entity => entity.ToTable("ESPECIALIDADES"));
        modelBuilder.Entity<Prestador>(entity => entity.ToTable("PRESTADORES"));
        modelBuilder.Entity<HorarioAtencion>(entity => entity.ToTable("HORARIOS_ATENCION"));
        modelBuilder.Entity<Agenda>(entity => entity.ToTable("AGENDAS"));
        modelBuilder.Entity<SituacionTerapeutica>(entity => entity.ToTable("SITUACIONES_TERAPEUTICAS"));
        // Forzar que todas las tablas y columnas sean en mayúsculas
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            // Tabla
            entity.SetTableName(entity.GetTableName().ToUpper());

            // Columnas
            foreach (var property in entity.GetProperties())
            {
                property.SetColumnName(property.GetColumnName(StoreObjectIdentifier.Table(entity.GetTableName(), null))!.ToUpper());
            }

            // Keys primarias
            foreach (var key in entity.GetKeys())
            {
                key.SetName(key.GetName()!.ToUpper());
            }

            // Foreign keys
            foreach (var fk in entity.GetForeignKeys())
            {
                fk.SetConstraintName(fk.GetConstraintName()!.ToUpper());
            }

            // Índices
            foreach (var index in entity.GetIndexes())
            {
                index.SetDatabaseName(index.GetDatabaseName()!.ToUpper());
            }
        }
        // Configuración de la relación muchos a muchos entre Persona y SituacionTerapeutica
        modelBuilder.Entity<Persona>()
        .HasMany(p => p.SituacionesTerapeuticas)
        .WithMany(st => st.Personas)
        .UsingEntity<Dictionary<string, object>>(
            "HISTORIAL_TERAPEUTICO", // Nombre de la tabla intermedia
            j => j.HasOne<SituacionTerapeutica>().WithMany().HasForeignKey("SITUACIONTERAPEUTICAID"),
            j => j.HasOne<Persona>().WithMany().HasForeignKey("PERSONAID")
        );
        // Configuración de la relación muchos a muchos entre Prestador y Especialidad
        modelBuilder.Entity<Prestador>()
        .HasMany(p => p.Especialidades)
        .WithMany(e => e.Prestadores)
        .UsingEntity<Dictionary<string, object>>(
            "ESPECIALIZACIONES", // Nombre de la tabla intermedia
            j => j.HasOne<Especialidad>().WithMany().HasForeignKey("ESPECIALIDADID"),
            j => j.HasOne<Prestador>().WithMany().HasForeignKey("PRESTADORID")
        );


        base.OnModelCreating(modelBuilder);
    }

    // Definición de DbSet para cada entidad
    public DbSet<Persona> Personas { get; set; }
    public DbSet<Agenda> Agendas { get; set; }
    public DbSet<Direccion> Direcciones { get; set; }
    public DbSet<Documentacion> Documentaciones { get; set; }
    public DbSet<Email> Emails { get; set; }
    public DbSet<Especialidad> Especialidades { get; set; }
    public DbSet<Afiliado> Afiliados { get; set; }
    public DbSet<HorarioAtencion> HorariosAtencion { get; set; }
    public DbSet<PlanMedico> PlanesMedicos { get; set; }
    public DbSet<Prestador> Prestadores { get; set; }
    public DbSet<SituacionTerapeutica> SituacionesTerapeuticas { get; set; }
    public DbSet<Telefono> Telefonos { get; set; }

}
