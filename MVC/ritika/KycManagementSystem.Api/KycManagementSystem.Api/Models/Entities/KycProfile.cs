using KycManagementSystem.Api.Models.DTOs.Kyc;

namespace KycManagementSystem.Api.Models.Entities
{
    public class KycProfile
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string Address { get; set; } = null!;
        public string? FatherName { get; set; }
        public string? GrandfatherName { get; set; }
        public string? Occupation { get; set; }
        public decimal? AnnualIncome { get; set; }
        public string? Nationality { get; set; }
        public string? PhotoPath { get; set; }
        public string DocumentType { get; set; } = null!;
        public string DocumentNumber { get; set; } = null!;
        public string Status { get; set; } = "Pending";
        public string? Remarks { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

       
    }
}
