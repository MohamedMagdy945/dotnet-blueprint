using System.Diagnostics;

namespace Blueprint.API.RequestPipeline;

public sealed class TraceIdMiddleware
{
    private const string TraceIdHeader = "X-Trace-ID";
    private readonly RequestDelegate _next;

    public TraceIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

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

        await _next(context);
    }
}