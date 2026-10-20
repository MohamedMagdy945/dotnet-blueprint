using Blueprint.API.Extensions;
using Blueprint.API.Infrastructure.Configurations;
using Blueprint.API.Infrastructure.Middlewares;
using Blueprint.Application;
using Blueprint.Infrastructure;
using Serilog;

namespace Blueprint.API;

public static class Program
{
    public static void Main(string[] args)
    {
        LoggingConfiguration.ConfigureBootstrapLogger();

        try
        {
            DotNetEnv.Env.Load();

            var builder = WebApplication.CreateBuilder(args);


            // 1. Serilog Setup
            builder.AddSerilogLogging();

            builder.Services.AddApplicationServices();

            builder.Services.AddInfrastructureServices(builder.Configuration);

            builder.Services.AddApiServices(builder.Configuration);


            var app = builder.Build();


            // Print The Configuration Sections to the Console for Debugging Purposes
            app.Configuration.PrintAllSections();


            // 3. Configure the HTTP Request Pipeline
            app.UseSwaggerDocumentation();

            app.UseMiddleware<RequestLoggingMiddleware>();
            app.UseExceptionHandler();
            app.UseMiddleware<TraceIdMiddleware>();

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