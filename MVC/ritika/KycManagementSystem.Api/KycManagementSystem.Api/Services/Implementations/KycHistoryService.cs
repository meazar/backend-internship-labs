using KycManagementSystem.Api.Models.Entities;
using KycManagementSystem.Api.Repositories.Interfaces;
using KycManagementSystem.Api.Services.Interfaces;

namespace KycManagementSystem.Api.Services.Implementations
{
    public class KycHistoryService : IKycHistoryService
    {
        private readonly IKycHistoryRepository _repo;

        public KycHistoryService(IKycHistoryRepository repo)
        {
            _repo = repo;
        }

        public async Task AddAsync(int kycId, string action, string? remarks, int? officerId)
        {
            var history = new KycHistory
            {
                KycId = kycId,
                Action = action,
                Remarks = remarks,
                OfficerId = officerId,
                ActionDate = DateTime.Now
            };

            await _repo.AddHistoryAsync(history);
        }

        public Task<IEnumerable<KycHistory>> GetByKycIdAsync(int kycId)
        {
            return _repo.GetHistoryByKycIdAsync(kycId);
        }
    }
}
