namespace KycManagementSystem.Api.Models.Entities
{
    public class DummyOfacRecord
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string? AliasName { get; set; }
        public string? Nationality { get; set; }
        public string? Category { get; set; }
        public int? RiskLevel { get; set; }
        public string? Notes { get; set; }
    }
}
