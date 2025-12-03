namespace PetAdoptionSystemApi.DTOs.User
{
    public class UserResponseDto
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Role { get; set; }
        public DateTime DateRegistered { get; set; }
    }
}
