using System.ComponentModel.DataAnnotations.Schema;

namespace MechantInventory.Model
{
    public class ResellerTransaction
    {
        public int ResellerTransactionId { get; set; }
        public int ResellerId { get; set; }
        public Reseller Reseller { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal AmountPaid { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal OverPayment { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }

        public string Status { get; set; } // Pending, Partial, Paid, Overdue
        public DateTime CreatedAt { get; set; }
        public DateTime DueDate { get; set; }
        public string Description { get; set; }

        public ICollection<ResellerTransactionItem> Items { get; set; }
        public ICollection<ResellerPayment> Payments { get; set; }
        public ICollection<ResellerReturn> Returns { get; set; }
    }


}
