using System.Diagnostics;

namespace Blueprint.API.Infrastructure.Middlewares;

public sealed partial class RequestLoggingMiddleware(
    RequestDelegate next,
    ILogger<RequestLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var startTimestamp = Stopwatch.GetTimestamp();

        try
        {
            await next(context);
        }
        finally
        {
            var elapsedMs = Stopwatch.GetElapsedTime(startTimestamp).TotalMilliseconds;
            var statusCode = context.Response.StatusCode;
            var method = context.Request.Method;
            var path = context.Request.Path.Value ?? "/";

            var logLevel = statusCode switch
            {
                >= 500 => LogLevel.Error,
                >= 400 => LogLevel.Warning,
                _ => LogLevel.Information
            };

            LogHttpRequest(logger, logLevel, method, path, statusCode, elapsedMs);
        }
    }

    [LoggerMessage(
        EventId = 1,
        Message = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms")]
    private static partial void LogHttpRequest(
        ILogger logger,
        LogLevel logLevel,
        string requestMethod,
        string requestPath,
        int statusCode,
        double elapsed);
}