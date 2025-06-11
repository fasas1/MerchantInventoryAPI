namespace MechantInventory.Model.Dto
{
    public class TransactionReadDto
        {
            public int ProductId { get; set; }
            public string? ProductName { get; set; }
            public int Quantity { get; set; }
            public string Type { get; set; } // "Sale" or "Restock"
            public string PerformedBy { get; set; } 
    
    }    
}
