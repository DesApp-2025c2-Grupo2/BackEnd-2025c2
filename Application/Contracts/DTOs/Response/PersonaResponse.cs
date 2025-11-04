using Application.Contracts.DTOs.Internal;
using Domain.Entities;
using Domain.Enums;

namespace Application.Contracts.DTOs.Response;

public class PersonaResponse
{
    public int Id { get; set; }
    public int NumeroIntegrante { get; set; }
    public required string Nombre { get; set; }
    public required string Apellido { get; set; }
    public Parentesco Parentesco { get; set; }
    public DateTime Alta { get; set; }
    public DateTime? Baja { get; set; }
    public DateTime FechaNacimiento { get; set; }
    public List<TelefonoDTO>? Telefonos { get; set; }
    public List<EmailDTO>? Emails { get; set; }
    public DocumentacionDTO Documentacion { get; set; }
    public List<DireccionDTO>? Direcciones { get; set; }
    public HistorialTerapeuticoResponse? SituacionesTerapeuticas { get; set; }
}
