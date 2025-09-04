namespace MechantInventory.Model
{
    public class AuditLog
    {
        public int AuditLogId { get; set; }
        public int? TransactionId { get; set; }
        public string IssueType { get; set; } // e.g., SuspiciousReturn, OverReturn, UnderReport
        public decimal Difference { get; set; }
        public string RecordedBy { get; set; }
        public DateTime Date { get; set; }

        // Optional navigation property
       // public ResellerTransaction Transaction { get; set; }
    }

}
