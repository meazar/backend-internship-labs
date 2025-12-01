using KycManagementSystem.Api.Models.DTOs.Ofac;

namespace KycManagementSystem.Api.Services.Interfaces
{
    public interface IOfacService
    {
        Task<OfacCheckResponse> SearchAsync(string fullName, string? nationality = null);
        Task<IEnumerable<OfacMatchDto>> GetAllSanctionsAsync();
        Task<int> AddDummySanctionAsync(OfacMatchDto dto);
    }
}
