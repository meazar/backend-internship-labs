using Microsoft.AspNetCore.Identity;
using System.Linq;
using TeacherPortalMvc.Models;
namespace TeacherPortalMvc.Data
{
    public class UserRepository
    {
        //_users acts as in-memoey database
        private static List<User> _users = new()
        {
            new User {Id = 1, Username="admin", Email= "admin@gmail.com", Password= "admin123",Role= "Admin"},
            new User {Id= 2, Username="Ram Prasad",Email="ram@gmail.com",Password ="r@mprasad", Role="Teacher"}
        };

        //_nextId use to assign Ids to new users
        private static int _nextId = 3;

        //validate vako xa ki xaina checking
        public User ValidateUser(string email, string password)
        {
            return _users.FirstOrDefault(u => u.Email == email && u.Password == password);
        }

        public void Register(User user)
        {
            user.Id = _nextId++;
            _users.Add(user);
        }
        //username already exist or not checking to prevent 
        public bool Exists(string username) =>_users.Any(u=> u.Username == username);
    }
}
