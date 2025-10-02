using Domain.Entities;

namespace Application.Contracts.DTOs.Internal;

public class PersonaDTO
{

    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public DateTime FechaNacimiento { get; set; }
    public string Parentesco { get; set; }
    public DateTime Alta { get; set; }
    public DateTime? Baja { get; set; }
}
