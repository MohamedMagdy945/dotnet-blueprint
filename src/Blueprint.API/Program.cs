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

        app.UseCors();

        // Configure the HTTP request pipeline.
        builder.Services.AddProblemDetails();

        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

        app.UseAuthorization();

        app.Run();
    }
}

