namespace Domain.DataModels;

public class AltaPrestadoresPorPeriodoReportDataRow : ReportDataRow
{
    public DateTime FechaAlta { get; set; }
    public string NombreCompleto { get; set; }
    public string Documento { get; set; }
    public string Especialidades { get; set; }
}
