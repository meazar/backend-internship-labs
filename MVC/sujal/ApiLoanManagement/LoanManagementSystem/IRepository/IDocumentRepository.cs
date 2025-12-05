using LoanManagementSystem.Models;

namespace LoanManagementSystem.IRepository
{
    public interface IDocumentRepository
    {
        Task<int> UploadDocumentAsync(Document document);
        Task<Document?> GetDocumentByIdAsync(int documentId);
        Task<IEnumerable<Document>> GetDocumentsByApplicationAsync(int applicationId);
        Task<IEnumerable<Document>> GetDocumentsByUserAsync(int userId);
        Task<bool> DeleteDocumentAsync(int documentId);
        Task<bool> UpdateDocumentAsync(Document document);
    }
}
