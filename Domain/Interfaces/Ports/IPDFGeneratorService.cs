using Domain.DataModels;

namespace Domain.Interfaces.Ports;

public interface IPDFGeneratorService
{
    byte[] GenerateReportPDF<T>(string reportCode, ReportDataList<T> data) where T : ReportDataRow;

}
