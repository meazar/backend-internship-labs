using PetAdoptionSystemApi.Models;

namespace PetAdoptionSystemApi.DTOs.Auth
{
    public class RegisterDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }
}
