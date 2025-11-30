namespace Application.Contracts.DTOs.Response;


public class ReportesResponse : List<ReporteResponse> { }
public class ReporteResponse
{
    public string HexaID { get; set; }
    public string TipoReporte { get; set; }
    public string Parametros { get; set; }
    public DateTime FechaGeneracion { get; set; }
    public string FileURL { get; set; }
}
