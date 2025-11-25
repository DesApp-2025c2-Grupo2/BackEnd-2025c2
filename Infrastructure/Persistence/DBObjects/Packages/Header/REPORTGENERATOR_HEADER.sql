create or replace PACKAGE REPORTDATAGENERATOR AS
  /*----------------------------------------------------------------*/
  /*  Package: REPORTDATAGENERATOR                                  */
  /*  Descripción: Generación de reportes del módulo Administración */
  /*  Reportes incluidos:                                           */
  /*    1. Altas de Afiliados por período                           */
  /*    2. Altas de Prestadores por período                         */
  /*    3. Prestadores por Especialidad y Código Postal             */
  /*    4. Situaciones Terapéuticas por Afiliado                    */
  /*    5. Prestadores sin Agenda de Turnos                         */
  /*----------------------------------------------------------------*/
  -- Procedure principal que actúa como router
  PROCEDURE CREATERETRIEVEREPORTDATA (  SCODE           IN  REPORTES.CODIGOIDENTIFICATORIO%TYPE DEFAULT NULL,
                                        NTYPEREPORT     IN  REPORTES.TIPO%TYPE DEFAULT NULL,
                                        DSTARTDATE      IN  REPORTES.FECHADESDE%TYPE DEFAULT NULL,
                                        DENDDATE        IN  REPORTES.FECHAHASTA%TYPE DEFAULT NULL,
                                        NAFILIADOID     IN  REPORTES.AFILIADOID%TYPE DEFAULT NULL,
                                        HEXAID          OUT REPORTES.CODIGOIDENTIFICATORIO%TYPE,
                                        REPORTCURSOR    OUT SYS_REFCURSOR
  );
  -- Reporte 1: Altas de Afiliados por período
  PROCEDURE REPORTE_AFILIADOS ( DSTARTDATE      IN DATE,
                                DENDDATE        IN DATE,
                                REPORTCURSOR    IN OUT SYS_REFCURSOR
  );

  -- Reporte 2: Altas de Prestadores por período
  PROCEDURE REPORTE_PRESTADORES ( DSTARTDATE    IN DATE,
                                  DENDDATE      IN DATE,
                                  REPORTCURSOR  IN OUT SYS_REFCURSOR
  );

  -- Reporte 3: Prestadores por Especialidad y Código Postal
  PROCEDURE REPORTE_ESPECIALIDAD_CP (   REPORTCURSOR    IN OUT SYS_REFCURSOR
  );

  -- Reporte 4: Situaciones Terapéuticas por Afiliado
  PROCEDURE REPORTE_SITUACIONES ( NAFILIADOID   IN NUMBER,
                                  REPORTCURSOR  IN OUT SYS_REFCURSOR
  );

  -- Reporte 5: Prestadores sin Agenda de Turnos
  PROCEDURE REPORTE_SIN_AGENDA ( REPORTCURSOR   IN OUT SYS_REFCURSOR 
  );

END REPORTDATAGENERATOR;