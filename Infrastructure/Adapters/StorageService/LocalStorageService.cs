using Domain.Interfaces.Ports;
using Microsoft.AspNetCore.Hosting;

namespace Infrastructure.Adapters.StorageService;

public class LocalStorageService : IStorageService
{
    private readonly string basePath;

    public LocalStorageService(IWebHostEnvironment env)
    {
        basePath = env.WebRootPath;
    }

    public async Task SaveAsync(string fullName, byte[] pdfBytes)
    {
        string path = Path.Combine(basePath, fullName);
        await File.WriteAllBytesAsync(path, pdfBytes);
    }

}
