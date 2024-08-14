using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace DotNetTestTemplate;

internal class Program
{
    static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(theme: AnsiConsoleTheme.Code,
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}]{SourceContext,14} {Message:lj}{NewLine}{Exception}")
            .CreateLogger();

        var AppHost = Host.CreateDefaultBuilder()
            .UseContentRoot(AppContext.BaseDirectory)
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton(typeof(ILogger<>), typeof(LoggerEx<>));

            })
            .UseSerilog()
            .Build();

        Log.Logger.Information("Start");

        //var logger = AppHost.Services.GetService<ILogger<AliCloudUtility>>();

        Run().Wait();

        Log.Logger.Information("End");
    }
    static async Task Run()
    {
    }
}
