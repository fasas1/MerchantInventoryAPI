﻿namespace MechantInventory.Model.Dto
{
    public class InvoiceItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
