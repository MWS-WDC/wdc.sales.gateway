using System.ComponentModel.DataAnnotations;

namespace Wdc.Sales.Payments.Api.Persistence.ServiceBus;

public class ServiceBusToOrderOptions
{

    public const string ServiceBusToOrder = "ServiceBusToOrder";

    [Required]
    public string TopicName { get; init; } = string.Empty;

    public string SubscriptionName { get; init; } = string.Empty;
}
