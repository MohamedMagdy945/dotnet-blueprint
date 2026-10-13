using Blueprint.API.Extensions;
using Blueprint.API.Infrastructure.Handlers;
using Blueprint.API.Infrastructure.Middlewares;
using Serilog;
using Serilog.Enrichers.Span;

namespace Blueprint.API;

public static class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateBootstrapLogger();

        try
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. Serilog Setup
            builder.Host.UseSerilog((context, services, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .Enrich.WithSpan());
            builder.Services.AddAuthorization();
            builder.Services.AddApiServices(builder.Configuration);

            builder.Services.AddProblemDetails();
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

            var app = builder.Build();

            // 3. Configure the HTTP Request Pipeline


            app.UseMiddleware<TraceIdMiddleware>();
            app.UseMiddleware<RequestLoggingMiddleware>();
            app.UseExceptionHandler();

            app.UseCors();

            app.UseAuthorization();

            app.MapEndpoints(typeof(Program).Assembly);

            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}