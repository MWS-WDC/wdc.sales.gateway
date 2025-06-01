namespace Wdc.Sales.Orders.Api.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string ShippingAddress { get; set; } = string.Empty;
        public ICollection<OrderItem> Items { get; set; } = [];
    }
}
