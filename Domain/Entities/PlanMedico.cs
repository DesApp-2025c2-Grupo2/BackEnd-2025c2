using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class PlanMedico
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    [MaxLength(32)]
    public string Nombre { get; set; } = string.Empty;
    [Required]
    [MaxLength(512)]
    public string Descripcion { get; set; } = string.Empty;
    [Required]
    public int CostoMensual { get; set; } = 0;
    [Required]
    [MaxLength(3)]
    public string Moneda { get; set; } = "ARS"; // ISO 4217 code
    [Required]
    public DateTime Alta { get; set; }
    public DateTime? Baja { get; set; }

    // Navegacion bidireccional
    public List<Afiliado> Afiliados { get; set; } = new List<Afiliado>();
}