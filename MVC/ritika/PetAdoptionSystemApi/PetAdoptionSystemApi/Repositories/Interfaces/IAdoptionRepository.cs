using PetAdoptionSystemApi.Models;

namespace PetAdoptionSystemApi.Repositories.Interfaces
{
    public interface IAdoptionRepository
    {
        Task<IEnumerable<Adoption>> GetAllAdoptionsAsync();
        Task<Adoption?> GetByIdAsync(int id);
        Task AddAsync(Adoption adoption);
    }
}
