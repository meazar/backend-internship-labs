using PetAdoptionSystemApi.Models;
using PetAdoptionSystemApi.Repositories.Interfaces;
using PetAdoptionSystemApi.Services.Interfaces;

namespace PetAdoptionSystemApi.Services.Implementation
{
    public class AdoptionService : IAdoptionService
    {
        private readonly IAdoptionRepository _adoptionRepo;
        private readonly IPetRepository _petRepo;
        private readonly IUserRepository _userRepo;
        public AdoptionService(IAdoptionRepository adoptionRepo, IPetRepository petRepo, IUserRepository userRepo)
        {
            _adoptionRepo = adoptionRepo;
            _petRepo = petRepo;
            _userRepo = userRepo;
        }
        public async Task<IEnumerable<Adoption>> GetAllAdoptionsAsync()
        {
            return await _adoptionRepo.GetAllAdoptionsAsync();
        }       
        public async Task<Adoption?> GetByIdAsync(int id)
        {
            return await _adoptionRepo.GetByIdAsync(id);
        }
        public async Task CreateAsync(Adoption adoption)
        {
            var pet = await _petRepo.GetByIdAsync(adoption.PetId);
            var user = await _userRepo.GetByIdAsync(adoption.UserId);
            if (pet == null)
            {
                throw new Exception("Pet not found");
            }
            if (user == null)
            {
                throw new Exception("User not found");
            }
            if (pet.IsAdopted)
            {
                throw new Exception("Pet is already adopted");
            }
            pet.IsAdopted = true;
            await _petRepo.UpdateAsync(pet);
            await _adoptionRepo.AddAsync(adoption);
        }


    }
}
