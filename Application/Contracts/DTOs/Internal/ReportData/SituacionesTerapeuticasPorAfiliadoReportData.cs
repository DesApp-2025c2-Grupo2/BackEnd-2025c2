namespace Application.Contracts.DTOs.Internal.ReportData;

public class SituacionesTerapeuticasPorAfiliadoReportDataRow : ReportDataRow
{
    //SELECT A.NUMEROAFILIADO,
    //       P.NOMBRE || ' ' || P.APELLIDO AS NOMBRECOMPLETO,
    //       P.PARENTESCO,
    //       LISTAGG(DISTINCT ST.NOMBRE, ', ') WITHIN GROUP(ORDER BY ST.NOMBRE) AS SITUACIONESTERAPEUTICAS
    //  FROM AFILIADOS A
    //       INNER JOIN PERSONAS P ON P.AFILIADOID = A.ID
    //       LEFT JOIN HISTORIALESTERAPEUTICOS HT ON HT.PERSONAID = P.ID
    //       LEFT JOIN SITUACIONES_TERAPEUTICAS ST ON HT.SITUACIONTERAPEUTICAID = ST.ID
    // WHERE A.ID = NVL(null, A.ID)
    //   AND (A.BAJA IS NULL OR TRUNC(A.BAJA) >= TRUNC(SYSDATE))
    //   AND(HT.FECHAFIN IS NULL OR TRUNC(HT.FECHAFIN) >= TRUNC(SYSDATE))
    // GROUP BY A.NUMEROAFILIADO, P.NOMBRE, P.APELLIDO, P.PARENTESCO, A.TITULARID
    // ORDER BY A.TITULARID, P.PARENTESCO
    //;
    public string NumeroAfiliado { get; set; }
    public string NombreCompleto { get; set; }
    public string Parentesco { get; set; }
    public string SituacionesTerapeuticas { get; set; }

}
