using Wdc.Sales.Orders.Api.Events;

namespace Wdc.Sales.Orders.Api.Messaging
{
    public interface IOrderEventPublisher
    {
        Task PublishOrderCancelledAsync(OrderCancelledEvent orderEvent);
    }

}
