create or replace PACKAGE BODY REPORTDATAGENERATOR AS

/*------------------------------------------------------------------------------*/
/* NOMBRE    : GETREPORTDATA                                                    */
/* OBJETIVO  : GENERAR DATOS PARA REPORTES DEL MÓDULO ADMINISTRACIÓN            */
/* PARAMETROS: 1 - SCODE        : CODIGO DE IDENTIFICACION DEL REPORTE 		    */
/*             2 - NTYPEREPORT  : TIPO DE REPORTE A GENERAR                     */
/*             3 - DSTARTDATE   : FECHA INICIAL DEL PERIODO A CONSULTAR         */
/*             4 - DENDDATE     : FECHA FINAL DEL PERIODO A CONSULTAR           */
/*             5 - NAFILIADOID  : ID DEL AFILIADO (SI APLICA)                   */
/*             6 - REPORTCURSOR : CURSOR DE SALIDA CON LOS DATOS DEL REPORTE    */
/*                                                                              */
/* SOURCESAFE INFORMATION:                                                      */
/*     $Author: DAMIAN $                                                        */
/*     $Date: 13/11/25 12:00 $                                                  */
/*------------------------------------------------------------------------------*/
PROCEDURE CREATERETRIEVEREPORTDATA (
    SCODE           IN NVARCHAR2 DEFAULT NULL,
    NTYPEREPORT     IN NUMBER DEFAULT NULL,
    DSTARTDATE      IN DATE DEFAULT NULL,
    DENDDATE        IN DATE DEFAULT NULL,
    NAFILIADOID     IN NUMBER DEFAULT NULL,
    REPORTCURSOR    OUT SYS_REFCURSOR
) IS
V_SCODE         NVARCHAR2(8):= SCODE;
V_NTYPEREPORT   NUMBER      := NTYPEREPORT;
V_DSTARTDATE    DATE        := DSTARTDATE;
V_DENDDATE      DATE        := DENDDATE;
V_NAFILIADOID   NUMBER      := NAFILIADOID;
BEGIN
    -- SI TODOS LOS PARAMETROS SON NULL, SE GENERA ERROR
    IF SCODE IS NULL AND NTYPEREPORT IS NULL THEN
        RAISE_APPLICATION_ERROR(-20001, 'Parámetros inválidos para la generación o recuperación del reporte.');
    END IF;

    IF SCODE IS NOT NULL THEN
        /* RECUPERAR PARÁMETROS DEL REPORTE YA GENERADO */
        SELECT NTYPEREPORT,
               DSTARTDATE,
               DENDDATE,
               NAFILIADOID
          INTO CREATERETRIEVEREPORTDATA.V_NTYPEREPORT,
               CREATERETRIEVEREPORTDATA.V_DSTARTDATE,
               CREATERETRIEVEREPORTDATA.V_DENDDATE,
               CREATERETRIEVEREPORTDATA.V_NAFILIADOID
          FROM REPORTES
          WHERE CODIGOIDENTIFICATORIO = SCODE
        ;
    ELSE
        /* GENERAR NUEVO CÓDIGO IDENTIFICATORIO Y GUARDAR PARÁMETROS DEL REPORTE */
        SELECT NVL(MAX(CODIGOIDENTIFICATORIO), '0') INTO V_SCODE FROM REPORTES;
        
        V_SCODE := DECIMAL_TO_HEXA(HEXA_TO_DECIMAL(V_SCODE) + 1);
        INSERT INTO REPORTES ( CODIGOIDENTIFICATORIO,
            TIPO,
            FECHADESDE,
            FECHAHASTA,
            AFILIADOID,
            FECHAGENERACION
        ) VALUES (
            V_SCODE,
            V_NTYPEREPORT,
            V_DSTARTDATE,
            V_DENDDATE,
            V_NAFILIADOID,
            TRUNC(SYSDATE)
        );
        COMMIT;
    END IF;

    CASE NTYPEREPORT
      WHEN 1 THEN  -- Altas de Afiliados
        REPORTE_AFILIADOS(SCODE, DSTARTDATE, DENDDATE, REPORTCURSOR);

      WHEN 2 THEN  -- Altas de Prestadores
        REPORTE_PRESTADORES(SCODE, DSTARTDATE, DENDDATE, REPORTCURSOR);

      WHEN 3 THEN  -- Prestadores por Especialidad y Código Postal
        REPORTE_ESPECIALIDAD_CP(SCODE, REPORTCURSOR);

      WHEN 4 THEN  -- Situaciones Terapéuticas por Afiliado
        REPORTE_SITUACIONES(SCODE, NAFILIADOID, REPORTCURSOR);

      WHEN 5 THEN  -- Prestadores sin Agenda
        REPORTE_SIN_AGENDA(SCODE, REPORTCURSOR);

      ELSE
        RAISE_APPLICATION_ERROR(-20001, 'Tipo de reporte no reconocido: ' || NTYPEREPORT);
    END CASE;
END CREATERETRIEVEREPORTDATA;
/*==============================================================*/
/* REPORTE 1 - ALTAS DE AFILIADOS POR PERÍODO                   */
/*==============================================================*/
PROCEDURE REPORTE_AFILIADOS (
    SCODE           IN NVARCHAR2,
    DSTARTDATE      IN DATE,
    DENDDATE        IN DATE,
    REPORTCURSOR    IN OUT SYS_REFCURSOR
) IS
BEGIN
    OPEN REPORTCURSOR FOR
        SELECT SCODE,
               P.NOMBRE || ' ' || P.APELLIDO AS NOMBRECOMPLETO,
               D.NUMERO AS DOCUMENTO,
               PM.NOMBRE AS PLANMEDICO,
               (SELECT COUNT(*) FROM PERSONAS P2 WHERE P2.AFILIADOID = AF.ID) AS INTEGRANTES
          FROM AFILIADOS AF
               INNER JOIN PERSONAS P ON P.AFILIADOID = AF.ID
               INNER JOIN DOCUMENTACIONES D ON D.PERSONAID = P.ID
               INNER JOIN PLANES_MEDICOS PM ON AF.PLANMEDICOID = PM.ID
         WHERE AF.TITULARID = P.ID
               AND TRUNC(AF.ALTA) BETWEEN TRUNC(NVL(DSTARTDATE,AF.ALTA)) AND TRUNC(NVL(DENDDATE,AF.ALTA))
        ;
  END REPORTE_AFILIADOS;

  /*==============================================================*/
  /* REPORTE 2 - ALTAS DE PRESTADORES POR PERÍODO                 */
  /*==============================================================*/
  PROCEDURE REPORTE_PRESTADORES ( 
    SCODE           IN NVARCHAR2,
    DSTARTDATE      IN DATE,
    DENDDATE        IN DATE,
    REPORTCURSOR    IN OUT SYS_REFCURSOR
  ) IS
  BEGIN
    OPEN REPORTCURSOR FOR
        SELECT SCODE,
               P.NOMBRECOMPLETO,
               MIN(D.NUMERO) AS DOCUMENTO,
               LISTAGG(DISTINCT E.NOMBRE, ', ') WITHIN GROUP (ORDER BY E.NOMBRE) AS ESPECIALIDADES
        FROM PRESTADORES P
             INNER JOIN DOCUMENTACIONES D ON D.PRESTADORID = P.ID
             INNER JOIN ESPECIALIZACIONES EP ON EP.PRESTADORID = P.ID
             INNER JOIN ESPECIALIDADES E ON EP.ESPECIALIDADID = E.ID
        WHERE TRUNC(P.ALTA) BETWEEN TRUNC(NVL(DSTARTDATE,P.ALTA)) AND TRUNC(NVL(DENDDATE,P.ALTA))
        GROUP BY P.ID, P.NOMBRECOMPLETO
        ;
  END REPORTE_PRESTADORES;

  /*==============================================================*/
  /* REPORTE 3 - PRESTADORES POR ESPECIALIDAD Y CÓDIGO POSTAL     */
  /*==============================================================*/
  PROCEDURE REPORTE_ESPECIALIDAD_CP (
    SCODE           IN NVARCHAR2,
    REPORTCURSOR    IN OUT SYS_REFCURSOR
  ) IS
  BEGIN
    OPEN REPORTCURSOR FOR
        SELECT SCODE,
               E.NOMBRE AS ESPECIALIDAD,
               P.NOMBRECOMPLETO,
               D.NUMERO AS DOCUMENTO,
               1688 AS CP
        FROM PRESTADORES P
             INNER JOIN DOCUMENTACIONES D ON D.PRESTADORID = P.ID
             INNER JOIN ESPECIALIZACIONES EP ON EP.PRESTADORID = P.ID
             INNER JOIN ESPECIALIDADES E ON EP.ESPECIALIDADID = E.ID
        WHERE P.BAJA IS NULL OR TRUNC(P.BAJA) >= TRUNC(SYSDATE)
        ORDER BY E.NOMBRE
        ;
  END REPORTE_ESPECIALIDAD_CP;

  /*==============================================================*/
  /* REPORTE 4 - SITUACIONES TERAPÉUTICAS POR AFILIADO            */
  /*==============================================================*/
  PROCEDURE REPORTE_SITUACIONES (
    SCODE           IN NVARCHAR2,
    NAFILIADOID     IN NUMBER,
    REPORTCURSOR    IN OUT SYS_REFCURSOR
  ) IS
  BEGIN
    OPEN REPORTCURSOR FOR
        SELECT SCODE,
               A.NUMEROAFILIADO,
               P.NOMBRE || ' ' || P.APELLIDO AS NOMBRECOMPLETO,
               P.PARENTESCO,
               LISTAGG(DISTINCT ST.NOMBRE, ', ') WITHIN GROUP (ORDER BY ST.NOMBRE) AS SITUACIONESTERAPEUTICAS
          FROM AFILIADOS A
               INNER JOIN PERSONAS P ON P.AFILIADOID = A.ID 
               LEFT JOIN HISTORIALESTERAPEUTICOS HT ON HT.PERSONAID = P.ID
               LEFT JOIN SITUACIONES_TERAPEUTICAS ST ON HT.SITUACIONTERAPEUTICAID = ST.ID
         WHERE A.ID = NVL(NAFILIADOID,A.ID)
           AND (A.BAJA IS NULL OR TRUNC(A.BAJA) >= TRUNC(SYSDATE))
           AND (HT.FECHAFIN IS NULL OR TRUNC(HT.FECHAFIN) >= TRUNC(SYSDATE))
         GROUP BY A.NUMEROAFILIADO, P.NOMBRE, P.APELLIDO, P.PARENTESCO, A.TITULARID
         ORDER BY A.TITULARID
        ;
  END REPORTE_SITUACIONES;
  /*==============================================================*/
  /* REPORTE 5 - PRESTADORES SIN AGENDA DE TURNOS                 */
  /*==============================================================*/
  PROCEDURE REPORTE_SIN_AGENDA (
    SCODE           IN NVARCHAR2,
    REPORTCURSOR    IN OUT SYS_REFCURSOR
  ) IS
  BEGIN
    OPEN REPORTCURSOR FOR
        SELECT SCODE,
               P.NOMBRECOMPLETO,
               D.NUMERO DOCUMENTO
          FROM PRESTADORES P
               INNER JOIN DOCUMENTACIONES D ON D.PRESTADORID = P.ID 
               LEFT JOIN AGENDAS AG ON AG.PROFESIONALID = P.ID
               LEFT JOIN HORARIOS_ATENCION HA ON HA.AGENDAID = AG.ID
         WHERE HA.ID IS NULL
           AND P.BAJA IS NULL OR TRUNC(P.BAJA) >= TRUNC(SYSDATE)
        ;
  END REPORTE_SIN_AGENDA;

END REPORTDATAGENERATOR;