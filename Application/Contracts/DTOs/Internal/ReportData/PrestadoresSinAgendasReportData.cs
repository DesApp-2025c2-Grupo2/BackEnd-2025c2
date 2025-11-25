using Domain.Entities;

namespace Application.Contracts.DTOs.Internal.ReportData;

public class PrestadoresSinAgendasReportDataRow : ReportDataRow
{
    //SELECT P.NOMBRECOMPLETO AS NOMBRECOMPLETO,
    //       D.NUMERO AS DOCUMENTO,
    //       TO_CHAR(SELECT COUNT(*) FROM DIRECCIONES D2 WHERE D2.PRESTADORID = P.ID) AS DIRECCIONES
    //  FROM PRESTADORES P
    //       INNER JOIN DOCUMENTACIONES D ON D.PRESTADORID = P.ID
    //       LEFT JOIN AGENDAS AG ON AG.PROFESIONALID = P.ID
    //       LEFT JOIN HORARIOS_ATENCION HA ON HA.AGENDAID = AG.ID
    // WHERE HA.ID IS NULL
    //   AND (P.BAJA IS NULL OR TRUNC(P.BAJA) >= TRUNC(SYSDATE))
    //       AND P.ROL<> 2
    //;

    public string NombreCompleto { get; set; }
    public string Documento { get; set; }
    public string Direcciones { get; set; }
}
