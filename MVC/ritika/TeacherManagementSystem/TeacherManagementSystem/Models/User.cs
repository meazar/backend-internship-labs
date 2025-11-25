using System.ComponentModel.DataAnnotations;

namespace TeacherPortalMvc.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }
        [Required(ErrorMessage ="Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
