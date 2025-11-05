using System.ComponentModel.DataAnnotations; // for Data Annotations which is used for validation of model properties

namespace CoursePortalMVC.Models
{
    public class User
    {
        [Required(ErrorMessage = "Email is required")] // Ensures that the Email field is not left empty
        [EmailAddress(ErrorMessage = "Invalid Email Address")] // Validates that the input is in the correct email format
        public string Email { get; set; } = string.Empty; // Property to store user's email allowed to be empty string by default

        [Required(ErrorMessage = "Password is required")] // Ensures that the Password field is not left empty
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")] // Validates that the password has a minimum length of 6 characters
        public string Password { get; set; } = string.Empty; 
    }
}