using Domain.Entities;
using System.Data;

namespace Domain.Interfaces;

public interface IReporteRepository
{
    Task<List<Reporte>> GetAllAsync();
    Task<DataTable> RetrieveAsync(string hexaID);
    Task<(string,DataTable)> GenerateAsync(Reporte reporte);
}
