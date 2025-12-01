using KycManagementSystem.Api.Models.DTOs.Documents;
using KycManagementSystem.Api.Models.Entities;
namespace KycManagementSystem.Api.Services.Interfaces
{
    public interface IDocumentService
    {
        Task<int> UploadDocumentAsync(DocumentUploadDto dto);
        Task<IEnumerable<KycDocument>> GetDocumentsByKycIdAsync(int KycId);
        Task UpdateDocumentAsync(int id, IFormFile file);
        Task DeleteDocumentAsync(int id);
    }
}
