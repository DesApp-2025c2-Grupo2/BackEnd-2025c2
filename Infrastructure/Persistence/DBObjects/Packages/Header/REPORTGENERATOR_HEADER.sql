CREATE OR REPLACE PACKAGE REPORTDATAGENERATOR AS
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
  PROCEDURE GETREPORTDATA ( NTYPEREPORT     IN NUMBER,
                            DSTARTDATE      IN DATE DEFAULT NULL,
                            DLASTDATE       IN DATE DEFAULT NULL,
                            NAFILIADOID     IN NUMBER DEFAULT 0,
                            REPORTCURSOR    OUT SYS_REFCURSOR
  );

  -- Reporte 1: Altas de Afiliados por período
  PROCEDURE REPORTE_AFILIADOS ( DSTARTDATE IN DATE,
                                DLASTDATE  IN DATE,
                                REPORTCURSOR OUT SYS_REFCURSOR
  );

  -- Reporte 2: Altas de Prestadores por período
  PROCEDURE REPORTE_PRESTADORES ( DSTARTDATE IN DATE,
                                  DLASTDATE  IN DATE,
                                  REPORTCURSOR OUT SYS_REFCURSOR
  );

  -- Reporte 3: Prestadores por Especialidad y Código Postal
  PROCEDURE REPORTE_ESPECIALIDAD_CP ( REPORTCURSOR OUT SYS_REFCURSOR
  );

  -- Reporte 4: Situaciones Terapéuticas por Afiliado
  PROCEDURE REPORTE_SITUACIONES ( NAFILIADOID IN NUMBER,
                                  REPORTCURSOR OUT SYS_REFCURSOR
  );

  -- Reporte 5: Prestadores sin Agenda de Turnos
  PROCEDURE REPORTE_SIN_AGENDA ( REPORTCURSOR OUT SYS_REFCURSOR
  );

END REPORTDATAGENERATOR;