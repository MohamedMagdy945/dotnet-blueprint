using Serilog;
using Serilog.Enrichers.Span;

namespace Blueprint.API.Infrastructure.Configurations;

public static class LoggingConfiguration
{
    public static void ConfigureBootstrapLogger()
    {
        Log.Logger = new LoggerConfiguration()
           .WriteTo.Console()
           .CreateBootstrapLogger();
    }

    public static WebApplicationBuilder AddSerilogLogging(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, services, configuration) => configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .Enrich.WithSpan());

        return builder;
    }
}