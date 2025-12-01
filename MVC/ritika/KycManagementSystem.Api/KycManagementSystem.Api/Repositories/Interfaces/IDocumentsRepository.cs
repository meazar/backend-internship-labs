using KycManagementSystem.Api.Models.DTOs.Documents;
using KycManagementSystem.Api.Models.Entities;

namespace KycManagementSystem.Api.Repositories.Interfaces
{
    public interface IDocumentsRepository
    {
        Task<int> UploadDocumentAsync(KycDocument document);
        Task<IEnumerable<KycDocument>> GetDocumentsByKycIdAsync(int KycId);
        Task UpdateDocumentAsync(int id, IFormFile file);
        Task DeleteDocumentAsync(int kycId);
    }
}
