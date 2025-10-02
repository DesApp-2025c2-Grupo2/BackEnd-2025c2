using Application.Contracts.DTOs.Internal;

namespace Application.Contracts.DTOs.Response;

public class AfiliadosResponse : List<AfiliadoResponse>;
public class AfiliadoResponse
{
    public int NumeroAfiliado { get; set; }
    public PersonaDTO Titular { get; set; }
    public List<PersonaDTO> Integrantes { get; set; }
    public PlanMedicoDTO PlanMedico { get; set; }
}
