namespace LoanManagementSystem.Models
{
    public class CreateLoanApplication
    {
        public int UserId { get; set; }

        public int LoanTypeId { get; set; }

        public decimal RequestedAmount {  get; set; }

        public int DurationMonths { get; set; }

    }
}
