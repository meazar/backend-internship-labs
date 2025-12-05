using LoanManagementSystem.Service;

namespace LoanManagementSystem.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<JwtMiddleware> _logger;

        public JwtMiddleware(RequestDelegate next, ILogger<JwtMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, IJwtService jwtService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                try
                {
                    var principal = jwtService.ValidateToken(token);
                    if (principal != null)
                    {
                        context.User = principal;
                        _logger.LogDebug("JWT token validated successfully for user: {UserId}",
                            principal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "JWT token validation failed");
                }
            }

            await _next(context);
        }
    }

    public static class JwtMiddlewareExtensions
    {
        public static IApplicationBuilder UseJwtMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JwtMiddleware>();
        }
    }
}

