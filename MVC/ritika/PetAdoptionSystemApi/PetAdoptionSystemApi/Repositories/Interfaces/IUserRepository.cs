using PetAdoptionSystemApi.Models;

namespace PetAdoptionSystemApi.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetByIdAsync(int userId);
      
        Task CreateAsync(User user);
    }
}
