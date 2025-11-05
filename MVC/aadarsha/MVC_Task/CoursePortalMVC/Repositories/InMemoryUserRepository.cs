using System.Reflection.Metadata;
using CoursePortalMVC.Models;

namespace CoursePortalMVC.Repositories
{
    public class InMemoryUserRepository : IUserRepository
    { 
        private readonly List<User> _users = new()
        {
            new User { Email = "admin@portal.com", Password = "admin123" } // Hardcoded admin only user.
        };

        public User? ValidateUser(string email, string password)
        {
            return _users.FirstOrDefault(u => u.Email == email && u.Password == password); // Check if user with given email and password exists.
        }

        // public User? GetUserByEmail(string email)
        // {
        //     return _users.FirstOrDefault(u => u.Email == email); // Retrieve user by email.
        // }
    }
}