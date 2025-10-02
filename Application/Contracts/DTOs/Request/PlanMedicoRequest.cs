namespace Application.Contracts.DTOs.Request;

public class PlanMedicoRequest
{
    public string nombre { get; set; }
    public string descripcion { get; set; }
    public int costoMensual { get; set; }
    public string moneda { get; set; } = "ARS"; // ISO 4217 code
    public bool activa { get; set; }
}
