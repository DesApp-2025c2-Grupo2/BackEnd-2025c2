using Application.Contracts.DTOs.Internal;

namespace Application.Contracts.DTOs.Response;

public class PersonaResponse
{
    public int id { get; set; }
    public int numeroIntegrante { get; set; }
    public string nombre { get; set; }
    public string apellido { get; set; }
    public DateTime fechaNacimiento { get; set; }
    public int parentesco { get; set; }
    public DateTime alta { get; set; }
    public DateTime? baja { get; set; }
    public List<TelefonoDTO> telefonos { get; set; }
    public List<EmailDTO> emails { get; set; }
    public DocumentacionDTO documentacion { get; set; }
    public List<DireccionDTO> direcciones { get; set; }
    public SituacionesTerapeuticasResponse situacionesTerapeuticasResponse { get; set; }
}
