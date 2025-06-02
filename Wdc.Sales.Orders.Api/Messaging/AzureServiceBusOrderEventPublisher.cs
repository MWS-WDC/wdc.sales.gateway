using Azure.Messaging.ServiceBus;
using System.Text.Json;
using Wdc.Sales.Orders.Api.Events;
using Wdc.Sales.Orders.Api.Messaging;

public class AzureServiceBusOrderEventPublisher : IOrderEventPublisher
{
    private readonly ServiceBusSender _sender;

    public AzureServiceBusOrderEventPublisher(ServiceBusClient client, string topicName)
    {
        _sender = client.CreateSender(topicName);
    }

    public async Task PublishOrderCancelledAsync(OrderCancelledEvent orderEvent)
    {
        string body = JsonSerializer.Serialize(orderEvent);
        ServiceBusMessage message = new(body)
        {
            Subject = orderEvent.EventType
        };

        await _sender.SendMessageAsync(message);
    }
}
