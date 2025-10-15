using Domain.Enums;

namespace Application.Contracts.DTOs.Internal;

public class DocumentacionDTO
{
    public int Id { get; set; }
    public TipoDocumento TipoDocumento { get; set; }
    public string Numero { get; set; }
}
