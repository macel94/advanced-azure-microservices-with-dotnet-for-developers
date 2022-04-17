using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using WisdomPetMedicine.Common;
using WisdomPetMedicine.Hospital.Domain.Entities;
using WisdomPetMedicine.Hospital.Domain.Repositories;
using WisdomPetMedicine.Hospital.Domain.ValueObjects;

namespace WisdomPetMedicine.Hospital.Infrastructure
{
    public class PatientAggregateStore : IPatientAggregateStore
    {
        private readonly CosmosClient _client;
        private readonly Container _container;

        public PatientAggregateStore(IConfiguration configuration)
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

        public async Task<Patient> LoadAsync(PatientId patientId)
        {
            if (patientId == null)
            {
                throw new ArgumentNullException(nameof(patientId));
            }

            var aggregateId = GetAggregateIdFromPatient(patientId.Value);
            var sql = $"SELECT * FROM c WHERE c.aggregateId = '{aggregateId}'";
            var queryDefinition = new QueryDefinition(sql);
            var iterator = _container.GetItemQueryIterator<CosmosEventData>(queryDefinition);
            var allEvents = new List<CosmosEventData>();
            while (iterator.HasMoreResults)
            {
                var resultSet = await iterator.ReadNextAsync();
                foreach (var data in resultSet)
                {
                    allEvents.Add(data);
                }
            }

            var domainEvents = allEvents.Select(e =>
            {
                var assemblyQualifiedName = JsonConvert.DeserializeObject<string>(e.AssemblyQualifiedName);
                var eventType = Type.GetType(assemblyQualifiedName);
                var data = JsonConvert.DeserializeObject(e.Data, eventType);

                return data as IDomainEvent;
            });

            var aggregate = new Patient();
            aggregate.Load(domainEvents);

            return aggregate;
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
                  AggregateId = GetAggregateIdFromPatient(patient.Id),
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

        private static string GetAggregateIdFromPatient(Guid id)
        {
            return $"Patient-{id}";
        }
    }
}