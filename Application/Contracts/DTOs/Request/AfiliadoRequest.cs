using Application.Contracts.DTOs.Internal;

namespace Application.Contracts.DTOs.Request;

public class AfiliadoRequest
{
    public int PlanMedicoId { get; set; }
    public PersonaDTO Titular { get; set; }
    public List<PersonaDTO> Integrantes { get; set; }
    public DateTime FechaAlta { get; set; }
    public DateTime? FechaBaja { get; set; }


}
