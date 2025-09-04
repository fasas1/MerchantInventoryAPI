namespace MechantInventory.Model.Dto
{
    public class InvoiceDto
    {
        public Guid InvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal TotalAmount { get; set; }

        public CustomerDto Customer { get; set; }
        public List<InvoiceItemDto> InvoiceItems { get; set; }
    }

}
