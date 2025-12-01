namespace LoanManagementSystem.Middleware
{
    public class RequestloggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestloggingMiddleware> _logger;

        public RequestloggingMiddleware(RequestDelegate next, ILogger<RequestloggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;

        }

        public async Task InvokeAsync(HttpContext context)
        {
            var startingTime = DateTime.UtcNow;


            _logger.LogInformation($"Request:{context.Request.Method} {context.Request.Path}");

            await _next(context);
            var endTime = DateTime.UtcNow;

            var duration = endTime - startingTime;

            _logger.LogInformation($"Response:{context.Response.StatusCode}  -  Duration: {duration.TotalMilliseconds} ms");
        }


    }


    public static class RequestLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestLogging( this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestloggingMiddleware>();
        }
    }
}
