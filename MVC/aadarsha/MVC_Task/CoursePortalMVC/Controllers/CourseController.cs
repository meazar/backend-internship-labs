using Microsoft.AspNetCore.Mvc; // for Controller base class and IActionResult return type
using CoursePortalMVC.Models; // for Course model
using CoursePortalMVC.Repositories; // for ICourseRepository interface
using System.ComponentModel.Design; // for design-time attributes and classes 

namespace CoursePortalMVC.Controllers
{
    // Controller: handles HTTP requests related to Course entities.
    // uses ICourseRepository injected via Dependency Injection Container for data access.
    public class CourseController : Controller // Inherits from Controller base class from ASP.NET Core MVC
    {
        // private field to hold the injected repository instance and readonly to ensure it is assigned only once in the constructor.
        private readonly ICourseRepository _repo;

        // Constructor: receives ICourseRepository instance via Dependency Injection.
        public CourseController(ICourseRepository repo)
        {
            _repo = repo; // Dependency Injection: assign the injected repository to the private field.
        }

        // Action method GET: /Course/Index which handles GET requests to list all courses.
        //  IActionResult: represents the result of an action method. 
        public IActionResult Index() // Index action returns a view with the list of courses.
        {
            if(HttpContext.Session.GetString("IsAdminLoggedIn") != "true")
            {
                return RedirectToAction("Login", "User"); // Redirect to Login if not logged in as admin.
            }
            var courses = _repo.GetAllCourses(); // Retrieve all courses from the repository.
            return View(courses); // Return the view with the model:(List<Course>) containing all courses.
        }

        // GET: /Course/Details/{id}
        public IActionResult Details(int id) // Details action returns a view with details of a specific course identified by its ID.
        {
            var course = _repo.GetCourseById(id);
            if (course == null)
            {
                return NotFound(); // Response handling: Return 404 Not Found if the course does not exist.
            }
            return View(course);
        }

        // GET: /Course/Create to show the create form.
        public IActionResult Create() // Create action to display the create form for a new course.
        {
            return View(); // Return the create view.
        }

        // POST: /Course/Create
        [HttpPost] // Specifies that this action only handles HTTP POST requests.
        [ValidateAntiForgeryToken] // Security: protects against CSRF attacks.
        public IActionResult Create(Course course) // Create action to add a new course.
        {
            // Model binding and validation: checks if the incoming model is valid based on data annotations in the Course model.
            if (ModelState.IsValid) // Server-side validation check.
            {
                _repo.AddCourse(course); // Add the new course to the repository
                return RedirectToAction(nameof(Index)); // Redirect to Index action after successful creation(POST) (PRG pattern). PRG prevents duplicate form submissions. 
            }
            // If model state is invalid, return the same view with validation messages.
            return View(course);
        }

        // GET: /Course/Edit/{id} to show the edit form.
        public IActionResult Edit(int id) // Edit action to display the edit form for a specific course.
        {
            var course = _repo.GetCourseById(id);
            return (course == null) ? NotFound() : View(course); // Return 404 if not found, else return the edit view with the course model.
        }

        // POST: /Course/Edit/{id} to submit the edited course data.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Course course) // Edit action to update an existing course.
        {
            if (id != course.Id)
            {
                return BadRequest(); // Response handling: Return 400 Bad Request if the ID in the URL does not match the course ID.
            }
            if (ModelState.IsValid)
            {
                _repo.UpdateCourse(course); // Update the course in the repository.
                return RedirectToAction(nameof(Index)); // Redirect to Index action after successful update. 
            }
            return View(course); // If model state is invalid, return the same view with validation messages.
        }


        // GET: /Course/Delete/{id} to show the delete confirmation page.
        public IActionResult Delete(int id) // Delete action to display the delete confirmation for a specific course.
        {
            var course = _repo.GetCourseById(id);
            if (course == null)
            {
                return NotFound(); // Return 404 if the course does not exist.
            }
            return View(course); // Return the delete confirmation view with the course model.
        }

        // POST: /Course/Delete/{id} to confirm deletion.
        // Use POST to actually perform the delete and (CSRF protection) using [ValidateAntiForgeryToken].
        // Use ActionName to differentiate between GET and POST Delete actions.
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id) // DeleteConfirmed action to delete a specific course.
        {
            _repo.DeleteCourse(id); // Delete the course from the repository.
            return RedirectToAction(nameof(Index)); // Redirect to Index action after successful deletion where Index action shows the updated list of courses.
        }
    }
}
