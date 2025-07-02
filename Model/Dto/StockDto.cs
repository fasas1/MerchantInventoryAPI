namespace MechantInventory.Model.Dto
{

        public class StockDto
        {
            public int ProductId { get; set; }
            public int CurrentQuantity { get; set; }
            public int Threshold { get; set; }
            public DateTime LastUpdated { get; set; }
        }


}
