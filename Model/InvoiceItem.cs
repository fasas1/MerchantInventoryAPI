namespace MechantInventory.Model
{
    public class InvoiceItem
    {
        public Guid InvoiceItemId { get; set; }
        public Guid InvoiceId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public Product Product { get; set; }
        public Invoice Invoice { get; set; }
    }
}
