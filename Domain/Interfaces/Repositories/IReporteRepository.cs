using Domain.Entities;
using System.Data;

namespace Domain.Interfaces.Repositories;

public interface IReporteRepository
{
    Task<List<Reporte>> GetAllAsync();
    Task<(string,DataTable)> RetrieveAsync(string hexaID);
    Task<(string,DataTable)> GenerateAsync(Reporte reporte);
}
