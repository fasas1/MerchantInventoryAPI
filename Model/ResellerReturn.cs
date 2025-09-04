using System.ComponentModel.DataAnnotations.Schema;

namespace MechantInventory.Model
{
    public class ResellerReturn
    {
        public int ResellerReturnId { get; set; }
        public int ResellerTransactionId { get; set; }
        public ResellerTransaction Transaction { get; set; }

        public decimal Value { get; set; }
        public DateTime ReturnDate { get; set; }
        public string Reason { get; set; }
        public string ApprovedBy { get; set; }

        public ICollection<ResellerReturnItem> Items { get; set; }
    }
}
