using Arch.Core;
using Arch.Core.Extensions;
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

        Run();

        Log.Logger.Information("End");
    }

    public struct Position { public float X, Y; }
    public struct Velocity { public float Dx, Dy; }

    static void Run()
    {
        var world = World.Create();
        for (var index = 0; index < 1000; index++)
        {
            var entity = world.Create(new Position { X = 0, Y = 0 }, new Velocity { Dx = 1, Dy = 1 });
            var refs = entity.Get<Position,Velocity>();
            entity.Set(new Position());
        }

        // Query and modify entities ( There are also alternatives without lambdas ;) ) 
        var query = new QueryDescription().WithAll<Position, Velocity>(); // Targets entities with Position AND Velocity.
        world.Query(in query, (ref Position pos, ref Velocity vel) =>
        {
            pos.X += vel.Dx;
            pos.Y += vel.Dy;
        });

        world.Query(in query, (ref Position pos) =>
        {
            Log.Logger.Information(pos.X + " " + pos.Y);
        });
    }
}
