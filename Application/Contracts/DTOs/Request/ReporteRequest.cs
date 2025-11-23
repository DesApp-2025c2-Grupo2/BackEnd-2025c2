namespace Application.Contracts.DTOs.Request;

public class ReporteRequest
{
    public int TipoReporte { get; set; }
    public DateTime? FechaDesde { get; set; }
    public DateTime? FechaHasta { get; set; }
    public int? AfiliadoId { get; set; }
}
