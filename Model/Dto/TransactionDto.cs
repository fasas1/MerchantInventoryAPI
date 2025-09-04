namespace MechantInventory.Model.Dto
{
    public class TransactionDto

    {
        public int TransactionId { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int QuantityChanged { get; set; }
        public string TransactionType { get; set; } // "Sale" or "Restock"
        public string PerformedBy { get; set; }
        public DateTime Timestamp { get; set; }
        public string? Description { get; set; }
    }
}
