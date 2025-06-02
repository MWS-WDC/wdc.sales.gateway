using Wdc.Sales.Orders.Api.Enums;

namespace Wdc.Sales.Orders.Api.Entitys
{
    public class Order
    {
        public string Id { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string ShippingAddress { get; set; } = string.Empty;
        public ICollection<OrderItem> Items { get; set; } = [];
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
    }
}
