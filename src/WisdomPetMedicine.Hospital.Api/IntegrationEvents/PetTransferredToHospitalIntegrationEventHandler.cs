using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace WisdomPetMedicine.Hospital.Api.IntegrationEvents
{
    public class PetTransferredToHospitalIntegrationEventHandler : BackgroundService
    {
        private readonly IConfiguration config;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly ILogger<PetTransferredToHospitalIntegrationEventHandler> logger;

        private readonly ServiceBusClient client;
        private readonly ServiceBusProcessor processor;

        public PetTransferredToHospitalIntegrationEventHandler(IConfiguration config, 
            IServiceScopeFactory serviceScopeFactory,
            ILogger<PetTransferredToHospitalIntegrationEventHandler> logger)
        {
            this.config = config;
            this.serviceScopeFactory = serviceScopeFactory;
            this.logger = logger;

            client = new ServiceBusClient(config["ServiceBus:ConnectionString"]);
            processor = client.CreateProcessor(topicName: config["ServiceBus:TopicName"], subscriptionName: config["ServiceBus:SubscriptionName"]);
            processor.ProcessMessageAsync += Processor_ProcessMessageAsync;
            processor.ProcessErrorAsync += Processor_ProcessErrorAsync;
        }

        private Task Processor_ProcessErrorAsync(ProcessErrorEventArgs arg)
        {
            throw new System.NotImplementedException();
        }

        private Task Processor_ProcessMessageAsync(ProcessMessageEventArgs arg)
        {
            throw new System.NotImplementedException();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
