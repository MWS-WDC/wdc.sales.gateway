namespace Wdc.Sales.Orders.Api.Entitys
{
    public class OrderItem
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string ProductId { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string OrderId { get; set; } = string.Empty;
        public Order? Order { get; set; }
    }
}
