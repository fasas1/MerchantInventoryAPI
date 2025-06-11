using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MerchantInventory.Models.Dto
{
    public class ProductUpdateDto
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        public string Name { get; set; }
     
        public string Category { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        [Required]
        public IFormFile Image { get; set; }
    }
}
