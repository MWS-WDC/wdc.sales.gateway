using System.ComponentModel.DataAnnotations;

namespace Wdc.Sales.Payments.Api.Persistence.ServiceBus;

public class ServiceBusOptions
{

    public const string ServiceBus = "ServiceBus";

    [Required]
    public string TopicName { get; init; } = string.Empty;

    public string SubscriptionName { get; init; } = string.Empty;
}
