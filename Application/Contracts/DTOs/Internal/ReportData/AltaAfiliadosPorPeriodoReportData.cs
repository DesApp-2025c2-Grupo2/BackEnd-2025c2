using Domain.Entities;

namespace Application.Contracts.DTOs.Internal.ReportData;

public class AltaAfiliadosPorPeriodoReportDataRow : ReportDataRow
{
    //SELECT AF.ALTA AS FECHAALTA,
    //       P.NOMBRE || ' ' || P.APELLIDO AS NOMBRECOMPLETO,
    //       D.NUMERO AS DOCUMENTO,
    //       PM.NOMBRE AS PLANMEDICO,
    //       TO_CHAR(SELECT COUNT(*) FROM PERSONAS P2 WHERE P2.AFILIADOID = AF.ID) AS INTEGRANTES
    //  FROM AFILIADOS AF
    //       INNER JOIN PERSONAS P ON P.AFILIADOID = AF.ID
    //       INNER JOIN DOCUMENTACIONES D ON D.PERSONAID = P.ID
    //       INNER JOIN PLANES_MEDICOS PM ON AF.PLANMEDICOID = PM.ID
    // WHERE AF.TITULARID = P.ID
    //       AND TRUNC(AF.ALTA) BETWEEN TRUNC(NVL(DSTARTDATE, AF.ALTA)) AND TRUNC(NVL(DENDDATE, AF.ALTA))
    //;
    public DateTime FechaAlta { get; set; }
    public string NombreCompleto { get; set; }
    public string Documento { get; set; }
    public string PlanMedico { get; set; }
    public string Integrantes { get; set; }
}