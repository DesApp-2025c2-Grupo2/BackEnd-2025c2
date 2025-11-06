namespace Application.Contracts.Interfaces;

public interface IReporteService
{
    Task<byte[]> GenerarReporteEjemplo();
}
