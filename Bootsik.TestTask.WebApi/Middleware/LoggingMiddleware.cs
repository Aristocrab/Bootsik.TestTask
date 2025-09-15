using System.Diagnostics;

namespace Bootsik.TestTask.WebApi.Middleware;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = new Stopwatch();
            
        stopwatch.Start();
        await _next(context);
        stopwatch.Stop();
            
        _logger.LogInformation("{RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.000} ms",
            context.Request.Method, 
            context.Request.Path, 
            context.Response.StatusCode, 
            stopwatch.Elapsed.TotalMilliseconds);
    }
}