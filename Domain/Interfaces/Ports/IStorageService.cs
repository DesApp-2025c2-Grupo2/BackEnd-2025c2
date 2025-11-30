namespace Domain.Interfaces.Ports;

public interface IStorageService
{
    Task SaveAsync(string name, byte[] pdfBytes);
}
