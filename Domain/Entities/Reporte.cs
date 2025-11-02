using Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Reporte
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    [MaxLength(6)]
    public string CodigoIdentificatorio { get; set; }
    [Required]
    public TipoReporte Tipo { get; set; }
    public DateTime? FechaDesde { get; set; }
    public DateTime? FechaHasta { get; set; }
    public int? AfiliadoId { get; set; }
    [Required]
    public DateTime FechaGeneracion { get; set; }
}
