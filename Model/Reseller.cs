using System.ComponentModel.DataAnnotations.Schema;

namespace MechantInventory.Model
{
    public class Reseller
    {
        public int ResellerId { get; set; }
        public string Name { get; set; }

        public string Phone { get; set; }
        public string Address { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal CreditBalance { get; set; }

        public ICollection<ResellerTransaction> Transactions { get; set; }
  
    }

}
