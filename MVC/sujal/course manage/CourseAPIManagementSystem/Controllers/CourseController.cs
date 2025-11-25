using CourseAPIManagementSystem.Model;
using CourseAPIManagementSystem.Repo;
using Microsoft.AspNetCore.Mvc;

namespace CourseAPIManagementSystem.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly CourseRepo _courseRepo;

        public CourseController(CourseRepo courseRepo)
        {
            _courseRepo = courseRepo;
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] string? search)
        {
            var courses = string.IsNullOrWhiteSpace(search) ? _courseRepo.GetAll() : _courseRepo.SearchByName(search);

            return Ok(courses);
        }


        [HttpGet("{id}")]

        public IActionResult GetById(int id)
        {
            var course = _courseRepo.GetbyId(id);
            if (course == null)
            {
                return NotFound();
            }
            return Ok(course);

        }


        [HttpPost]
        public IActionResult Create([FromBody] Course course)
        {
            if (string.IsNullOrWhiteSpace(course.CourseName))
            {
                return BadRequest("Course name is Required");

            }
            _courseRepo.Add(course);
            return CreatedAtAction(nameof(GetById), new { id = course.Id }, course);
        }

        [HttpPut("{id}")]

        public IActionResult Update(int id, [FromBody]Course course)
        {
            var existing = _courseRepo.GetbyId(id);
            if (existing == null) 
            {
                return NotFound();

            }
            course.Id = id;
            _courseRepo.Update(course);
            return NoContent();        

        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existing = _courseRepo.GetbyId(id);
            if(existing == null)
            {
                return NotFound();
            }
            _courseRepo.Delete(id);
            return NoContent();
        }
    }
}
