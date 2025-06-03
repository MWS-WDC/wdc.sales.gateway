using Azure.Messaging.ServiceBus;

namespace Wdc.Sales.Products.Api.Persistence.ServiceBus
{
    public class ServiceBus(IConfiguration configuration)
    {
        public ServiceBusClient Client { get; } = new ServiceBusClient(configuration.GetConnectionString("ServiceBus"));
    }
}
