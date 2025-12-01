namespace KycManagementSystem.Api.Models.Entities
{
    public class SanctionCheckResultcs
    {
        public int Id { get; set; }
        public int KycProfileId { get; set; }
        public string? MatchedName { get; set; }
        public int? MatchScore { get; set; }
        public bool IsPositiveMatch { get; set; } = false;
        public DateTime CheckedOn { get; set; } = DateTime.Now;
    }
}

