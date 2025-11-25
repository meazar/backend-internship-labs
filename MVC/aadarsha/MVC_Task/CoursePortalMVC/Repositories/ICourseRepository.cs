using CoursePortalMVC.Models; // for importing the Course model used in method signatures
using System.Collections.Generic; // for using IEnumerable<T> used in method signatures and collections

namespace CoursePortalMVC.Repositories
{
    // Repository interface abstracts data access operations for Course entities.
    public interface ICourseRepository
    {
        List<Course> GetAllCourses(); // Retrieve all courses
        Course? GetCourseById(int id); // Retrieve a course by its ID
        void AddCourse(Course course); // Add a new course
        void UpdateCourse(Course course); // Update an existing course
        void DeleteCourse(int id); // Delete a course by its ID
        int GetNextId(); // Get the next available ID for a new course
    }
}