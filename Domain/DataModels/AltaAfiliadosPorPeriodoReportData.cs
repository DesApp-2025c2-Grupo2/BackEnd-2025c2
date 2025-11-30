namespace Domain.DataModels;

public class AltaAfiliadosPorPeriodoReportDataRow : ReportDataRow
{
    public DateTime FechaAlta { get; set; }
    public string NombreCompleto { get; set; }
    public string Documento { get; set; }
    public string PlanMedico { get; set; }
    public string Integrantes { get; set; }
}