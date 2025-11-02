using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class HistorialTerapeutico
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    [ForeignKey(nameof(Persona))]
    public int PersonaId { get; set; }
    public Persona Persona { get; set; }
    [Required]
    [ForeignKey(nameof(SituacionTerapeutica))]
    public int SituacionTerapeuticaId { get; set; }
    public SituacionTerapeutica SituacionTerapeutica { get; set; }
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
}
