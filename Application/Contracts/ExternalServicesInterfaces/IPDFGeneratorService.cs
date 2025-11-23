using System.Data;

namespace Application.Contracts.ExternalServicesInterfaces;

public interface IPDFGeneratorService
{
    byte[] GenerateAltaAfiliadosAsync((string, DataTable) dataTable);
    byte[] GenerateAltaPrestadoresAsync((string, DataTable) dataTable);
    byte[] GeneratePrestadoresPorEspecialidadYCodigoPostalAsync((string, DataTable) dataTable);
    byte[] GenerateSituacionesTerapeuticasPorAfiliadoAsync((string, DataTable) dataTable);
    byte[] GeneratePrestadoresSinAgendasAsync((string, DataTable) dataTable);

}
