using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Prestador
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    [MaxLength(16)]
    public string Rol { get; set; } // 0: Centro Médico, 1: Profesional Independiente, 2: Profesional de Centro Médico
    [Required]
    [MaxLength(128)]
    public string NombreCompleto { get; set; }
    [MaxLength(128)]
    public string? CentroMedico { get; set; } // Sin especificar, se asume que es un profesional independiente o centro medico
    [Required]
    public DateTime Alta { get; set; }
    public DateTime? Baja { get; set; }

    // Navegacion bidireccional
    public List<Especialidad> Especialidades { get; set; } = new List<Especialidad>();
    public List<Documentacion> Documentaciones { get; set; } = new List<Documentacion>();
    public List<Telefono> Telefonos { get; set; } = new List<Telefono>();
    public List<Email> Emails { get; set; } = new List<Email>();
    public List<Direccion> Direcciones { get; set; } = new List<Direccion>();
    public List<Agenda> Agendas { get; set; } = new List<Agenda>();

}