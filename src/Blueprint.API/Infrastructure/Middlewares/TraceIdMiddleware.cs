using System.Diagnostics;

namespace Blueprint.API.Infrastructure.Middlewares;

public sealed class TraceIdMiddleware(RequestDelegate next)
{
    private const string TraceIdHeader = "X-Trace-ID";

    public async Task InvokeAsync(HttpContext context)
    {
        var traceId = Activity.Current?.TraceId.ToString() ?? context.TraceIdentifier;

        context.Response.OnStarting(() =>
        {
            if (!context.Response.Headers.ContainsKey(TraceIdHeader))
            {
                context.Response.Headers.Append(TraceIdHeader, traceId);
            }
            return Task.CompletedTask;
        });

        await next(context);
    }
}