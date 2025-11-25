using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.Contracts.DTOs.Internal.ReportData;

public class AltaPrestadoresPorPeriodoReportDataRow : ReportDataRow
{
    //SELECT P.ALTA AS FECHAALTA,
    //       P.NOMBRECOMPLETO,
    //       MIN(D.NUMERO) AS DOCUMENTO,
    //       LISTAGG(DISTINCT E.NOMBRE, ', ') WITHIN GROUP(ORDER BY E.NOMBRE) AS ESPECIALIDADES
    //  FROM PRESTADORES P
    //       INNER JOIN DOCUMENTACIONES D ON D.PRESTADORID = P.ID
    //       INNER JOIN ESPECIALIZACIONES EP ON EP.PRESTADORID = P.ID
    //       INNER JOIN ESPECIALIDADES E ON EP.ESPECIALIDADID = E.ID
    // WHERE TRUNC(P.ALTA) BETWEEN TRUNC(NVL(DSTARTDATE, P.ALTA)) AND TRUNC(NVL(DENDDATE, P.ALTA))
    // GROUP BY P.ID, P.NOMBRECOMPLETO
    //;
    public DateTime FechaAlta { get; set; }
    public string NombreCompleto { get; set; }
    public string Documento { get; set; }
    public string Especialidades { get; set; }
}
