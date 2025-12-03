using PetAdoptionSystemApi.Data;
using PetAdoptionSystemApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using PetAdoptionSystemApi.Models;
namespace PetAdoptionSystemApi.Repositories.Implementations
{
    public class PetRepository: IPetRepository
    {
        private readonly ApplicationDbContext _db;
        public PetRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task AddAsync(Pet pet)
        {
            await _db.Pets.AddAsync(pet);
            await _db.SaveChangesAsync();
        }
        public async Task<IEnumerable<Pet>> GetAllPetsAsync()
        {
            return await _db.Pets.ToListAsync();
        }
        public async Task<Pet?> GetByIdAsync(int id)
        {
            return await _db.Pets.FindAsync(id);
        }
        public async Task<IEnumerable<Pet>> GetAvailablePetsAsync()
        {
            return await _db.Pets.Where(p => !p.IsAdopted).ToListAsync();
        }
        public async Task UpdateAsync(Pet pet)
        {
            _db.Pets.Update(pet);
            await _db.SaveChangesAsync();
        }
        public async Task DeleteAsync(int petId)
        {
            var pet = await _db.Pets.FindAsync(petId);
            if (pet != null)
            {
                _db.Pets.Remove(pet);
                await _db.SaveChangesAsync();
            }
        }
    }
}
