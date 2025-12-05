using System.Net;
using System.Text.Json;

namespace APIBusSeatBookingManagement.MiddleWare
{
    public class ErrorhandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorhandlingMiddleware> _logger;

        public ErrorhandlingMiddleware(RequestDelegate next, ILogger<ErrorhandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex,"An unhandled exception occurred");
                await HandleExceptionAsync(context, ex);

            }
           
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            var result = string.Empty;

            switch (exception)
            {
                case UnauthorizedAccessException:
                    code = HttpStatusCode.Unauthorized;
                    result = JsonSerializer.Serialize(new { error = "Unauthorized access" });
                    break;
                case KeyNotFoundException:
                    code = HttpStatusCode.NotFound;
                    result = JsonSerializer.Serialize(new { error = "Resource not found" });
                    break;
                case ArgumentException:
                    code = HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(new { error = exception.Message });
                    break;
                default:
                    result = JsonSerializer.Serialize(new { error = "An internal server error occurred" });
                    break;
            }
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);

        }
    }


}
