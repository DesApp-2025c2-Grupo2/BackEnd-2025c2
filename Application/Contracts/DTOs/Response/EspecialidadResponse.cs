namespace Application.Contracts.DTOs.Response;

public class EspecialidadesResponse : List<EspecialidadResponse> { }
public class EspecialidadResponse
{
    public int id { get; set; }
    public string nombre { get; set; }
    public string descripcion { get; set; }
    public bool activa { get; set; }
}
