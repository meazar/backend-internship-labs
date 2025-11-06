using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Controllers
{
    public class RoutineController : Controller
    {

        private static List<Routine> routines = new List<Routine>
        {
            new Routine {Id=1, Semester="3th",SubjectName="Math",Day="Tuesday",Time="8.30-9.30", TeacherName="SANTARAM"},
            new Routine {Id=2, Semester="4th",SubjectName="JAVA",Day="Wednesday",Time="8.30-9.30", TeacherName="SANTARAM"},
            new Routine {Id=3, Semester="5th",SubjectName="Web Application",Day="Friday",Time="8.30-9.30", TeacherName="SANTARAM"}
        };

        private bool IsLoggedIn() => HttpContext.Session.GetString("UserName") != null;
        private bool IsAdmin() => HttpContext.Session.GetString("Role") == "Admin";
        private bool IsStudent() => HttpContext.Session.GetString("Role") == "Student";

        private string CurrentUsername() => HttpContext.Session.GetString("UserName");
        public IActionResult Index()
        {

            if (!IsLoggedIn())
            {
                return RedirectToAction("Login","Account");

            }
            if (IsAdmin())
            {
                return View(routines);
            }
            if (IsStudent())
            {
                var student = AccountController.GetStudents().FirstOrDefault(s=>s.UserName==CurrentUsername());
                if(student == null)
                {
                    return NotFound();
                }
                var studentRoutine = routines.Where(r=>r.Semester== student.Semester).ToList();
                return View(studentRoutine);
            }
            return RedirectToAction("login","Account");
        }


        [HttpGet]

        public IActionResult Create()
        {
            if(!IsLoggedIn() || !IsAdmin())
            {
                return RedirectToAction("Login", "Account");

            }
            return View();
        }


        [HttpPost]
        public IActionResult Create(Routine routine)
        {
            if(!IsLoggedIn() || !IsAdmin())
            {
                return RedirectToAction("Login", "Account");

            }
            routine.Id = routines.Count > 0 ? routines.Max(r => r.Id) + 1 :1;
            routines.Add(routine);
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login","Account");

            }
            var routine = routines.FirstOrDefault(r=> r.Id == id);
            if(routine == null)
            {
                return NotFound();
            }
            return View(routine);
        }


        [HttpPost]
        public IActionResult Edit(Routine updatedRoutine)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");

            } 
                

            var routine = routines.FirstOrDefault(r => r.Id == updatedRoutine.Id);
            if (routine == null)
            {
                return NotFound();
            }
            routine.Semester = updatedRoutine.Semester;
            routine.Day = updatedRoutine.Day;
            routine.Time = updatedRoutine.Time;
            routine.SubjectName = updatedRoutine.SubjectName;
            routine.TeacherName = updatedRoutine.TeacherName;

            return RedirectToAction("Index");
        }


        public IActionResult Delete(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");


            }
            var routine = routines.FirstOrDefault(r => r.Id == id);
            if (routine != null)
            {
                routines.Remove(routine);

            }
                
            return RedirectToAction("Index");
        }
    }
}
