namespace KycManagementSystem.Api.Models.DTOs.Ofac
{
    public class OfacCheckResponse
    {
        public string FullName { get; set; }
        public string? Nationality { get; set; }
        public bool IsSanctioned { get; set; }
        public DateTime CheckedOn { get; set; }
        public List<OfacMatchDto>? Matches { get; set; }
    }
}
