namespace MechantInventory.Model.Dto
{
    public class ResellerCreateDto
    {
        public string Name { get; set; }

        public string Phone { get; set; }
        public string Address { get; set; }

        public decimal CreditBalance { get; set; }
    }
}
