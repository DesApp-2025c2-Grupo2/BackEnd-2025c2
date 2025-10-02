using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Especialidad
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    [MaxLength(64)]
    public string Nombre { get; set; }
    [Required]
    [MaxLength(512)]
    public string Descripcion { get; set; }
    [Required]
    public DateTime Alta { get; set; }
    public DateTime? Baja { get; set; }

    // Navegacion bidireccional
    public List<Prestador> Prestadores { get; set; } = new List<Prestador>();
}
