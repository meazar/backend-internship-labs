using LoanManagementSystem.Models;

namespace LoanManagementSystem.IRepository
{
    public interface IUserRepository
    {
        Task<User?> GetUserByIdAsync(int userid);
        Task<User?> GetUserByEmailAsync(string email);

        Task<int> CreateUserAsync(CreateUserRequest user);

        Task<bool> UpdatedUserAsync(User user);

        Task<bool> UpdatedKYCStatusAsync(int userId, string status);

        Task<IEnumerable<User>> GetUserByRoleAsync(string role);





    }
}
