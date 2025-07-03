namespace MechantInventory.Model
{
    public class Invoice
    {
        public Guid InvoiceId { get; set; }
        public string  InvoiceNumber { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime InvoiceDate { get; set; } 
        public decimal  TotalAmount { get; set; }
        public string PaymentStatus { get; set; }
        public Customer Customer { get; set; }
        public ICollection<InvoiceItem> InvoiceItems { get; set; }

    }
}
