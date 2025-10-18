using Application.Contracts.DTOs.Internal;

namespace Application.Contracts.DTOs.Response;

public class PrestadoresResponse : List<PrestadorResponse> { }
public class PrestadorResponse
{
    public int Id { get; set; }
    public string NombreCompleto { get; set; }
    public int Rol { get; set; }
    public string? CentroMedico { get; set; }
    public List<int> Especialidades { get; set; } = new List<int>();
    public DocumentacionDTO Documentacion { get; set; }
    public List<TelefonoDTO> Telefonos { get; set; } = new List<TelefonoDTO>();
    public List<EmailDTO> Emails { get; set; } = new List<EmailDTO>();
    public List<DireccionDTO> Direcciones { get; set; } = new List<DireccionDTO>();
}
