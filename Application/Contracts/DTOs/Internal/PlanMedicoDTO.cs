namespace Application.Contracts.DTOs.Internal;

public class PlanMedicoDTO
{
    public string Nombre { get; set; } = string.Empty;
    public int CostoMensual { get; set; } = 0;
    public string Moneda { get; set; } = "ARS"; // ISO 4217 code
}
