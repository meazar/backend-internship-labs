namespace LoanManagementSystem.Models
{
    public class AddPenaltyRequest
    {
        public int LoanAccountId { get; set; }
        public int ScheduleId { get; set; }

        public decimal PenaltyAmount { get; set; }

    }
}
