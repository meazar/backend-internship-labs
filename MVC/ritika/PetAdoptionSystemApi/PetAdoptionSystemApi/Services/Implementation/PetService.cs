using PetAdoptionSystemApi.Models;
using PetAdoptionSystemApi.Repositories.Interfaces;
using PetAdoptionSystemApi.Services.Interfaces;

namespace PetAdoptionSystemApi.Services.Implementation
{
    public class PetService : IPetService
    {
        private readonly IPetRepository _petRepo;
        public PetService(IPetRepository petRepo)
        {
            _petRepo = petRepo;
        }
        public async Task<IEnumerable<Pet>> GetAllPetsAsync()
        {
            return await _petRepo.GetAllPetsAsync();
        }
        public async Task<Pet?> GetByIdAsync(int id)
        {
            return await _petRepo.GetByIdAsync(id);
        }
        public async Task<IEnumerable<Pet>> GetAvailablePetsAsync()
        {
            return await _petRepo.GetAvailablePetsAsync();
        }
        public async Task CreateAsync(Pet pet)
        {
            await _petRepo.AddAsync(pet);
           
        }
        public async Task UpdateAsync(Pet pet)
        {
            await _petRepo.UpdateAsync(pet);
        }
        public async Task DeleteAsync(int id)
        {
            var pet = await _petRepo.GetByIdAsync(id);
            if (pet != null)
                await _petRepo.DeleteAsync(id);
        }

    }
}
