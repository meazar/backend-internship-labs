using KycManagementSystem.Api.Models.DTOs.Kyc;
using KycManagementSystem.Api.Models.DTOs.Pagination;

namespace KycManagementSystem.Api.Repositories.Interfaces
{
    public interface IKycRepository
    {
        Task<int> CreateKycAsync(KycProfileDto kycProfile);
        Task<KycProfileDto> GetByIdAsync(int id);
        Task<IEnumerable<KycProfileDto>> GetPagedAsync(PageRequest pageRequest);
        Task UpdateKycAsync(KycProfileDto kycProfile);
        Task UpdateStatusAsync(int kycId, string status, int? officerId, string? remarks);
        Task<int> CountAsync();

    }
}
