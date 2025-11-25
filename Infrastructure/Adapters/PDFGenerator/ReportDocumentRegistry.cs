using Application.Contracts.DTOs.Internal.ReportData;
using Infrastructure.Adapters.PDFGenerator.Documents;
using QuestPDF.Infrastructure;

namespace Infrastructure.Adapters.PDFGenerator;

public static class ReportDocumentRegistry
{
    private static readonly Dictionary<Type, Type> Registry = new()
    {
        { typeof(AltaAfiliadosPorPeriodoReportDataRow), typeof(AltaAfiliadosDocument) },
        { typeof(AltaPrestadoresPorPeriodoReportDataRow), typeof(AltaPrestadoresDocument) },
        { typeof(PrestadoresPorEspecialidadYCodigoPostalReportDataRow), typeof(PrestadoresPorEspecialidadYCodigoPostalDocument) },
        { typeof(SituacionesTerapeuticasPorAfiliadoReportDataRow), typeof(SituacionesTerapeuticasPorAfiliadoDocument) },
        { typeof(PrestadoresSinAgendasReportDataRow), typeof(PrestadoresSinAgendasDocument) }
    };

    public static ReportDocument<T> ResolveFor<T>(
        ReportDataList<T> data,
        string logoPath,
        string reportCode
    ) where T : ReportDataRow
    {
        var rowType = typeof(T);

        if (!Registry.TryGetValue(rowType, out var documentType)) throw new NotImplementedException($"No hay documento para {rowType.Name}");

        return (ReportDocument<T>)Activator.CreateInstance(documentType, data, logoPath, reportCode)!;
    }
}