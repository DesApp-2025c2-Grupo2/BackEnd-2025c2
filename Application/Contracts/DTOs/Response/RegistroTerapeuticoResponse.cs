namespace Application.Contracts.DTOs.Response;

public class HistorialTerapeuticoResponse : List<RegistroTerapeuticoResponse> { }
public class RegistroTerapeuticoResponse
{
    public int id { get; set; }
    public string nombre { get; set; }
    public DateTime? fechaFin { get; set; }

}
