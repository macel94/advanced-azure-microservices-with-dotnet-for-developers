using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace WisdomPetMedicine.Hospital.Infrastructure
{
    public class WisdomPetMedicineCosmosDbHealthCheck : IHealthCheck
    {
        private readonly CosmosClient cosmosClient;
        private readonly IConfiguration configuration;

        public WisdomPetMedicineCosmosDbHealthCheck(IConfiguration configuration)
        {
            this.configuration = configuration;
            var cosmosClientOptions = new CosmosClientOptions()
            {
                HttpClientFactory = () =>
                {
                    HttpMessageHandler httpMessageHandler = new HttpClientHandler()
                    {
                        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                    };

                    return new HttpClient(httpMessageHandler);
                },
                ConnectionMode = ConnectionMode.Gateway
            };

            cosmosClient = new CosmosClient(configuration["CosmosDb:ConnectionString"], cosmosClientOptions);
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            await cosmosClient.ReadAccountAsync();
            var databaseId = configuration["CosmosDb:DatabaseId"];
            var containerId = configuration["CosmosDb:ContainerId"];
            var response = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
            await response.Database.CreateContainerIfNotExistsAsync(new ContainerProperties(containerId, "aggregateId"));
            var container = cosmosClient.GetContainer(databaseId, containerId);
            var containerProperties = await container.ReadContainerAsync(cancellationToken: cancellationToken);
            return containerProperties.StatusCode == System.Net.HttpStatusCode.OK ? HealthCheckResult.Healthy() :
                HealthCheckResult.Unhealthy();
        }
    }
}