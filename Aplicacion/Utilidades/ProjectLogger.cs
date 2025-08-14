namespace Aplicacion.Utilidades;

public interface IProjectLogger
{
    void LogInformation(string message);
    void LogWarning(string message);
    void LogError(string message, Exception exception);
    void LogSuccess(string message);
}
public class ProjectLogger : IProjectLogger
{
    public ProjectLogger()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
    }
    public void LogInformation(string message)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"ℹ  INFO: {message}");
        Console.ResetColor();
    }

    public void LogWarning(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"⚠  WARNING: {message}");
        Console.ResetColor();
    }

    public void LogError(string message, Exception exception)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"✖  ERROR: {message}");
        if (exception != null)
        {
            Console.WriteLine($"   → {exception.Message}");
        }
        Console.ResetColor();
    }

    public void LogSuccess(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"✔  SUCCESS: {message}");
        Console.ResetColor();
    }
}
