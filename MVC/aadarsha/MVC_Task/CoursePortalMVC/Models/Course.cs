using System.ComponentModel.DataAnnotations; // for data annotations like [Key], [Required] - used for validation and defining primary keys

namespace CoursePortalMVC.Models
{
    // Model: represents the data structure for a Course entity in the application(domain model).
    public class Course
    {
        // Id is the primary key for the Course entity.
        public int Id { get; set; }

        [Required(ErrorMessage = "Course title is required.")] // Validation: Title is required.
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Course title must be between 2 and 100 characters long.")] // Validation: Title length constraints.
        public string? Title { get; set; }

        [Range(1, 10, ErrorMessage = "Credits must be between 1 and 10.")] // Validation: Credits must be within the specified range.
        public int Credits { get; set; }
    }
}