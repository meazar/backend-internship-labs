namespace LoanManagementSystem.Models
{
    public class LoanAccount
    {

        public int LoanAccountId { get; set; }
        public int ApplicationId { get; set; }
        public int UserId { get; set; }

        public decimal PrincipalAmount { get; set; }

        public decimal InterestRate { get; set; }

        public decimal EMIAmount { get; set; }

        public decimal TotalInterest { get; set; }

        public decimal TotalPayable {  get; set; }

        public decimal BalanceAmount { get; set; }

        public decimal PenaltyAmount { get; set; }

        public string LoanStatus { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }


    }
}
