namespace LoanManagementSystem.Models
{
    public class PaymentSchedule
    {
        public int ScheduleId { get; set; }
        public int LoanAccountId { get; set; }

        public int InstallationNO { get; set; }

        public DateTime DueDate { get; set; }
        public decimal Amount { get; set; }

        public decimal PrincipalPortion { get; set; }

        public decimal InterestPortion { get; set; }

        public string Status { get; set; }

        public DateTime? PaidDate { get; set; }

        public decimal PenaltyAdded { get; set ; }
    }
}
