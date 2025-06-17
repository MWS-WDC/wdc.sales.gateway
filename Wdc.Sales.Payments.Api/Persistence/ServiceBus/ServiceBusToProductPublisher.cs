using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text.Json.Serialization;
using Wdc.Sales.Payments.Api.abstracts;

namespace Wdc.Sales.Payments.Api.Persistence.ServiceBus
{
    public class ServiceBusToProductPublisher
    {
        protected readonly ServiceBusSender _sender;

        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        public ServiceBusToProductPublisher(IOptions<ServiceBusToProductOptions> options, IConfiguration configuration)
        {
            ServiceBusClient client = new(configuration.GetConnectionString("ServiceBusToProduct"));

            _sender = client.CreateSender(options.Value.TopicName);
        }

        public async Task PublishEventAsync(IEvent @event)
        {
            string json = JsonSerializer.Serialize(@event, _jsonSerializerOptions);

            ServiceBusMessage serviceBusMessage = new(json)
            {
                PartitionKey = @event.AggregateId.ToString(),
                SessionId = @event.AggregateId.ToString(),
                Subject = @event.GetType().Name,
            };

            await _sender.SendMessageAsync(serviceBusMessage);
        }
    }
}
