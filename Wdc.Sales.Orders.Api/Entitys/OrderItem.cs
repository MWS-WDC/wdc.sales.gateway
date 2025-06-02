namespace Wdc.Sales.Orders.Api.Entitys
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public string ProductId { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string OrderId { get; set; } = string.Empty;
        public Order? Order { get; set; }
    }
}
