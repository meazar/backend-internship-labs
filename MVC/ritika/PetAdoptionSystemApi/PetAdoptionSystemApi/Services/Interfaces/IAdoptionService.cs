using PetAdoptionSystemApi.Models;

namespace PetAdoptionSystemApi.Services.Interfaces
{
    public interface IAdoptionService
    {
        Task<IEnumerable<Adoption>> GetAllAdoptionsAsync();
        Task<Adoption?> GetByIdAsync(int id);
        Task CreateAsync(Adoption adoption);
    }
}
