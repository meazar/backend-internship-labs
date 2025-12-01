using CustomerSupport.API.Models;

namespace CustomerSupport.API.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserById(int id);
    Task<User?> GetUserByEmail(string email);
    Task<User> CreateUser(User user);
    Task<User?> DeleteUser(int id);
    Task<IEnumerable<User?>> GetAllUsers();
}
