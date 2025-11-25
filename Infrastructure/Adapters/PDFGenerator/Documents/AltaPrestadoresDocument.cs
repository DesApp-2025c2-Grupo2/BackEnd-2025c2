using Application.Contracts.DTOs.Internal.ReportData;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Infrastructure.Adapters.PDFGenerator.Documents;

public class AltaPrestadoresDocument : ReportDocument<AltaPrestadoresPorPeriodoReportDataRow>
{
    public AltaPrestadoresDocument(
        ReportDataList<AltaPrestadoresPorPeriodoReportDataRow> data, 
        string logoPath, 
        string reportID
    ) : base(data, logoPath, "Alta de Prestadores por Período", reportID) { }

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
                // Definimos las columnas que son 1 por cada propiedad de AltaPrestadoresPorPeriodoReportDataRow
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(1); // FechaAlta
                    columns.RelativeColumn(2); // NombreCompleto
                    columns.RelativeColumn(1); // Documento
                    columns.RelativeColumn(3); // Especialidades

                });
                // Agregamos el encabezado de la tabla
                table.Header(header =>
                {
                    header.Cell().Text("Fecha de Alta").Style(PDFStyles.TableHeaderText);
                    header.Cell().Text("Nombre Completo").Style(PDFStyles.TableHeaderText);
                    header.Cell().Text("Documento").Style(PDFStyles.TableHeaderText);
                    header.Cell().Text("Especialidades").Style(PDFStyles.TableHeaderText);
                });
                // Agregamos las filas de datos
                foreach (var row in data)
                {
                    table.Cell().Text(row.FechaAlta.ToString("dd/MM/yyyy")).Style(PDFStyles.TableText);
                    table.Cell().Text(row.NombreCompleto).Style(PDFStyles.TableText);
                    table.Cell().Text(row.Documento).Style(PDFStyles.TableText);
                    table.Cell().Text(row.Especialidades).Style(PDFStyles.TableText);
                }
                // Le damos una fila de separacion (una fila completamente en blanco
                table.Cell().ColumnSpan(4).Text("");
                // Ahora convinamos las primeras 3 columnas en la ultima fila para mostrar el total de prestadores dados de alta y las otras 2 para mostrar el total
                table.Cell().ColumnSpan(2).Text("Total de prestadores dados de alta:").Style(PDFStyles.TableHeaderText).Style(PDFStyles.SpacedText);
                table.Cell().Text(data.Count.ToString()).Style(PDFStyles.TableHeaderText).Style(PDFStyles.SpacedText);
            });
        });
    }

}
