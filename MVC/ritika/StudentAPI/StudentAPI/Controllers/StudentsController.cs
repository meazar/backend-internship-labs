using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentAPI.Data;
using StudentAPI.Models;

namespace StudentAPI.Controllers
{
    //defines the base route for controller
    [Route("api/[controller]")]
    //enables automatic model validation, standardized error response
    [ApiController]
    //controllerBase : base class for API controllers 
    public class StudentsController : ControllerBase
    {
        //dependency injection: any service or object that a class needs to do its job from outside the class
        //_repo is private field that holds the data
        
        private readonly IStudentRepository _repo;
         public StudentsController(IStudentRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Student>> GetStudents()
        {
            //return http 200 status with data
            return Ok(_repo.GetAll());
        }
        [HttpGet("{id}")]
        public ActionResult<Student> GetStudent(int id)
        {
            //fetching a student id anusar
            var student =_repo.GetById(id);
            if(student == null)
            {
                //404 not found
                return NotFound();
            }
            return Ok(student);
        }

        [HttpPost]
        public ActionResult<Student>CreateStudent(Student student)
        {
            //sabai data annotations check garxa eg: email pass age phone
            if (!ModelState.IsValid)
            {
                //400 with validation error
                return BadRequest(ModelState);
            }
            //in memory list ma add garxa,
            _repo.Add(student);
            return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, student);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateStudent(int id, Student student)
        {
            if(id != student.Id)
            {
                return BadRequest("Id mismatch");

            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existing = _repo.GetById(id);
            if(existing== null)
            {
                return NotFound();
            }
            _repo.Update(student);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            var existing = _repo.GetById(id);
            if( existing== null)
            {
                return NotFound();
            }
            _repo.Delete(id);
            return NoContent(); 
        }
    }
}
