using Domain.Enums;

namespace Application.Contracts.DTOs.Internal;

public class DocumentacionDTO
{
    public int? id { get; set; }
    public TipoDocumento tipoDocumento { get; set; }// 1: Documento Nacional de Identidad, 2: Cédula de Identidad, 3: Matricula Nacional, 4: CUIL, 5: RUT, 6: CUIT
    public string numero { get; set; }
}
