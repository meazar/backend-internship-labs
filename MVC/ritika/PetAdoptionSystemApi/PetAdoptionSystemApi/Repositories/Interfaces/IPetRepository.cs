using PetAdoptionSystemApi.Models;

namespace PetAdoptionSystemApi.Repositories.Interfaces
{
    public interface IPetRepository
    {
        Task<IEnumerable<Pet>> GetAllPetsAsync();
        Task<Pet?> GetByIdAsync(int petId);
        Task<IEnumerable<Pet>> GetAvailablePetsAsync();
        Task AddAsync(Pet pet);
        Task UpdateAsync(Pet pet);
        Task DeleteAsync(int petId);
        
    }
}
