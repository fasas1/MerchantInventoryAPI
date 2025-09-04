namespace MechantInventory.Model.Dto
{
    public class ResellerReturnDto
    {
        public int ResellerTransactionId { get; set; }
        public decimal Value { get; set; } 
        public string Reason { get; set; }
        public string ApprovedBy { get; set; }
        public List<ResellerReturnItemDto> Items { get; set; } = new();
    }
}
