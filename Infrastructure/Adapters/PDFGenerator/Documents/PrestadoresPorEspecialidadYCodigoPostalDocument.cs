using Application.Contracts.DTOs.Internal.ReportData;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Infrastructure.Adapters.PDFGenerator.Documents;

public class PrestadoresPorEspecialidadYCodigoPostalDocument : ReportDocument<PrestadoresPorEspecialidadYCodigoPostalReportDataRow>
{
    public PrestadoresPorEspecialidadYCodigoPostalDocument(
        ReportDataList<PrestadoresPorEspecialidadYCodigoPostalReportDataRow> data,
        string logoPath,
        string reportID
    ) : base(data, logoPath, "Prestadores por Especialidad y Código Postal", reportID) { }

    protected override void ComposeContent(IContainer container)
    {
        // Si ReportDataList está vacío usamos el metodo base
        if (data.Count == 0)
        {
            base.ComposeContent(container);
            return;
        }

        container.PaddingTop(10).Column(col =>
        {
            col.Spacing(10);
            // Como ya validamos que el data.Count > 0, podemos proceder a crear el contenido
            // Vamos a agrupar por Especialidad
            var groupedData = data.GroupBy(row => row.Especialidad);
            foreach (var specialtyGroup in groupedData)
            {
                // Agregamos un encabezado para la especialidad
                col.Item().Text($"Especialidad: {specialtyGroup.Key}").Style(PDFStyles.Header1);
                col.Item().PaddingBottom(5);
                // Agregamos una tabla para los prestadores de esta especialidad
                col.Item().Table(table =>
                {
                    // Definimos las columnas que son 1 por cada propiedad de PrestadoresPorEspecialidadYCodigoPostalReportDataRow excepto Especialidad
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(3); // NombreCompleto
                        columns.RelativeColumn(1); // Documento
                        columns.RelativeColumn(1); // CodigoPostal
                    });
                    // Agregamos el encabezado de la tabla
                    table.Header(header =>
                    {
                        header.Cell().Text("Nombre Completo").Style(PDFStyles.TableHeaderText);
                        header.Cell().Text("Documento").Style(PDFStyles.TableHeaderText);
                        header.Cell().Text("Código Postal").Style(PDFStyles.TableHeaderText);
                    });
                    // Agregamos las filas de datos
                    foreach (var row in specialtyGroup)
                    {
                        table.Cell().Text(row.NombreCompleto).Style(PDFStyles.TableText);
                        table.Cell().Text(row.Documento).Style(PDFStyles.TableText);
                        table.Cell().Text(string.IsNullOrEmpty(row.CodigoPostal) || row.CodigoPostal == "0" ? "N/D" : row.CodigoPostal).Style(PDFStyles.TableText);
                    }
                    table.Cell().ColumnSpan(3).Text("");
                    table.Cell().ColumnSpan(3).Text("");
                });
            }
        });
    }
}
