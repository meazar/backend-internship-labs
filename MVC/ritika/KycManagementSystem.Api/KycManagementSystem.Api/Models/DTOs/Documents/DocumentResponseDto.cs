namespace KycManagementSystem.Api.Models.DTOs.Documents
{
    public class DocumentResponseDto
    {
        public int Id { get; set; }
        public int KycId { get; set; }
        public string DocType { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadedAt { get; set; }
    }

}
