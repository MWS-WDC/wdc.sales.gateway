using Azure.Messaging.ServiceBus;

namespace Wdc.Sales.Orders.Api.Persistence.ServiceBus
{
    public class ServiceBus(IConfiguration configuration)
    {
        public ServiceBusClient Client { get; } = new ServiceBusClient(configuration.GetConnectionString("ServiceBus"));
    }
}
