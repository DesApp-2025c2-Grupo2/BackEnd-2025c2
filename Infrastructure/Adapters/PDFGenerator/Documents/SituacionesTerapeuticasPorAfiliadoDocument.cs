using Domain.DataModels;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace Infrastructure.Adapters.PDFGenerator.Documents;

public class SituacionesTerapeuticasPorAfiliadoDocument : ReportDocument<SituacionesTerapeuticasPorAfiliadoReportDataRow>
{
    public SituacionesTerapeuticasPorAfiliadoDocument(
        ReportDataList<SituacionesTerapeuticasPorAfiliadoReportDataRow> data,
        string logoPath,
        string reportID
    ) : base(data, logoPath, "Situaciones Terapéuticas por Afiliado", reportID) { }
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
            // Agrupamos por NumeroAfiliado
            var groupedData = data.GroupBy(row => row.NumeroAfiliado);
            // Recorremos cada grupo
            foreach (var afiliadoGroup in groupedData)
            {
                // Agregamos un encabezado para el integrante titular
                var titular = afiliadoGroup.FirstOrDefault(r => r.Parentesco == "0");
                col.Item().Text($"Afiliado N° {afiliadoGroup.Key} - {titular?.NombreCompleto}").Style(PDFStyles.Header1);
                col.Item().PaddingBottom(5);
                // Agregamos una tabla para las situaciones terapeuticas de todo el Grupo Familiar
                col.Item().Table(table =>
                {
                    // Definimos las columnas que son 1 por cada propiedad de SituacionesTerapeuticasPorAfiliadoReportDataRow excepto NumeroAfiliado
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(2); // NombreCompleto
                        columns.RelativeColumn(1); // Parentesco
                        columns.RelativeColumn(5); // SituacionesTerapeuticas
                    });

                    // Agregamos el encabezado de la tabla
                    table.Header(header =>
                    {
                        header.Cell().Text("Nombre Completo").Style(PDFStyles.TableHeaderText);
                        header.Cell().Text("Parentesco").Style(PDFStyles.TableHeaderText);
                        header.Cell().Text("Situaciones Terapéuticas").Style(PDFStyles.TableHeaderText);
                    });

                    // Agregamos las filas de datos
                    foreach (var row in afiliadoGroup)
                    {
                        table.Cell().Text(row.NombreCompleto).Style(PDFStyles.TableText);
                        string parentescoStr = row.Parentesco switch
                        {
                            "0" => "Titular",
                            "1" => "Cónyuge",
                            "2" => "Hijo/a",
                            "3" => "Familiar a Cargo",
                            _ => "Desconocido"
                        };
                        table.Cell().Text(parentescoStr).Style(PDFStyles.TableText);
                        // Situaciones terapeuticas puede llegar vacio, en ese caso mostramos no aplica "N/A"
                        // en otros caso puede ser bastante larga
                        table.Cell().Text(row.SituacionesTerapeuticas ?? "N/A").Style(PDFStyles.TableText);
                    }
                    table.Cell().ColumnSpan(3).Text("");
                    table.Cell().ColumnSpan(3).Text("");
                });
            }
        });
    }
}
