using Microsoft.AspNetCore.Mvc;
using TeacherPortalMVC.Data;
using TeacherPortalMvc.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace TeacherPortalMvc.Controllers
{
    public class TeacherController : Controller
    {
        //noterepository is injected so teacher can access note data
        private readonly NoteRepository _notes;
        //this give access to server root folder
        private readonly IWebHostEnvironment _env;

        public TeacherController(NoteRepository notes, IWebHostEnvironment env)
        {
            _notes = notes;
            _env = env;
        }

        public IActionResult Dashboard()
        {
            var username = HttpContext.Session.GetString("Username");
            ViewBag.TeacherName = username;
            return View();
        }

        public IActionResult Notes()
        {
            var notes = _notes.GetByTeacher(1);
            return View(notes);
        }

        public IActionResult AddNote()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddNote(Note note)
        {
            if (note.File != null && note.File.Length > 0)
            {
                //uploads ma create hunxa 
                var uploadPath = Path.Combine(_env.WebRootPath, "uploads");
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                //upoloads ma save hunxa wwwroot ma,guid le unique name genreate garxa
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(note.File.FileName);
                var filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    note.File.CopyTo(stream);
                }

                // Store filename
                note.FileName = fileName;
            }

            note.TeacherId = 1;
            note.UploadedAt = DateTime.Now;
            _notes.Add(note);

            return RedirectToAction("Notes");
        }

        public IActionResult Edit(int id)
        {
            var note = _notes.GetById(id);
            if (note == null) return NotFound();
            return View(note);
        }

        [HttpPost]
        public IActionResult Edit(Note note)
        {
            if (note.File != null && note.File.Length > 0)
            {
                var uploadPath = Path.Combine(_env.WebRootPath, "uploads");
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(note.File.FileName);
                var filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    note.File.CopyTo(stream);
                }

                note.FileName = fileName;
            }

            _notes.Update(note);
            return RedirectToAction("Notes");
        }

        public IActionResult Delete(int id)
        {
            _notes.Delete(id);
            return RedirectToAction("Notes");
        }
    }
}
