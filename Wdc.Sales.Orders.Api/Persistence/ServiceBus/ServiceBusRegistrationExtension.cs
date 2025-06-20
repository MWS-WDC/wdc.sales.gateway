﻿namespace Wdc.Sales.Orders.Api.Persistence.ServiceBus
{
    public static class ServiceBusRegistrationExtension
    {
        public static void AddServiceBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddServiceBusOptions(configuration);
            services.AddSingleton(new ServiceBus(configuration));
            services.ListenToEvents();
        }

        private static void AddServiceBusOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<ServiceBusOptions>()
                .Bind(configuration.GetSection(ServiceBusOptions.ServiceBus))
                .ValidateDataAnnotations()
                .ValidateOnStart();
        }

        private static void ListenToEvents(this IServiceCollection services)
            => services.AddHostedService<ServiceBusListener>();
    }
}
