namespace Application.Contracts.DTOs.Request;

public class PrestadorRequest
{
    public string NombreCompleto { get; set; }
    public int Rol { get; set; }
    public string? CentroMedico { get; set; }
    public List<int> EspecialidadesIds { get; set; } = new List<int>();
    public string Documentacion { get; set; }
    public List<string> Telefonos { get; set; } = new List<string>();
    public List<string> Emails { get; set; } = new List<string>();
    public List<string> Direcciones { get; set; } = new List<string>();
}
