using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
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

        public async Task<Patient> LoadAsync(PatientId patient)
        {
            throw new NotImplementedException();
        }

        public async Task SaveAsync(Patient patient)
        {
            if (patient == null)
            {
                throw new ArgumentNullException(nameof(patient));
            }

            var changes = patient.GetChanges()
              .Select(e => new CosmosEventData()
              {
                  Id = Guid.NewGuid(),
                  AggregateId = $"Patient-{patient.Id}",
                  EventName = e.GetType().Name,
                  Data = JsonConvert.SerializeObject(e),
                  AssemblyQualifiedName = JsonConvert.SerializeObject(e.GetType().AssemblyQualifiedName)
              }).AsEnumerable();

            if (!changes.Any())
            {
                return;
            }

            foreach (var item in changes)
            {
                await _container.CreateItemAsync(item);
            }

            patient.ClearChanges();
        }
    }
}