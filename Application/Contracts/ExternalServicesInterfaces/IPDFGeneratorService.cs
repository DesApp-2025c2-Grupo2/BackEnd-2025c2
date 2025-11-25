using Application.Contracts.DTOs.Internal.ReportData;
using System.Data;

namespace Application.Contracts.ExternalServicesInterfaces;

public interface IPDFGeneratorService
{
    byte[] GenerateReportPDF<T>(string reportCode, ReportDataList<T> data) where T : ReportDataRow;

}
