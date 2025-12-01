namespace KycManagementSystem.Api.Models.Entities
{
    public class KycDocument
    {
        public int Id { get; set; }
        public int KycId { get; set; }
        public string DocType { get; set; } = null!;
        public string FilePath { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.Now;
        
    }
}
