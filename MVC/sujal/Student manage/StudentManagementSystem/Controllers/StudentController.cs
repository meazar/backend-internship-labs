using Microsoft.AspNetCore.Mvc;
using StudentManagementSystem.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace StudentManagementSystem.Controllers
{
    public class StudentController : Controller
    {



        private static List<Student> students = AccountController.GetStudents();



        private bool IsLoggedIn() => HttpContext.Session.GetString("UserName") != null;
        private bool IsAdmin() => HttpContext.Session.GetString("Role") == "Admin";

        private string CurrentUsername() => HttpContext.Session.GetString("UserName");

        /* private bool IsAdmin()
         {
             return HttpContext.Session.GetString("Role") == "Admin";
         }*/

        [HttpGet]
        public IActionResult Index()
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Account");

            }
            if (IsAdmin())
            {
                return View(students);
            }
            else
            {
                return RedirectToAction("MyProfile");
            }



        }


        [HttpGet]
        public IActionResult Create()
        {

            if (!IsLoggedIn() || !IsAdmin())
            {
                return RedirectToAction("Login", "Account");

            }
            return View();
        }

        [HttpPost]
        public IActionResult Create(Student student)
        {

            if (!IsLoggedIn() || !IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                student.Id = students.Max(s => s.Id) + 1;
                //students.Count + 1; 

                students.Add(student);
                return RedirectToAction("Index", "Student");

            }
            return View(student);
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }
            
            var student = students.FirstOrDefault(s => s.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        [HttpPost]
        public IActionResult Edit(Student updatedStudent)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }

            //if (!IsAdmin())
            //{
            //    return RedirectToAction("Index", "Student");
            //}

            var student = students.FirstOrDefault(s => s.Id == updatedStudent.Id);
            if (student == null)
            {
                return NotFound();
            }
            student.UserName = updatedStudent.UserName;

            student.Name = updatedStudent.Name;
            student.age = updatedStudent.age;
            student.Address = updatedStudent.Address;
            student.Semester = updatedStudent.Semester;

            student.Password = updatedStudent.Password;

            return RedirectToAction("Index", "Student");


        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (!IsLoggedIn() || !IsAdmin())
            {
                return RedirectToAction("Login", "Account");
            }
            //if (!IsAdmin())
            //{
            //    return RedirectToAction("Index", "Student");
            //}
            
            var student = students.FirstOrDefault(s => s.Id == id);
            if(student != null)
            {
                students.Remove(student);
            }
            
            return RedirectToAction("Index", "Student");
        }


        public IActionResult MyProfile()
        {
            if (!IsLoggedIn())
            {
                return RedirectToAction("Login", "Account");

            }
            var username = CurrentUsername();
            var student = students.FirstOrDefault(s => s.UserName == username);

            if(student == null)
            {
                return NotFound();
            }
            return View(student);
        }

    }
}
