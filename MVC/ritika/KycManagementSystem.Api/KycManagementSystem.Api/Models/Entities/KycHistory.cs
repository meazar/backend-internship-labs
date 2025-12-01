namespace KycManagementSystem.Api.Models.Entities
{
    public class KycHistory
    {
        public int Id { get; set; }
        public int KycId { get; set; }
        public string Action { get; set; } = null!; 
        public string? Remarks { get; set; }
        public int? OfficerId { get; set; }
        public DateTime ActionDate { get; set; } = DateTime.Now;
    }
}

