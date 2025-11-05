using Microsoft.AspNetCore.Mvc;           // For Controller and IActionResult
using Microsoft.AspNetCore.Http;           // For Session management
using CoursePortalMVC.Models;             // For User model
using CoursePortalMVC.Repositories;       // For IUserRepository interface

namespace CoursePortalMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepo;

        // Constructor Dependency Injection
        public UserController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        // GET: /User/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /User/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(User user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            // Validate user credentials
            var validUser = _userRepo.ValidateUser(user.Email, user.Password);

            if (validUser != null)
            {
                // Create session for admin
                HttpContext.Session.SetString("IsAdminLoggedIn", "true");

                // Redirect to Course portal
                return RedirectToAction("Index", "Course");
            }

            // If invalid credentials, show error message
            ViewBag.ErrorMessage = "Invalid email or password.";
            return View(user);
        }

        // GET: /User/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // clear session on logout
            return RedirectToAction("Login");
        }
    }
}
