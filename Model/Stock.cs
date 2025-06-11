using MechantInventory.Model;
using System.ComponentModel.DataAnnotations.Schema;

public class Stock
{
    public int StockId { get; set; }             
    public int CurrentQuantity { get; set; }     
    public DateTime LastUpdated { get; set; }
    [ForeignKey("Product")]
    public int ProductId { get; set; }          
    public Product Product { get; set; }         
    public int Threshold { get; set; }           
}
