namespace LoanManagementSystem.Models
{
    public class Payments
    {
        public int PaymentId { get; set; }
        public int LoanAccountId { get; set; }
        public int ScheduleId { get; set; }

        public decimal AmountPaid { get; set; }

        public string PaymentMethod { get; set; }

        public string TransactionId { get; set; }

        public DateTime PaymentDate { get; set; }

        public string Remarks { get; set; }
    }
}




