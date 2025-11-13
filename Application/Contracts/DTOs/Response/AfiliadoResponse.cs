using Application.Contracts.DTOs.Internal;

namespace Application.Contracts.DTOs.Response;

public class AfiliadosResponse : List<AfiliadoResponse>;
public class AfiliadoResponse
{
    public int Id { get; set; }
    public int NumeroAfiliado { get; set; }
    public int TitularID { get; set; }
    public int PlanMedicoId { get; set; }
    public string? PlanMedicoNombre { get; set; }
    public DateTime Alta { get; set; }
    public DateTime? Baja { get; set; }

    public PersonasResponse? Integrantes { get; set; }
}
