using Application.Contracts.DTOs.Internal;

namespace Application.Contracts.DTOs.Request;

public class AgendaRequest
{
    public int Id { get; set; }
    public List<HorarioAtencionDTO> HorariosAtencion { get; set; } = new();
}
