namespace MechantInventory.Model.Dto
{
    public class AuditLogDto
    {
        public int? TransactionId { get; set; }
        public string IssueType { get; set; }
        public decimal Difference { get; set; }
        public string RecordedBy { get; set; }
        public DateTime Date { get; set; }
    }

}
