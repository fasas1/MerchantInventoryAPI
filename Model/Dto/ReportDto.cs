namespace MechantInventory.Model.Dto
{
    public class InventoryReportDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public int Quantity { get; set; }
        public bool LowStock { get; set; }
    }

    public class SalesReportDto
    {
        public string ProductName { get; set; }
        public int TotalQuantitySold { get; set; }
        public decimal TotalRevenue { get; set; }
    }
    public class SummaryReportDto
    {
        public decimal TotalRevenue { get; set; }
        public int TotalRestockedItems { get; set; }
        public int TotalSalesCount { get; set; }
        public string TopSellingProduct { get; set; }
        public int TopSellingQuantity { get; set; }
    }

}
