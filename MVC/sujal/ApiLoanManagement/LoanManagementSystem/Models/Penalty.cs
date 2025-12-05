
namespace LoanManagementSystem.Models
{
    public class Penalty
    {
        public int PenaltyId { get; set; }
        public int LoanAccountId { get; set; }
        public int ScheduleId {  get; set; }

        public decimal PenaltyAmount { get; set; }
        public DateTime CreatedAt { get; set; }

        public string Status { get; set; }

    }
}
