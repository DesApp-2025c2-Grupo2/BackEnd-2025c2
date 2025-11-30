using Application.Contracts.DTOs.Internal;
using Domain.Enums;

namespace Application.Contracts.DTOs.Request;


public class PrestadorRequest
{
    public int? Id { get; set; }
    public required string NombreCompleto { get; set; }
    public required RolMedico Rol { get; set; } // 0: Centro Médico, 1: Profesional Independiente
    public string? Matricula { get; set; }
    public string? RazonSocial { get; set; }
    public int? CentroId { get; set; }
    public DateOnly? Alta { get; set; } // YYYY-MM-DD
    public List<int> Especialidades { get; set; } = new();
    public required DocumentacionDTO Documentacion { get; set; }
    public List<TelefonoDTO> Telefonos { get; set; } = new();
    public List<EmailDTO> Emails { get; set; } = new();
    public List<DireccionDTO> Direcciones { get; set; } = new();
}
