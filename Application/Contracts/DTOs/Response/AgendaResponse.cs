using Application.Contracts.DTOs.Internal;

namespace Application.Contracts.DTOs.Response;

public class AgendasResponse : List<AgendaResponse> { }
public class AgendaResponse
{
    public int Id { get; set; }

    public string? DireccionAtencion { get; set; }
    public List<HorarioAtencionDTO> Horarios { get; set; }

}
