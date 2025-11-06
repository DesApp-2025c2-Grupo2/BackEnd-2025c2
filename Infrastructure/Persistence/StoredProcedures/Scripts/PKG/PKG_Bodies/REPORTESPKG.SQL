CREATE OR REPLACE PACKAGE REPORTESPKG AS

/*
estos son los reportes validos:
- Alta de afiliados por periodo. Parámetros fecha desde y hasta (opcionales).
- Alta de prestadores por periodo. Parámetros fecha desde y hasta (opcionales).
- Cantidad de prestadores por especialidad y por código postal. Sin parámetros.
- Reporte de situaciones terapéuticas por afiliado. Parámetro AfiliadoID o NroAfiliado (opcional)
- Prestadores sin agendas de turnos cargadas. Sin parámetros.


public class ReporteRequest
{
    public int TipoReporte { get; set; }
    public DateTime? FechaDesde { get; set; }
    public DateTime? FechaHasta { get; set; }
    public int? AfiliadoId { get; set; }
}


public class Reporte
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    [MaxLength(8)]
    public string CodigoIdentificatorio { get; set; }
    [Required]
    public TipoReporte Tipo { get; set; }
    public DateTime? FechaDesde { get; set; }
    public DateTime? FechaHasta { get; set; }
    public int? AfiliadoId { get; set; }
    [Required]
    public DateTime FechaGeneracion { get; set; }
}
*/
    TYPE RCT1 IS REF C
-- PRIMERO UN PROC ORQUESTADOR, ES DECIR QUE SIEMPRE SE VA A LLAMAR A ESTE REPORTE PERO ESTE REPORTE VA DECIDIR A QUE PROC PARTICULAR LLAMAR.
    PROCEDURE GENERAR_REPORTE(  NTIPO IN NUMBER,
                                SHEXACODE IN CHAR DEFAULT NULL,
                                DDESDE IN CHAR DEFAULT NULL,
                                DHASTA IN CHAR DEFAULT NULL,
                                NAFILIADOID IN NUMBER DEFAULT NULL);
                                







END REPORTESPKG;
/



/*
SELECT (PER.NOMBRE || ' ' || PER.APELLIDO) AS NOMBRE_COMPLETO, AF.ALTA FECHA_ALTA, () CANTIDAD_INTEGRANTES 
  FROM AFILIADOS AF
       INNER JOIN PLANES_MEDICOS PM ON AF.PLANMEDICOID = PM.ID
       INNER JOIN PERSONAS PER ON PER.ID = AF.TITULARID
 WHERE AF.ALTA BETWEEN TO_DATE('01/01/0001', 'DD/MM/YYYY') AND TO_DATE(SYSDATE, 'DD/MM/YYYY')




;

SELECT COUNT(*) FROM PERSONAS P2 WHERE P2.AFILIADOID = AF.ID AND (P2.BAJA IS NULL OR TO_DATE(P2.BAJA,'DD/MM/YYYY') >  TO_DATE(SYSDATE,'DD/MM/YYYY'))

*/