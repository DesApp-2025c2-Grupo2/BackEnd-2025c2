CREATE OR REPLACE PACKAGE BODY REPORTDATAGENERATOR AS

  /*==============================================================*/
  /* PROCEDURE PRINCIPAL - ROUTER DE REPORTES                     */
  /*==============================================================*/
  PROCEDURE GETREPORTDATA (
    NTYPEREPORT   IN NUMBER,
    DSTARTDATE    IN DATE DEFAULT NULL,
    DLASTDATE     IN DATE DEFAULT NULL,
    NAFILIADOID   IN NUMBER DEFAULT 0,
    REPORTCURSOR  OUT SYS_REFCURSOR
  ) IS
  BEGIN
    CASE NTYPEREPORT
      WHEN 1 THEN  -- Altas de Afiliados
        REPORTE_AFILIADOS(DSTARTDATE, DLASTDATE, REPORTCURSOR);

      WHEN 2 THEN  -- Altas de Prestadores
        REPORTE_PRESTADORES(DSTARTDATE, DLASTDATE, REPORTCURSOR);

      WHEN 3 THEN  -- Prestadores por Especialidad y Código Postal
        REPORTE_ESPECIALIDAD_CP(REPORTCURSOR);

      WHEN 4 THEN  -- Situaciones Terapéuticas por Afiliado
        REPORTE_SITUACIONES(NAFILIADOID, REPORTCURSOR);

      WHEN 5 THEN  -- Prestadores sin Agenda
        REPORTE_SIN_AGENDA(REPORTCURSOR);

      ELSE
        RAISE_APPLICATION_ERROR(-20001, 'Tipo de reporte no reconocido: ' || NTYPEREPORT);
    END CASE;
  END GETREPORTDATA;

  /*==============================================================*/
  /* REPORTE 1 - ALTAS DE AFILIADOS POR PERÍODO                   */
  /*==============================================================*/
  PROCEDURE REPORTE_AFILIADOS (
    DSTARTDATE   IN DATE,
    DLASTDATE    IN DATE,
    REPORTCURSOR OUT SYS_REFCURSOR
  ) IS
  BEGIN
    OPEN REPORTCURSOR FOR
      SELECT 123 FROM DUAL;
  END REPORTE_AFILIADOS;

  /*==============================================================*/
  /* REPORTE 2 - ALTAS DE PRESTADORES POR PERÍODO                 */
  /*==============================================================*/
  PROCEDURE REPORTE_PRESTADORES (
    DSTARTDATE   IN DATE,
    DLASTDATE    IN DATE,
    REPORTCURSOR OUT SYS_REFCURSOR
  ) IS
  BEGIN
    OPEN REPORTCURSOR FOR
      SELECT 123 FROM DUAL;
  END REPORTE_PRESTADORES;

  /*==============================================================*/
  /* REPORTE 3 - PRESTADORES POR ESPECIALIDAD Y CÓDIGO POSTAL     */
  /*==============================================================*/
  PROCEDURE REPORTE_ESPECIALIDAD_CP (
    REPORTCURSOR OUT SYS_REFCURSOR
  ) IS
  BEGIN
    OPEN REPORTCURSOR FOR
      SELECT 123 FROM DUAL;
  END REPORTE_ESPECIALIDAD_CP;

  /*==============================================================*/
  /* REPORTE 4 - SITUACIONES TERAPÉUTICAS POR AFILIADO            */
  /*==============================================================*/
  PROCEDURE REPORTE_SITUACIONES (
    NAFILIADOID  IN NUMBER,
    REPORTCURSOR OUT SYS_REFCURSOR
  ) IS
  BEGIN
    OPEN REPORTCURSOR FOR
      SELECT 123 FROM DUAL;
  END REPORTE_SITUACIONES;

  /*==============================================================*/
  /* REPORTE 5 - PRESTADORES SIN AGENDA DE TURNOS                 */
  /*==============================================================*/
  PROCEDURE REPORTE_SIN_AGENDA (
    REPORTCURSOR OUT SYS_REFCURSOR
  ) IS
  BEGIN
    OPEN REPORTCURSOR FOR
      SELECT 123 FROM DUAL;
  END REPORTE_SIN_AGENDA;

END REPORTDATAGENERATOR;
/
