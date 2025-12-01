using KycManagementSystem.Api.Models.Entities;
namespace KycManagementSystem.Api.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<int> CreateUserAsync(User user);
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByUsernameAsync(string username);

        Task<IEnumerable<User>> GetAllAsync();
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
    }
}
