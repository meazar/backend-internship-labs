namespace LoanManagementSystem.Models
{
    public class Document
    {
        public int DocumentId { get; set; }
        public int ApplicationId { get; set; }

        public int UserId { get; set; }

        public string DocumentType { get; set; }

        public string FileName { get; set; }

        public string FileUrl { get; set; }

        public DateTime UploadedAt { get; set; }

    }
}
