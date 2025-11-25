using Application.Contracts.DTOs.Internal.ReportData;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Infrastructure.Adapters.PDFGenerator.Documents;

public class PrestadoresSinAgendasDocument : ReportDocument<PrestadoresSinAgendasReportDataRow>
{
    public PrestadoresSinAgendasDocument(
        ReportDataList<PrestadoresSinAgendasReportDataRow> data,
        string logoPath,
        string reportID
    ) : base(data, logoPath, "Prestadores Sin Agendas", reportID) { }

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
            // Agregamos una tabla
            col.Item().Table(table =>
            {
                // Definimos las columnas que son 1 por cada propiedad de PrestadoresSinAgendasReportDataRow
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(3); // NombreCompleto
                    columns.RelativeColumn(1); // Documento
                    columns.RelativeColumn(1); // Direcciones
                });
                // Agregamos el encabezado de la tabla
                table.Header(header =>
                {
                    header.Cell().Text("Nombre Completo").Style(PDFStyles.TableHeaderText);
                    header.Cell().Text("Documento").Style(PDFStyles.TableHeaderText);
                    header.Cell().Text("Direcciones cargadas").Style(PDFStyles.TableHeaderText);
                });
                // Agregamos las filas de datos
                foreach (var row in data)
                {
                    table.Cell().Text(row.NombreCompleto).Style(PDFStyles.TableText);
                    table.Cell().Text(row.Documento).Style(PDFStyles.TableText);
                    table.Cell().Text(row.Direcciones.ToString()).Style(PDFStyles.TableText);
                }
            });
        });
    }
}
