using System.ComponentModel.DataAnnotations;

namespace Wdc.Sales.Products.Api.Persistence.ServiceBus
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

