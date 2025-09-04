namespace MechantInventory.Model.Dto
{
    public class ResellerTransactionDto
    {
        public int ResellerId { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public List<ResellerTransactionItemDto> Items { get; set; }
    }

}
