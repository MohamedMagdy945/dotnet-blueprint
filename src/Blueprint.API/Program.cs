using Blueprint.API.Extensions;
using Blueprint.API.RequestPipeline;
using Serilog;

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
                .Enrich.FromLogContext());

            builder.Services.AddAuthorization();
            builder.Services.AddApiServices(builder.Configuration);

            builder.Services.AddProblemDetails();
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

            var app = builder.Build();

            // 3. Configure the HTTP Request Pipeline

            app.UseExceptionHandler();

            app.UseMiddleware<TraceIdMiddleware>();

            app.UseSerilogRequestLogging(options =>
            {
                options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
            });

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