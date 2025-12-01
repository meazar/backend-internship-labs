using KycManagementSystem.Api.Models.Entities;
namespace KycManagementSystem.Api.Repositories.Interfaces
{
    public interface IOfacRepository
    {
        Task<bool> CheckAgainstOfacAsync(string fullName, string? nationality= null);
      
        Task<int> AddDummySanctionAsync(DummyOfacRecord sanction);
        Task<IEnumerable<DummyOfacRecord>> GetAllSanctionsAsync();

        Task<IEnumerable<DummyOfacRecord>> SearchSanctionsAsync(string name, string? nationality);
    }
}
