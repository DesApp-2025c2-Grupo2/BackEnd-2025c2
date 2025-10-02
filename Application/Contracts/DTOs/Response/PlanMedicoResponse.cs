namespace Application.Contracts.DTOs.Response;

public class PlanesMedicosResponse : List<PlanMedicoResponse> { }
public class PlanMedicoResponse
{
    public int id { get; set; }
    public string nombre { get; set; } = string.Empty;
    public string descripcion { get; set; } = string.Empty;
    public int costoMensual { get; set; } = 0;
    public string moneda { get; set; } = "ARS"; // ISO 4217 code
    public bool activa { get; set; }
}
