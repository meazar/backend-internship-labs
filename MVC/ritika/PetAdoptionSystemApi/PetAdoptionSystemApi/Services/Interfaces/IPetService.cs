using PetAdoptionSystemApi.Models;

namespace PetAdoptionSystemApi.Services.Interfaces
{
    public interface IPetService
    {
        Task<IEnumerable<Pet>> GetAllPetsAsync();
        Task<Pet?> GetByIdAsync(int id);
        Task<IEnumerable<Pet>> GetAvailablePetsAsync();
        Task CreateAsync(Pet pet);
        Task UpdateAsync(Pet pet);
        Task DeleteAsync(int id);

    }
}
