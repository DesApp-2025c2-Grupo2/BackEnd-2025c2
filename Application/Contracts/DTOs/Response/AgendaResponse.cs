namespace Application.Contracts.DTOs.Response;

public class AgendasResponse : List<AgendaResponse> { }
public class AgendaResponse
{
    public int Id { get; set; }
    public int EspecialidadId { get; set; }
    public string Direccion { get; set; }
    public int DuracionConsulta { get; set; }
    public List<HorarioAtencionResponse> HorariosAtencion { get; set; } = new List<HorarioAtencionResponse>();
}
