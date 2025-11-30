using Domain.DataModels;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Infrastructure.Adapters.PDFGenerator.Documents;

public class AltaAfiliadosDocument : ReportDocument<AltaAfiliadosPorPeriodoReportDataRow>
{
    public AltaAfiliadosDocument(
        ReportDataList<AltaAfiliadosPorPeriodoReportDataRow> data,
        string logoPath,
        string reportID
    ) : base(data, logoPath, "Alta de Afiliados por Período", reportID) { }

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
                // Definimos las columnas que son 1 por cada propiedad de AltaAfiliadosPorPeriodoReportDataRow
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(1); // FechaAlta
                    columns.RelativeColumn(2); // NombreCompleto
                    columns.RelativeColumn(1); // Documento
                    columns.RelativeColumn(1); // PlanMedico
                    columns.RelativeColumn(1); // Integrantes(cantidad)

                });
                // Agregamos el encabezado de la tabla
                table.Header(header =>
                {
                    header.Cell().Text("Fecha de Alta").Style(PDFStyles.TableHeaderText);
                    header.Cell().Text("Nombre Completo").Style(PDFStyles.TableHeaderText);
                    header.Cell().Text("Documento").Style(PDFStyles.TableHeaderText);
                    header.Cell().Text("Plan Médico").Style(PDFStyles.TableHeaderText);
                    header.Cell().Text("Integrantes").Style(PDFStyles.TableHeaderText);
                });
                // Agregamos las filas de datos
                foreach (var row in data)
                {
                    table.Cell().Text(row.FechaAlta.ToString("dd/MM/yyyy")).Style(PDFStyles.TableText);
                    table.Cell().Text(row.NombreCompleto).Style(PDFStyles.TableText);
                    table.Cell().Text(row.Documento).Style(PDFStyles.TableText);
                    table.Cell().Text(row.PlanMedico).Style(PDFStyles.TableText);
                    table.Cell().Text(row.Integrantes.ToString()).Style(PDFStyles.TableText);
                }
                table.Cell().ColumnSpan(5).Text("");
                // Ahora convinamos las primeras 4 columnas en la ultima fila para mostrar el total de afiliados dados de alta
                table.Cell().ColumnSpan(2).Text("Total de afiliados dados de alta:").Style(PDFStyles.TableHeaderText).Style(PDFStyles.SpacedText);
                table.Cell().Text(data.Count.ToString()).Style(PDFStyles.TableHeaderText).Style(PDFStyles.SpacedText);
            });
        });
    }
}