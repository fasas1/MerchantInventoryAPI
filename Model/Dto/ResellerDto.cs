namespace MechantInventory.Model.Dto
{
    public class ResellerDto
    {
        public int ResellerId { get; set; }
        public string Name { get; set; }

        public string Phone { get; set; }
        public string Address { get; set; }

        //public decimal CreditBalance { get; set; }

        //public ICollection<ResellerTransaction> Transactions { get; set; }
    }
}
