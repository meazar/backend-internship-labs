using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CoursePortalMVC.Filters
{
    // Custom Authorization Filter to protect admin-only routes
    public class AdminAuthorizeAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Check session before executing controller action
            var isLoggedIn = context.HttpContext.Session.GetString("IsAdminLoggedIn");
            if (isLoggedIn != "true")
            {
                // Redirect unauthorized users to login page
                context.Result = new RedirectToActionResult("Login", "User", null);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // No logic needed after action execution
        }
    }
}
