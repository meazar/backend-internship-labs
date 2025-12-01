namespace LoanManagementSystem.Models
{
    public class MakePaymentRequest
    {
        public int LoanAccountId { get; set; }

        public int ScheduleId { get; set; }

        public decimal AmoundPaid { get; set; }

        public string PaymentMethod { get; set; }

        public string TransactionId { get; set; }

        public string Remarks { get; set; }

    }
}
