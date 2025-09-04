using System.ComponentModel.DataAnnotations.Schema;

namespace MechantInventory.Model
{
    public class ResellerPayment
    {
        public int ResellerPaymentId { get; set; }
        public int ResellerTransactionId { get; set; }
        public ResellerTransaction Transaction { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public string RecordedBy { get; set; }
    }


}
