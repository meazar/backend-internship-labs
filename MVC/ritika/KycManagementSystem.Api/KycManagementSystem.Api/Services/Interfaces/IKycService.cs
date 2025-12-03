using KycManagementSystem.Api.Models.DTOs.Kyc;
using KycManagementSystem.Api.Models.DTOs.Pagination;

namespace KycManagementSystem.Api.Services.Interfaces
{
    public interface IKycService
    {
        Task<int> CreateKycAsync(CreateKycDto dto);
        Task<KycProfileDto?> GetKycByIdAsync(int kycId);
        Task<PageResult<KycProfileDto>> GetPagedKycAsync(PageRequest pageRequest);
        Task UpdateKycAsync(int id, CreateKycDto dto);
        Task UpdateKycStatusAsync(int kycId, KycStatusUpdateDto dto);

    }
}
