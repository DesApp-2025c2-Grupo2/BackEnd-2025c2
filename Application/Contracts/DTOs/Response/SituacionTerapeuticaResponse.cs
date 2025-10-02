namespace Application.Contracts.DTOs.Response;
public class SituacionesTerapeuticasResponse : List<SituacionTerapeuticaResponse> { }
public class SituacionTerapeuticaResponse
{
    public int id { get; set; }
    public string nombre { get; set; }
    public string descripcion { get; set; }
    public bool activa { get; set; }
}
