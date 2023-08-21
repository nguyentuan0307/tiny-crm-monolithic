using Microsoft.Extensions.Configuration;
using Serilog;

namespace TinyCRM.Infrastructure.Serilog;

public static class LoggerService
{
    public static void ConfigureLogger(IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();
    }

    public static void LogInformation(string message)
    {
        Log.Information(message);
    }

    public static void LogError(string message)
    {
        Log.Error(message);
    }

    public static void LogError(Exception? ex, string message)
    {
        Log.Error(ex, message);
    }

    public static void LogWarning(string message)
    {
        Log.Warning(message);
    }

    public static void LogDebug(string message)
    {
        Log.Debug(message);
    }

    public static void LogFatal(string message)
    {
        Log.Fatal(message);
    }

    public static void LogVerbose(string message)
    {
        Log.Verbose(message);
    }

    public static void LogInformation(string message, params object[] args)
    {
        Log.Information(message, args);
    }
}