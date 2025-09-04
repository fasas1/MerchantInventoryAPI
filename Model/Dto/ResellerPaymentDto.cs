namespace MechantInventory.Model.Dto
{
    public class ResellerPaymentDto
    {
        public int ResellerTransactionId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } 
        public string RecordedBy { get; set; }
    }
}
