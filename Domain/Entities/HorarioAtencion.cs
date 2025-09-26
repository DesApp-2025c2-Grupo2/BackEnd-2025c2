using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class HorarioAtencion
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    [ForeignKey(nameof(Agenda))]
    public int AgendaId { get; set; }
    public Agenda Agenda { get; set; }
    [Required]
    [MaxLength(16)]
    public string DiaAtencion { get; set; }
    [Required]
    public DateTime HoraInicio { get; set; }
    [Required]
    public DateTime HoraFin { get; set; }
    [Required]
    public DateTime Alta { get; set; }
    public DateTime? Baja { get; set; }
}
