using System.ComponentModel.DataAnnotations;

namespace StudentAPI.Models
{
    public class Student
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Name is required")]
        [StringLength(50,MinimumLength = 3, ErrorMessage = "Name should be between 3 to 50 characters")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }
        [Required(ErrorMessage ="Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email adress")]
        public string Email { get; set; }= string.Empty;
        [Required(ErrorMessage ="Phome number is required")]
        public string Phone { get; set; }
        [Required(ErrorMessage ="Age is required")]
        [Range(5, 100, ErrorMessage = "Age must be between 5 and 100")]
        public int Age { get; set; }

    }
}
