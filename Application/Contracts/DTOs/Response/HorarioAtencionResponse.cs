namespace Application.Contracts.DTOs.Response;

public class HorariosAtencionResponse : List<HorarioAtencionResponse> { }
public class HorarioAtencionResponse
{
    public int Id { get; set; }
    public List<string> DiasDeLaSemana { get; set; } = new List<string>();
    public string HoraInicio { get; set; }
    public string HoraFin { get; set; }
    public int? DuracionMinutos { get; set; }
    public List<int> Especialidades { get; set; } = new List<int>();
}
