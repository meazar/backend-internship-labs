namespace Library.Core.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; } // No hashing
        public string? Role { get; set; }     // Admin/Librarian/Member
    }
}
