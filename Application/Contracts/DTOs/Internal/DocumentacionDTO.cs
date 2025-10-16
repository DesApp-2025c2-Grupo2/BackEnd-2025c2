using Domain.Enums;

namespace Application.Contracts.DTOs.Internal;

public class DocumentacionDTO
{
    public int Id { get; set; }
    public required TipoDocumento TipoDocumento { get; set; }
    public required string Numero { get; set; }
}
