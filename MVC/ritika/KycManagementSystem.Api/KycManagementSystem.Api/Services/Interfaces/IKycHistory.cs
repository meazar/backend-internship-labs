using KycManagementSystem.Api.Models.Entities;

namespace KycManagementSystem.Api.Services.Interfaces
{
    public interface IKycHistoryService
    {
        Task AddAsync(int kycId, string action, string? remarks, int? officerId);
        Task<IEnumerable<KycHistory>> GetByKycIdAsync(int kycId);
    }
}
