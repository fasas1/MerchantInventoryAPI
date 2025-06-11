namespace MechantInventory.Model
{
    public class Transaction
    {
        public int TransactionId { get; set; }       
        public int ProductId { get; set; }           
        public Product Product { get; set; }         
        public int QuantityChanged { get; set; }     // The quantity added/removed 
        public string TransactionType { get; set; }  // Type of transaction (Sale or Restock)
        public DateTime Timestamp { get; set; }      // Timestamp for when the transaction occurred
        public string  PerformedBy { get; set; }
    }

}
