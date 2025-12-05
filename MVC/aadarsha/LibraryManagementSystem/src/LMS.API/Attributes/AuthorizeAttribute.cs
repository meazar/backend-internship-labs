using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LMS.API.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly string[] _roles;

    public AuthorizeAttribute(params string[] roles)
    {
        _roles = roles;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        if (!user.Identity?.IsAuthenticated ?? true)
        {
            context.Result = new JsonResult(new { message = "Unauthorized" })
            {
                StatusCode = StatusCodes.Status401Unauthorized,
            };
            return;
        }

        if (_roles.Length > 0)
        {
            var hasRequiredRole = _roles.Any(role => user.IsInRole(role));
            if (!hasRequiredRole)
            {
                context.Result = new JsonResult(new { message = "Forbidden" })
                {
                    StatusCode = StatusCodes.Status403Forbidden,
                };
            }
        }
    }
}
