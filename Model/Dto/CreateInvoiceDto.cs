namespace MechantInventory.Model.Dto
{
    public class CreateInvoiceDto
    {
        public Guid CustomerId { get; set; }
        public List<InvoiceItemDto> InvoiceItems { get; set; }
        public string PaymentStatus { get; set; }

    }
}
