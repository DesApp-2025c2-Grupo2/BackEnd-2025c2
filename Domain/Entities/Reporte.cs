using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Reporte
{
    [Key]
    public string CodigoIdentificatorio { get; set; }
    [Required]
    public TipoReporte Tipo { get; set; }
    public DateTime? FechaDesde { get; set; }
    public DateTime? FechaHasta { get; set; }
    public int? AfiliadoId { get; set; }
    [Required]
    public DateTime FechaGeneracion { get; set; }
}
