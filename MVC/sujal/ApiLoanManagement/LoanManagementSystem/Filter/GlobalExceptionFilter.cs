using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LoanManagementSystem.Filter
{
    public class GlobalExceptionFilter : IExceptionFilter 
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }


        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "An unhandled exception occurred");
            var result = new ObjectResult
                (new
                {
                    Success = false,
                    Message = "An internal server error occurred",
                    Timestamp = DateTime.UtcNow

                })
            {
                StatusCode = 500
            };

            context.Result = result;
            context.ExceptionHandled = true;

        }

    }
}
