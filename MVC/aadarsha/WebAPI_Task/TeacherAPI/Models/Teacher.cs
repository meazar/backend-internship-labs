using System.ComponentModel.DataAnnotations;

namespace TeacherAPI.Models
{
    public class Teacher
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string? FullName { get; set; }
        [Required]
        [StringLength(100)]
        public string? Subject { get; set; }
        [Range(0, 75, ErrorMessage = "Years of experience must be between 0 and 75.")]
        public int YearsOfExperience { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
    }
}
