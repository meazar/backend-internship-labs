
using System.Text.Json.Serialization;

namespace KycManagementSystem.Api.Models.DTOs.Kyc
{
    public class CreateKycDto
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string FatherName { get; set; }
        public string GrandfatherName { get; set; }
        public string Occupation {  get; set; }
        public decimal AnnualIncome { get; set; }
        public string Nationality { get; set; }
        public IFormFile Photo { get; set; }
        public DateTime DateOfBirth { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Gender Gender { get; set; }
        public string Address { get; set; }
        public string Remarks { get; set; }
        
       
    }
}
