using System.ComponentModel.DataAnnotations;

namespace Wdc.Sales.Orders.Api.Persistence.ServiceBus
{
    public class ServiceBusOptions
    {
        public const string ServiceBus = "ServiceBus";

        [Required]
        public required string TopicName { get; init; }

        [Required]
        public required string SubscriptionName { get; init; }
    }
}

