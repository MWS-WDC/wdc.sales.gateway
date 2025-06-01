namespace Wdc.Sales.Orders.Api.DTOs
{

    public class CreateOrderInputModel
    {
        public string ShippingAddress { get; set; } = string.Empty;
        public List<OrderItemModel> Items { get; set; } = [];
    }

    public class OrderItemModel
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
