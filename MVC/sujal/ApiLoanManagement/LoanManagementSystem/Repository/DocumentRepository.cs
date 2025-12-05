using LoanManagementSystem.Database;
using LoanManagementSystem.IRepository;
using LoanManagementSystem.Models;
using Microsoft.Data.SqlClient;

namespace LoanManagementSystem.Repository
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly IDatabaseService _databaseService;
        public DocumentRepository(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<int> UploadDocumentAsync(Document document)
        {
            const string sql = @"
                INSERT INTO Documents (ApplicationId, UserId, DocumentType, FileName, FileUrl, UploadedAt)
                OUTPUT INSERTED.DocumentId
                VALUES (@ApplicationId, @UserId, @DocumentType, @FileName, @FileUrl, @UploadedAt)";

            var documentId = await _databaseService.ExecuteScalarAsync(sql,
                new SqlParameter("@ApplicationId", document.ApplicationId),
                new SqlParameter("@UserId", document.UserId),
                new SqlParameter("@DocumentType", document.DocumentType),
                new SqlParameter("@FileName", document.FileName),
                new SqlParameter("@FileUrl", document.FileUrl),
                new SqlParameter("@UploadedAt", document.UploadedAt));

            return documentId != null ? Convert.ToInt32(documentId) : 0;
        }

        public async Task<Document?> GetDocumentByIdAsync(int documentId)
        {
            const string sql = @"
                SELECT DocumentId, ApplicationId, UserId, DocumentType, FileName, FileUrl, UploadedAt
                FROM Documents WHERE DocumentId = @DocumentId";

            using var reader = await _databaseService.ExecuteReaderAsync(sql,
                new SqlParameter("@DocumentId", documentId));

            if (await reader.ReadAsync())
            {
                return new Document
                {
                    DocumentId = reader.GetInt32(0),
                    ApplicationId = reader.GetInt32(1),
                    UserId = reader.GetInt32(2),
                    DocumentType = reader.GetString(3),
                    FileName = reader.GetString(4),
                    FileUrl = reader.GetString(5),
                    UploadedAt = reader.GetDateTime(6)
                };
            }

            return null;
        }

        public async Task<IEnumerable<Document>> GetDocumentsByApplicationAsync(int applicationId)
        {
            const string sql = @"
                SELECT DocumentId, ApplicationId, UserId, DocumentType, FileName, FileUrl, UploadedAt
                FROM Documents WHERE ApplicationId = @ApplicationId
                ORDER BY UploadedAt DESC";

            var documents = new List<Document>();
            using var reader = await _databaseService.ExecuteReaderAsync(sql,
                new SqlParameter("@ApplicationId", applicationId));

            while (await reader.ReadAsync())
            {
                documents.Add(new Document
                {
                    DocumentId = reader.GetInt32(0),
                    ApplicationId = reader.GetInt32(1),
                    UserId = reader.GetInt32(2),
                    DocumentType = reader.GetString(3),
                    FileName = reader.GetString(4),
                    FileUrl = reader.GetString(5),
                    UploadedAt = reader.GetDateTime(6)
                });
            }

            return documents;
        }

        public async Task<IEnumerable<Document>> GetDocumentsByUserAsync(int userId)
        {
            const string sql = @"
                SELECT DocumentId, ApplicationId, UserId, DocumentType, FileName, FileUrl, UploadedAt
                FROM Documents WHERE UserId = @UserId
                ORDER BY UploadedAt DESC";

            var documents = new List<Document>();
            using var reader = await _databaseService.ExecuteReaderAsync(sql,
                new SqlParameter("@UserId", userId));

            while (await reader.ReadAsync())
            {
                documents.Add(new Document
                {
                    DocumentId = reader.GetInt32(0),
                    ApplicationId = reader.GetInt32(1),
                    UserId = reader.GetInt32(2),
                    DocumentType = reader.GetString(3),
                    FileName = reader.GetString(4),
                    FileUrl = reader.GetString(5),
                    UploadedAt = reader.GetDateTime(6)
                });
            }

            return documents;
        }

        public async Task<bool> DeleteDocumentAsync(int documentId)
        {
            const string sql = "DELETE FROM Documents WHERE DocumentId = @DocumentId";

            var rowsAffected = await _databaseService.ExecuteNonQueryAsync(sql,
                new SqlParameter("@DocumentId", documentId));

            return rowsAffected > 0;
        }

        public async Task<bool> UpdateDocumentAsync(Document document)
        {
            const string sql = @"
                UPDATE Documents 
                SET DocumentType = @DocumentType, FileName = @FileName, FileUrl = @FileUrl
                WHERE DocumentId = @DocumentId";

            var rowsAffected = await _databaseService.ExecuteNonQueryAsync(sql,
                new SqlParameter("@DocumentType", document.DocumentType),
                new SqlParameter("@FileName", document.FileName),
                new SqlParameter("@FileUrl", document.FileUrl),
                new SqlParameter("@DocumentId", document.DocumentId));

            return rowsAffected > 0;
        }

    }
}
