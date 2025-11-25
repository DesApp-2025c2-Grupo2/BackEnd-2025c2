using Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Prestador
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public RolMedico Rol { get; set; } // 0: Centro Médico, 1: Profesional Independiente

    [Required]
    [MaxLength(128)]
    public string NombreCompleto { get; set; }

    [MaxLength(128)]
    public string? CentroMedico { get; set; } // Sin especificar, se asume que es un profesional independiente o centro medico

    // Relación centro médico -> profesionales
    // Para un profesional independiente, CentroId apunta al prestador que actúa como centro médico.
    public int? CentroId { get; set; }
    public Prestador? Centro { get; set; }
    public List<Prestador> Profesionales { get; set; } = new List<Prestador>();

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