using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace KycManagementSystem.Api.Models.DTOs.Kyc
{
    public class KycProfileDto
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Gender Gender { get; set; }

        public int Id { get; set; }
        public int UserId { get; set; }
        public string? FullName { get; set; }
        public string? FatherName { get; set; }
        public string? GrandfatherName { get; set; }
        public string? Occupation { get; set; }
        public decimal AnnualIncome { get; set; }
        public string? Nationality { get; set; }
        public string? PhotoPath { get; set; }
        public string? Status { get; set; }
        public string? Remarks { get; set; }
        public bool IsOfacMatched { get; set; }
        public string? OfacMessage { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Address { get; set; }
       
    }
    public enum Gender
    {
        Male,
        Female,
        Other
    }
}
