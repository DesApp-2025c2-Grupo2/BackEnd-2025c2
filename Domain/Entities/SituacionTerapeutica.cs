using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class SituacionTerapeutica
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    [MaxLength(128)]
    public string Nombre { get; set; } = string.Empty;
    [Required]
    [MaxLength(512)]
    public string Descripcion { get; set; } = string.Empty;
    [Required]
    public DateTime Alta { get; set; }
    public DateTime? Baja { get; set; }

    // Navegacion bidireccional
    public List<Persona> Personas { get; set; } = new List<Persona>();
}
