﻿namespace Wdc.Sales.Orders.Api.Models
{
    public class OrderItemModel
    {
        public string ProductId { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}
