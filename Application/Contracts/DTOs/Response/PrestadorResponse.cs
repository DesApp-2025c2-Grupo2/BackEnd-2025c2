using Application.Contracts.DTOs.Internal;
using Domain.Enums;

namespace Application.Contracts.DTOs.Response;

public class PrestadoresResponse : List<PrestadorResponse> { }
public class PrestadorResponse
{
    public int Id { get; set; }
    public string NombreCompleto { get; set; }
    public DocumentacionDTO Documentacion { get; set; }
    public List<EmailDTO> Emails { get; set; }
    public List<TelefonoDTO> Telefonos { get; set; }
    public List<EspecialidadDTO> Especialidades { get; set; }
    public List<DireccionDTO> Direcciones { get; set; }
    public DateOnly Alta { get; set; } // YYYY-MM-DD
    public DateOnly? Baja { get; set; } // YYYY-MM-DD
    public string? Matricula { get; set; }
    public string? RazonSocial { get; set; }
    public RolMedico Rol { get; set; } // 0: Centro Médico, 1: Profesional Independiente
    public CentroMedicoDTO? Centro { get; set; }
    public List<ProfesionalDTO>? Profesionales { get; set; }
    public AgendasResponse Agendas { get; set; }

}
