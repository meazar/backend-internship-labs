using PetAdoptionSystemApi.Models;

namespace PetAdoptionSystemApi.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetByIdAsync(int id);
        Task<User> CreateAsync(User user, string password);
       
    }
}
