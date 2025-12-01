namespace LoanManagementSystem.Models
{
    public class AuditLog
    {
        public int AuditId { get; set; }

        public int UserId { get; set; }

        public string Action { get; set; }

        public string TableName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DateTime Timestamp { get; set; }

    }
}
