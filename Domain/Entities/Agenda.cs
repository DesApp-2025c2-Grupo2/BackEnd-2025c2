using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Agenda
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    [ForeignKey(nameof(Profesional))]
    public int ProfesionalId { get; set; }
    public Prestador Profesional { get; set; }
    [Required]
    public int EspecialidadId { get; set; }
    [Required]
    [MaxLength(128)]
    public string Direccion { get; set; }
    [Required]
    public int DuracionConsulta { get; set; }
    [Required]
    public DateTime Alta { get; set; }
    public DateTime? Baja { get; set; }
}
