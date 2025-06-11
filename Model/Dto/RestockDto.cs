using System.ComponentModel.DataAnnotations;

namespace MechantInventory.Model.Dto
{
    public class RestockDto
    {
      [Range(1, int.MaxValue, ErrorMessage = "Restock quantity must be at least 1.")]
      public int Quantity { get; set; }
    }

}
