using Azure.Messaging.ServiceBus;
using MediatR;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;
using Wdc.Sales.Products.Api.Events;

namespace Wdc.Sales.Products.Api.Persistence.ServiceBus
{
    public class ServiceBusListener : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<ServiceBusListener> _logger;
        private readonly ServiceBusSessionProcessor _processor;
        private readonly ServiceBusProcessor _deadLetterProcessor;

        public ServiceBusListener(
            IOptions<ServiceBusOptions> serviceBusOptions,
            ILogger<ServiceBusListener> logger,
            ServiceBus serviceBus,
            IServiceProvider serviceProvider,
            IWebHostEnvironment environment
        )
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _environment = environment;
            ServiceBusOptions azure = serviceBusOptions.Value;
            _processor = serviceBus.Client.CreateSessionProcessor(
                topicName: azure.TopicName,
                subscriptionName: azure.SubscriptionName,
                options: new ServiceBusSessionProcessorOptions
                {
                    AutoCompleteMessages = false,
                    PrefetchCount = 1,
                    MaxConcurrentSessions = 100,
                    MaxConcurrentCallsPerSession = 1
                });
            _processor.ProcessMessageAsync += Processor_ProcessMessageAsync;
            _processor.ProcessErrorAsync += Processor_ProcessErrorAsync;

            _deadLetterProcessor = serviceBus.Client.CreateProcessor(
                topicName: azure.TopicName,
                subscriptionName: azure.SubscriptionName,
                options: new ServiceBusProcessorOptions()
                {
                    AutoCompleteMessages = false,
                    PrefetchCount = 10,
                    MaxConcurrentCalls = 10,
                    SubQueue = SubQueue.DeadLetter,
                    ReceiveMode = ServiceBusReceiveMode.PeekLock
                });

            _deadLetterProcessor.ProcessMessageAsync += DeadLetterProcessor_ProcessMessageAsync;
            _deadLetterProcessor.ProcessErrorAsync += Processor_ProcessErrorAsync;
        }

        private async Task DeadLetterProcessor_ProcessMessageAsync(ProcessMessageEventArgs args)
        {
            string json = Encoding.UTF8.GetString(args.Message.Body);
            bool isHandled = await HandleMessage(json, args.Message.Subject);
            if (isHandled)
            {
                await args.CompleteMessageAsync(args.Message);
            }
            else
            {
                _logger.LogWarning("Message {MessageId} not handled", args.Message.MessageId);
                await Task.Delay(5000);
                await args.AbandonMessageAsync(args.Message);
            }
        }

        private Task Processor_ProcessErrorAsync(ProcessErrorEventArgs args)
        {
            _logger.LogError("Message {MessageId} not handled", args.ErrorSource);
            return Task.CompletedTask;
        }

        private async Task Processor_ProcessMessageAsync(ProcessSessionMessageEventArgs args)
        {
            string json = Encoding.UTF8.GetString(args.Message.Body);
            try
            {
                bool isHandled = await HandleMessage(json, args.Message.Subject);

                if (isHandled)
                {
                    await args.CompleteMessageAsync(args.Message);
                }
                else
                {
                    _logger.LogWarning("Message {MessageId} not handled", args.Message.MessageId);
                    await Task.Delay(5000);
                    await args.AbandonMessageAsync(args.Message);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error : {e}", e);
            }
        }

        private async Task<bool> HandleMessage(string message, string subject)
        {
            using IServiceScope scope = _serviceProvider.CreateScope();
            IMediator mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            Event @event = JsonDocument.Parse(message).ToEvent(subject, _logger);
            return await mediator.Send(@event);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _processor.StartProcessingAsync(cancellationToken);
            if (_environment.IsProduction())
                await _deadLetterProcessor.StartProcessingAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _processor.CloseAsync(cancellationToken);
            if (_environment.IsProduction())
                await _deadLetterProcessor.CloseAsync(cancellationToken);
        }
    }
}