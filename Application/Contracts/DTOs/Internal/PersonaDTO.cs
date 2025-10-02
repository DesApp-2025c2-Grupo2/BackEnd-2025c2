using Application.Contracts.DTOs.Request;

namespace Application.Contracts.DTOs.Internal;

public class PersonaDTO
{
    public int id { get; set; }
    public int NumeroIntegrante { get; set; }
    public string nombre { get; set; }
    public string apellido { get; set; }
    public DateTime fechaNacimiento { get; set; }
    public int parentesco { get; set; }
    public DateTime alta { get; set; }
    public DateTime? baja { get; set; }
    public List<string> telefonos { get; set; }
    public List<string> emails { get; set; }
    public DocumentacionDTO documentacion { get; set; }
    public List<string> direcciones { get; set; }
    public List<SituacionTerapeuticaRequest>? situacionTerapeuticaRequests { get; set; }
}
