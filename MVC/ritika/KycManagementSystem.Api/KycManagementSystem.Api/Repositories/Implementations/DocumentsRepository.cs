using Dapper;
using KycManagementSystem.Api.Database;
using KycManagementSystem.Api.Models.DTOs.Documents;
using KycManagementSystem.Api.Models.Entities;
using KycManagementSystem.Api.Repositories.Interfaces;

namespace KycManagementSystem.Api.Repositories.Implementations
{
    public class DocumentsRepository: IDocumentsRepository
    {
        private readonly ISqlConnectionFactory _connectionFactory;
        public DocumentsRepository(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<int>UploadDocumentAsync(KycDocument document)
        {
            string sql = @"
                    Insert into KycDocuments(kycId, DocType,FilePath,UploadedAt)
                    Values (@KycId,@DocType,@FilePath,GetDate());
                    Select Cast(Scope_Identity() as int)";
            using var conn =_connectionFactory.CreateConnection();
            return await conn.QuerySingleAsync<int>(sql, document);
        }

        public async Task<IEnumerable<KycDocument>> GetDocumentsByKycIdAsync(int kycId)
{
    string sql = @"
        SELECT 
            Id,
            KycId,
            DocType,
            FilePath,
            UploadedAt
        FROM KycDocuments
        WHERE KycId = @KycId
        ORDER BY UploadedAt DESC;
    ";

    using var conn = _connectionFactory.CreateConnection();
    return await conn.QueryAsync<KycDocument>(sql, new { KycId = kycId });
}


        public async Task UpdateDocumentAsync(int id, IFormFile file)
        {
            const string sql = "UPDATE KycDocuments SET FilePath = @FilePath WHERE Id = @id";
            using var conn = _connectionFactory.CreateConnection();
            await conn.ExecuteAsync(sql, new { filePath = file.FileName, id });
        }
        public async Task DeleteDocumentAsync(int kycId)
        {
            const string sql = "DELETE FROM KycDocuments WHERE KycId = @kycId";

            using var conn = _connectionFactory.CreateConnection();
            await conn.ExecuteAsync(sql, new { kycId });
        }

    }
}
