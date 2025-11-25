using Domain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.Contracts.DTOs.Internal.ReportData;

public class PrestadoresPorEspecialidadYCodigoPostalReportDataRow : ReportDataRow
{
    //SELECT DISTINCT P.NOMBRECOMPLETO AS NOMBRECOMPLETO,
    //       E.NOMBRE AS ESPECIALIDAD,
    //       DOC.NUMERO AS DOCUMENTO,
    //       DIR.CODIGOPOSTAL AS CODIGOPOSTAL
    //  FROM PRESTADORES P
    //       INNER JOIN DOCUMENTACIONES DOC ON DOC.PRESTADORID = P.ID
    //       INNER JOIN DIRECCIONES DIR ON DIR.PRESTADORID = P.ID
    //       INNER JOIN ESPECIALIZACIONES EP ON EP.PRESTADORID = P.ID
    //       INNER JOIN ESPECIALIDADES E ON EP.ESPECIALIDADID = E.ID
    // WHERE P.BAJA IS NULL OR TRUNC(P.BAJA) >= TRUNC(SYSDATE)
    // ORDER BY E.NOMBRE
    //;
    public string NombreCompleto { get; set; }
    public string Especialidad { get; set; }
    public string Documento { get; set; }
    public string CodigoPostal { get; set; }

}
