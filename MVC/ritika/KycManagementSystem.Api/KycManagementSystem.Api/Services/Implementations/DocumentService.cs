using System.Data.SqlClient;
using Dapper;
using KycManagementSystem.Api.Database;
using KycManagementSystem.Api.Models.DTOs.Documents;
using KycManagementSystem.Api.Models.Entities;
using KycManagementSystem.Api.Repositories.Interfaces;
using KycManagementSystem.Api.Services.Interfaces;

namespace KycManagementSystem.Api.Services.Implementations
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentsRepository _docRepo;
        private readonly IWebHostEnvironment _env;
        private readonly ISqlConnectionFactory _connectionFactory;

        public DocumentService(IDocumentsRepository docRepo, IWebHostEnvironment env, ISqlConnectionFactory connectionFactory)
        {
            _docRepo = docRepo;
            _env = env;
            _connectionFactory = connectionFactory;
        }

        public async Task<int> UploadDocumentAsync(DocumentUploadDto dto)
        {
            
            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.File.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await dto.File.CopyToAsync(stream);

            var document = new KycDocument
            {
                KycId = dto.KycId,
                DocType = dto.DocType,
                FilePath = fileName
            };

            return await _docRepo.UploadDocumentAsync(document);
        }


        public async Task<IEnumerable<KycDocument>> GetDocumentsByKycIdAsync(int kycId)
        {

            var docs = await _docRepo.GetDocumentsByKycIdAsync(kycId);

            return docs.Select(d => new KycDocument
            {
                KycId = d.KycId,
                DocType = d.DocType,
                FilePath = d.FilePath
            });
        }
        public async Task UpdateDocumentAsync(int id, IFormFile file)
        {
            await _docRepo.UpdateDocumentAsync(id, file);
        }

        public async Task DeleteDocumentAsync(int id)
        {
            await _docRepo.DeleteDocumentAsync(id);
        }
    }
}
