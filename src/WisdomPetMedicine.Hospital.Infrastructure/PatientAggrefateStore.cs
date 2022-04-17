using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using WisdomPetMedicine.Hospital.Domain.Entities;
using WisdomPetMedicine.Hospital.Domain.Repositories;
using WisdomPetMedicine.Hospital.Domain.ValueObjects;

namespace WisdomPetMedicine.Hospital.Infrastructure
{
    public class PatientAggrefateStore : IPatientAggregateStore
    {
        private readonly CosmosClient _client;
        private readonly Container _container;

        public PatientAggrefateStore(IConfiguration configuration)
        {
            var cs = configuration["CosmosDb:ConnectionString"];
            var dbId = configuration["CosmosDb:DatabaseId"];
            var containerId = configuration["CosmosDb:ContainerId"];

            _client = new CosmosClient(cs, new CosmosClientOptions()
            {
                SerializerOptions = new CosmosSerializationOptions()
                {
                    PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                }
            });

            _container = _client.GetContainer(dbId, containerId);
        }
        public Task<Patient> LoadAsync(PatientId patient)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync(Patient patient)
        {
            throw new NotImplementedException();
        }
    }
}