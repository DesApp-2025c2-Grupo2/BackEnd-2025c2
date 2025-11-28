using Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public abstract class Prestador
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    // Atributos comunes
    [Required]
    [MaxLength(128)]
    public string NombreCompleto { get; set; } = null!;

    [Required]
    public DateOnly Alta { get; set; }
    public DateOnly? Baja { get; set; }

    // Navegacion bidireccional
    public List<Documentacion> Documentacion { get; set; } = new();
    public List<Email> Emails { get; set; } = new();
    public List<Telefono> Telefonos { get; set; } = new();
    public List<Especialidad> Especialidades { get; set; } = new();
    public List<Direccion> Direcciones { get; set; } = new();
}


public class Profesional : Prestador
{
    [Required]
    [MaxLength(16)]
    public string Matricula { get; set; }

    [ForeignKey(nameof(Centro))]
    public int? CentroId { get; set; }
    public CentroMedico? Centro { get; set; }
    public List<AgendaProfesional> Agendas { get; set; } = new();

}

public class CentroMedico : Prestador
{
    public string? RazonSocial { get; set; }

    // Navegacion bidireccional
    public List<Profesional> Profesionales { get; set; } = new();
    public List<AgendaCentroMedico> Agendas { get; set; } = new();
}