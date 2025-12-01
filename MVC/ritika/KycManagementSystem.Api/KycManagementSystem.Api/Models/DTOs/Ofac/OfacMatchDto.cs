namespace KycManagementSystem.Api.Models.DTOs.Ofac
{
    public class OfacMatchDto
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? AliasName { get; set; }
        public string? Nationality { get; set; }
        public string? Category { get; set; }
        public int? RiskLevel { get; set; }
        public string? Notes { get; set; }
    }
}
