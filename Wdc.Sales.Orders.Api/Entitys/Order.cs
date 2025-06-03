using Wdc.Sales.Orders.Api.Enums;

namespace Wdc.Sales.Orders.Api.Entitys
{
    public class Order
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<OrderItem> Items { get; set; } = [];
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
    }
}
