namespace Wdc.Sales.Orders.Api.Models
{

    public class CreateOrderInputModel
    {
        public string ShippingAddress { get; set; } = string.Empty;
        public List<OrderItemModel> Items { get; set; } = [];
    }

    public class OrderItemModel
    {
        public string ProductId { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}
