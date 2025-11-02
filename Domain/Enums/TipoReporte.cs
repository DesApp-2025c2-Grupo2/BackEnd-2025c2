namespace Domain.Enums;

/// <summary>
/// Tipos de reportes disponibles en el sistema.
/// Algunos requieren parámetros específicos (por ejemplo, rangos de fecha o identificadores).
/// </summary>
public enum TipoReporte
{
    /// <summary>
    /// Reporte de altas de afiliados registradas dentro de un período determinado.
    /// Permite analizar el crecimiento o movimiento de afiliaciones entre fechas.
    /// </summary>
    AltaAfiliadosPorPeriodo = 1,

    /// <summary>
    /// Reporte de altas de prestadores registradas dentro de un período determinado.
    /// Facilita el seguimiento de nuevos prestadores incorporados al sistema.
    /// </summary>
    AltaPrestadoresPorPeriodo = 2,

    /// <summary>
    /// Reporte de cantidad de prestadores agrupados por especialidad y por código postal.
    /// Permite analizar la distribución geográfica y profesional de los prestadores.
    /// </summary>
    PrestadoresPorEspecialidadYCodigoPostal = 3,

    /// <summary>
    /// Reporte de situaciones terapéuticas por afiliado.
    /// Incluye las situaciones del afiliado y de su grupo familiar.
    /// </summary>
    SituacionesTerapeuticasPorAfiliado = 4,

    /// <summary>
    /// Reporte de prestadores que no tienen agendas de turnos cargadas en el sistema.
    /// Útil para detectar prestadores inactivos o con carga incompleta de disponibilidad.
    /// </summary>
    PrestadoresSinAgendas = 5
}
