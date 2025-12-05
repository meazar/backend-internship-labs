using System.Threading.RateLimiting;

namespace APIBusSeatBookingManagement.MiddleWare
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly RateLimiter _rateLimiter;


        public RateLimitingMiddleware(RequestDelegate next)
        {
            _next = next;
            _rateLimiter = new FixedWindowRateLimiter(new FixedWindowRateLimiterOptions
            {
                Window = TimeSpan.FromMinutes(5),
                PermitLimit = 100,
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 10,
            });
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var lease = await _rateLimiter.AcquireAsync();

            if (!lease.IsAcquired)
            {
                context.Response.StatusCode = 429;
                await context.Response.WriteAsync("Too many request. please try again later...");

                return;
            }
            try
            {
                await _next(context);

            }
            finally
            {
                lease.Dispose();
            }
        }
    }

}
