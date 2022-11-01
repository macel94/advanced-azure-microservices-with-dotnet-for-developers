using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WisdomPetMedicine.Hospital.Domain.Repositories;
using WisdomPetMedicine.Hospital.Infrastructure;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddSingleton<IPatientAggregateStore, PatientAggregateStore>();
    }) 
    .Build();

host.Run();