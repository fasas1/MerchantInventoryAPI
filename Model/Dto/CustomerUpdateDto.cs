using System.ComponentModel.DataAnnotations;

namespace MechantInventory.Model.Dto
{
    public class CustomerUpdateDto
    {
        public Guid CustomerId { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
