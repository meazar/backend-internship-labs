using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Controllers
{
    public class AccountController : Controller
    {

        private static List<Student> students = new List<Student>
        {
            new Student{Id=1,UserName = "student1",Password="student123",Name="Rojan Maharjan",age="24",Address="kantipur",Semester="5th", Role="Student"},

            new Student{Id=2,UserName = "student2",Password="student123",Name="kesab Gubta",age="34",Address="pokhara",Semester="4th", Role="Student"}

        };


        private static string adminUserName =  "admin";

        private static string adminPassword = "admin123";

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]

        public IActionResult Login(string username, string password)
        {

            // for Admin login
            if(username == adminUserName && password == adminPassword)
            {
                HttpContext.Session.SetString("UserName", adminUserName);
                HttpContext.Session.SetString("Role", "Admin");

                return RedirectToAction("Index", "Student");

            }


            //for Student Login 
            var student = students.FirstOrDefault(s => s.UserName == username && s.Password == password);
            if(student != null)
            {
                HttpContext.Session.SetString("UserName", student.UserName);
                HttpContext.Session.SetString("Role", "Student");
                return RedirectToAction("MyProfile", "Student");
            }

            ViewBag.Error = "InValid username or Password";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public IActionResult Profile()
        {
            var username = HttpContext.Session.GetString("username");
            var role = HttpContext.Session.GetString("Role");


            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login");
            }

            ViewBag.Username = username;
            ViewBag.Role = role;
            return View();
        }

        public static List<Student> GetStudents() => students;
    }
}
