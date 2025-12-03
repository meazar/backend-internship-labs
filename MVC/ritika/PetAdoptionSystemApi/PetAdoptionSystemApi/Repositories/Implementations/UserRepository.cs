using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using PetAdoptionSystemApi.Data;
using PetAdoptionSystemApi.Models;
using PetAdoptionSystemApi.Repositories.Interfaces;

namespace PetAdoptionSystemApi.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        public UserRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _db.Users.ToListAsync();
        }
        public async Task<User?> GetByIdAsync(int id)
        {
            return await _db.Users.FindAsync(id);
        }
       
       
        public async Task CreateAsync(User user)
        {
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
        }
    }
}
