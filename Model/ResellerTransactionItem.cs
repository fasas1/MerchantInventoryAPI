using System.ComponentModel.DataAnnotations.Schema;

namespace MechantInventory.Model
{
    public class ResellerTransactionItem
    {
        public int ResellerTransactionItemId { get; set; }

        public int ResellerTransactionId { get; set; }
        public ResellerTransaction ResellerTransaction { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }
        public int ReturnedQuantity { get; set; } 

        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => Quantity * UnitPrice;

        public ICollection<ResellerReturnItem> ReturnItems { get; set; }
    }
}
