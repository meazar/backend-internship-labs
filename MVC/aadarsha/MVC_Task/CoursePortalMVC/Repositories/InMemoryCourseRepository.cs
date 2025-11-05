using System.Collections.Generic; // for using collections like List<T> to store courses
using System.Linq; // for LINQ operations like FirstOrDefault
using System.Security.Cryptography.X509Certificates;
using CoursePortalMVC.Models; // for importing the Course model

namespace CoursePortalMVC.Repositories
{
    // In-memory implementation of the ICourseRepository interface.
    // This class provides CRUD operations for Course entities stored in memory.
    // Not thread-safe; suitable for development/testing purposes.

    public class InMemoryCourseRepository : ICourseRepository
    {
        private readonly List<Course> _courses; // Internal list to store courses in memory

        public InMemoryCourseRepository() // Constructor initializes the in-memory course list with sample data.
        {
            _courses = new List<Course> // Initialize the in-memory list with some sample courses.
            {
                new Course { Id = 1, Title = "Introduction to Programming with C++", Credits = 3 },
                new Course { Id = 2, Title = "Data Structures", Credits = 4 },
                new Course { Id = 3, Title = "Database Systems", Credits = 3 },
                new Course { Id = 4, Title = "Operating Systems", Credits = 7 }
            };
        }

        // Adds a new course to the repository.
        public void AddCourse(Course course)
        {
            course.Id = GetNextId(); // Assign a new ID to the course.
            _courses.Add(course); // Add the course to the in-memory list.
        }

        // Deletes a course by its ID.
        public void DeleteCourse(int id)
        {
            var existingCourse = GetCourseById(id); // Find the course by ID from the in-memory list.
            if (existingCourse != null)
            {
                _courses.Remove(existingCourse); // Remove the course if found.
            }
        }

        // Retrieves all courses from the repository.
        public List<Course> GetAllCourses() => _courses.ToList(); // Return a copy of the in-memory course list in a new list.

        // Retrieves a course by its ID.
        public Course? GetCourseById(int id) => _courses.FirstOrDefault(c => c.Id == id); // Find and return the course with the specified ID.

        // Updates an existing course.
        public void UpdateCourse(Course course)
        {
            var existingCourse = GetCourseById(course.Id); // Find the existing course by ID.
            if (existingCourse != null)
            {
                existingCourse.Title = course.Title; // Update the title of the course.
                existingCourse.Credits = course.Credits; // Update the credits of the course
            }
        }

        // Gets the next available ID for a new course.
        public int GetNextId()
        {
            return _courses.Any() ? _courses.Max(c => c.Id) + 1 : 1; // Returns 1 if the list is empty; otherwise, returns max ID + 1.
        }
    }
}



