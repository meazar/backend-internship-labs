using System.Globalization;

namespace KycManagementSystem.Api.Models.DTOs.Documents
{
    public class DocumentUploadDto
    {
     
        public int KycId { get; set; }
        public IFormFile? File { get; set; }
        public string? DocType { get; set; }
    }
}
