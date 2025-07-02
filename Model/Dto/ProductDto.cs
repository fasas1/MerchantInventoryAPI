using System.ComponentModel.DataAnnotations.Schema;

namespace MechantInventory.Model.Dto
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public string Category { get; set; }
        public string ImageUrl { get; set; }
        public ICollection<Stock> Stocks { get; set; }
    }
}
