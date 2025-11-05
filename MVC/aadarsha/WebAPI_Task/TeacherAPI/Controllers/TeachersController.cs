using Microsoft.AspNetCore.Mvc;
using TeacherAPI.Models;
using TeacherAPI.Repositories;

namespace TeacherAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeachersController : ControllerBase
    {
        private readonly InMemoryTeacherRepository _repository;

        public TeachersController(InMemoryTeacherRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult GetAllTeachers() => Ok(_repository.GetTeachersAll());

        [HttpGet("{id}")]
        public IActionResult GetTeacherById(int id)
        {
            var teacher = _repository.GetTeacherById(id);
            if (teacher == null)
                return NotFound();
            return Ok(teacher);
        }

        [HttpPost]
        public IActionResult AddTeacher(Teacher teacher)
        {
            var createdTeacher = _repository.AddTeacher(teacher);
            return CreatedAtAction(nameof(GetTeacherById), new { id = createdTeacher.Id }, createdTeacher);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTeacher(int id, Teacher teacher)
        {
            var updatedTeacher = _repository.UpdateTeacher(id, teacher);
            return updatedTeacher ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTeacher(int id)
        {
            var deletedTeacher = _repository.DeleteTeacher(id);
            return deletedTeacher ? NoContent() : NotFound();
        }
    }
}