using KycManagementSystem.Api.Models.Entities;

namespace KycManagementSystem.Api.Repositories.Interfaces
{
    public interface IKycHistoryRepository
    {
        Task AddHistoryAsync(KycHistory history);
        Task<IEnumerable<KycHistory>> GetHistoryByKycIdAsync(int kycId);
    }
}
