CREATE OR REPLACE FUNCTION DECIMAL_TO_HEXA (NDECIMAL IN NUMBER)
RETURN NVARCHAR2 IS
/*--------------------------------------------------------------------------*/
/*  Function: DECIMAL_TO_HEXA												*/
/*  Objetivo: Convierte un número decimal a su representación hexadecimal.	*/
/*  Parámetros: NDECIMAL - Número decimal a convertir.						*/
/*																			*/
/* SOURCESAFE INFORMATION:                                                  */
/*     $Author: DAMIAN $													*/
/*     $Date: 13/11/25 12:00 $                                              */
/*--------------------------------------------------------------------------*/

BEGIN
  RETURN TO_CHAR(NDECIMAL, 'FMXXXXXXXXXXXXXXXX');

END DECIMAL_TO_HEXA;