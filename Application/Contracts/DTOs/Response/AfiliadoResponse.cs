using Application.Contracts.DTOs.Internal;

namespace Application.Contracts.DTOs.Response;

public class AfiliadosResponse : List<AfiliadoResponse>;
public class AfiliadoResponse
{
    public int numeroAfiliado { get; set; }
    public PersonaResponse titular { get; set; }
    public List<PersonaResponse> integrantes { get; set; }
    public PlanMedicoDTO planMedico { get; set; }
}
