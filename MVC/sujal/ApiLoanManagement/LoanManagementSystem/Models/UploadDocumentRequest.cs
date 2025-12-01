
namespace LoanManagementSystem.Models
{
    public class UploadDocumentRequest
    {
        public int ApplicationId { get; set; }
        public int UserId { get; set; }
        public string DocumentType { get; set; } = string.Empty;
        public IFormFile File { get; set; } = null!;
    }
}
