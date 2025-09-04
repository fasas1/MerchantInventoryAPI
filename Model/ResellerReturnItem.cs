using System.ComponentModel.DataAnnotations.Schema;

namespace MechantInventory.Model
{
    public class ResellerReturnItem
    {
        public int ResellerReturnItemId { get; set; }

        public int ResellerReturnId { get; set; }
        public ResellerReturn ResellerReturn { get; set; }

        public int ResellerTransactionItemId { get; set; }
        public ResellerTransactionItem ResellerTransactionItem { get; set; }

        public int QuantityReturned { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }

    }


}
