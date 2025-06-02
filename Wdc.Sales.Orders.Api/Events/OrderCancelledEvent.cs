namespace Wdc.Sales.Orders.Api.Events
{
    public class OrderCancelledEvent
    {
        public string OrderId { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public DateTime CancelledAt { get; set; } = DateTime.UtcNow;
        public string EventType => "OrderCancelled";
    }

}
