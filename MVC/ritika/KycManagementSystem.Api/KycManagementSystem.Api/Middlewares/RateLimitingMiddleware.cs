using Microsoft.AspNetCore.Http;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System;

public class RateLimitingMiddleware
{
    private readonly RequestDelegate _next;
    private static readonly ConcurrentDictionary<string, (int Count, DateTime WindowStart)> _clients =
        new ConcurrentDictionary<string, (int, DateTime)>();

    private readonly int _maxRequests;
    private readonly TimeSpan _window;

    public RateLimitingMiddleware(RequestDelegate next, int maxRequests = 100, int windowSeconds = 60)
    {
        _next = next;
        _maxRequests = maxRequests;
        _window = TimeSpan.FromSeconds(windowSeconds);
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var clientIp = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        var entry = _clients.GetOrAdd(clientIp, (0, DateTime.UtcNow));

        if (DateTime.UtcNow - entry.WindowStart > _window)
        {
            entry = (0, DateTime.UtcNow);
        }

        if (entry.Count >= _maxRequests)
        {
            context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            await context.Response.WriteAsync("Rate limit exceeded. Try again later.");
            return;
        }

        _clients[clientIp] = (entry.Count + 1, entry.WindowStart);

        await _next(context);
    }
}
