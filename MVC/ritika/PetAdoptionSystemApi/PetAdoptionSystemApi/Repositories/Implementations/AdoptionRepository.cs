using Microsoft.EntityFrameworkCore;
using PetAdoptionSystemApi.Data;
using PetAdoptionSystemApi.Models;
using PetAdoptionSystemApi.Repositories.Interfaces;

namespace PetAdoptionSystemApi.Repositories.Implementations
{
    public class AdoptionRepository : IAdoptionRepository
    {
        private readonly ApplicationDbContext _db;
        public AdoptionRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task AddAsync(Adoption adoption)
        {
            await _db.Adoptions.AddAsync(adoption);
            await _db.SaveChangesAsync();
        }
        public async Task<IEnumerable<Adoption>> GetAllAdoptionsAsync()
        {
            return await _db.Adoptions.ToListAsync();
        }
        public async Task<Adoption?> GetByIdAsync(int id)
        {
            return await _db.Adoptions
                .Include(a => a.Pet)
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.AdoptionId == id);
        }
    }
}
