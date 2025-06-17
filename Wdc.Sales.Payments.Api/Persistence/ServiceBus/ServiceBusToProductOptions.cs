using System.ComponentModel.DataAnnotations;

namespace Wdc.Sales.Payments.Api.Persistence.ServiceBus;

public class ServiceBusToProductOptions
{

    public const string ServiceBusToProduct = "ServiceBusToProduct";

    [Required]
    public string TopicName { get; init; } = string.Empty;

    public string SubscriptionName { get; init; } = string.Empty;
}
