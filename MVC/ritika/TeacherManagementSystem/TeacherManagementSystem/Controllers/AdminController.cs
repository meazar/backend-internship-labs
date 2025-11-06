using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TeacherPortalMVC.Data;
using TeacherPortalMvc.Models;

namespace TeacherPortalMvc.Controllers
{
    public class AdminController : Controller
    {
        //dependency injection to inject the teacherRepository
        private readonly TeacherRepository _repo;

        public AdminController(TeacherRepository repo)
        {
            _repo = repo;
        }
        public IActionResult Dashboard()
        {
            var teachers = _repo.GetAll();
            return View(teachers);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Teacher t)
        {
            if (ModelState.IsValid)
            {
                _repo.Add(t);
                return RedirectToAction("Dashboard");
            }
            return View(t);
        }
        public IActionResult Edit(int id)
        {
            var t = _repo.GetById(id);
            return View(t);
        }
       

        [HttpPost]
        public IActionResult Edit(Teacher t)
        {
            if (ModelState.IsValid)
            {
                _repo.Update(t);
                return RedirectToAction("Dashboard");
            }
            return View(t);
        }


        public IActionResult Delete(int id)
        {
            _repo.Delete(id);
            return RedirectToAction("Dashboard");
        }


       
    }
}
