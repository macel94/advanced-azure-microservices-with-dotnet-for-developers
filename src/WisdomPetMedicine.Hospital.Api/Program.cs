using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace WisdomPetMedicine.Hospital.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).ConfigureAppConfiguration((hostContext, builder) =>
                {
                    var isDevelopment = hostContext.HostingEnvironment.IsDevelopment();

                    if (isDevelopment)
                    {
                        builder.AddUserSecrets<Program>();
                    }
                });
    }
}
