using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MechantInventory.Model.Dto
{
    public class StockReadDto
    { 
            public int StockId { get; set; }
    
            [Range(0, int.MaxValue, ErrorMessage = "Quantity must be zero or more.")]
            public int CurrentQuantity { get; set; }
            public DateTime LastUpdated { get; set; }
          
          
            [Range(1, int.MaxValue, ErrorMessage = "Threshold must be at least 1.")]
            public int Threshold { get; set; }
            public int ProductId { get; set; }
            public string? ProductName { get; set; }
        public decimal? ProductPrice { get; set; }
    }
    }

