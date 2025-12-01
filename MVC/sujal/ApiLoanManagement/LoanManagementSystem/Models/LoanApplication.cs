namespace LoanManagementSystem.Models
{
    public class LoanApplication
    {
        public int ApplicationId { get; set; }

        public int UserId { get; set; }

        public int LoanTypeId { get; set; }
        public  decimal RequestedAmount {  get; set; }

        public int DurationMonth { get; set; }

        public string Status {  get; set; }

        public int? VerifiedBy { get; set; }

        public DateTime? VerifiedAt { get; set; }

        public string? OfficerRemarks { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
