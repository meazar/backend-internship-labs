using Microsoft.AspNetCore.Mvc;
using TeacherPortalMvc.Data;
using TeacherPortalMvc.Models;
namespace TeacherPortalMvc.Controllers
{
    public class AccountController : Controller
    {
        //dependency injection 
        private readonly UserRepository _users;
        public AccountController(UserRepository users)
        {
            _users = users;
        }

        
        public IActionResult Register()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Register(User user)
        {
            //username exist or not checking
            if (_users.Exists(user.Username))
            {

                ViewBag.Error = "Username already exists";
                return View(user);
            }
            user.Role = "Teacher";
            //add user to memory list
            _users.Register(user);
            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            //user le pathako email ra password linxa and checks if it match or not
            var user = _users.ValidateUser(email, password);
            if(user == null)
            {
                ViewBag.Error = "Invalid email or password";
                return View();
            }
            //session allows us to temporarily store data
            //user valid xa vaney user info session ma store garxa
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("Email", user.Email);
            HttpContext.Session.SetString("Role", user.Role);


            if (user.Role == "Admin")
                return RedirectToAction("Dashboard", "Admin");
            else
                return RedirectToAction("Dashboard", "Teacher");
        }

        public IActionResult Logout()
        {
            
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
