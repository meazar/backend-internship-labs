using Microsoft.AspNetCore.Mvc;
using TeacherAPI.Models;
using TeacherAPI.Repositories;
namespace TeacherAPI.Controllers
{
    [ApiController]
    [Route("api")]
    public class TeachersController : ControllerBase
    {
        private readonly InMemoryTeacherRepository _repository;

        public TeachersController(InMemoryTeacherRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("teachers")]
        public IActionResult GetAllTeachers() => Ok(_repository.GetTeachersAll());

        [HttpGet("teacher/{id}")]
        public IActionResult GetTeacherById(int id)
        {
            var teacher = _repository.GetTeacherById(id);
            if (teacher == null)
                return NotFound();
            return Ok(teacher);
        }

        [HttpPost("teacher")]
        public IActionResult AddTeacher(Teacher teacher)
        {
            if (teacher == null)
                return BadRequest(new { message = "Invalid JSON format. Please check your request body." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdTeacher = _repository.AddTeacher(teacher);
            return CreatedAtAction(nameof(GetTeacherById), new { id = createdTeacher.Id }, createdTeacher);
        }

        [HttpPut("teacher/{id}")]
        public IActionResult UpdateTeacher(int id, Teacher teacher)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var updatedTeacher = _repository.UpdateTeacher(id, teacher);
            return updatedTeacher ? NoContent() : NotFound();
        }

        [HttpDelete("teacher/{id}")]
        public IActionResult DeleteTeacher(int id)
        {
            var deletedTeacher = _repository.DeleteTeacher(id);
            return deletedTeacher ? NoContent() : NotFound();
        }
    }
}
