using PetAdoptionSystemApi.Models;
using PetAdoptionSystemApi.Repositories.Interfaces;
using PetAdoptionSystemApi.Services.Interfaces;
using PetAdoptionSystemApi.utils;

namespace PetAdoptionSystemApi.Services.Implementation.cs
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        public UserService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }
        public async Task<User> CreateAsync(User user, string password)
        {
            user.Password = PasswordHasher.Hash(password);
            await _userRepo.CreateAsync(user);
            return user;
        } 
       
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepo.GetAllUsersAsync();
        }
        public async Task<User?> GetByIdAsync(int id)
        {
            return await _userRepo.GetByIdAsync(id);
        }
      
        


    }
}
