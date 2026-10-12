using Blueprint.API.Extensions;
using Blueprint.API.RequestPipeline;

namespace Blueprint.API;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();
        builder.Services.AddApiServices(builder.Configuration);


        var app = builder.Build();

        // Configure the HTTP request pipeline.

        app.UseCors();

        builder.Services.AddProblemDetails();

        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

        app.UseAuthorization();

        app.MapEndpoints(typeof(Program).Assembly);
        
        app.Run();
    }
}

