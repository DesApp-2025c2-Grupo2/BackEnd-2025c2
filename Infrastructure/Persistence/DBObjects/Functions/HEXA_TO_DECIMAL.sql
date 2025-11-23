CREATE OR REPLACE FUNCTION HEXA_TO_DECIMAL (NHEXA IN NVARCHAR2)
RETURN NUMBER IS
/*--------------------------------------------------------------------------*/
/*  Function: HEXA_TO_DECIMAL                                                */
/*  Objetivo: Convierte una cadena hexadecimal a su representación decimal.	*/
/*  Parámetros: NHEXA - Cadena hexadecimal a convertir.                     */
/*                                                                          */
/* SOURCESAFE INFORMATION:                                                  */
/*     $Author: DAMIAN $                                                    */
/*     $Date: 13/11/25 12:00 $                                              */
/*--------------------------------------------------------------------------*/

BEGIN
  RETURN TO_NUMBER(NHEXA, 'XXXXXXXXXXXXXXXX');
END HEXA_TO_DECIMAL;