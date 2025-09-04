namespace MechantInventory.Model.Dto
{
    public class TransactionCreateDto
        {
            public int ProductId { get; set; }
           public string? ProductName { get; set; }
           public int Quantity { get; set; }
            public string Type { get; set; } // "Sale" or "Restock"
            public string PerformedBy { get; set; }
        public string Description { get; set; }

    }    
}
