using ADP.Assignment.Common.Rest.Interfaces;
using ADP.Assignment.Common.Rest.Policy;
using ADP.Assignment.Common.Rest.Services;
using ADP.Assignment.Domain.Interfaces;
using ADP.Assignment.Domain.Providers;
using ADP.Assignment.Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ADP.Assignment.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IRestService, RestService>();
                    services.AddSingleton<IRestPolicy, RestPolicy>();
                    services.AddSingleton<IMathService, MathService>();
                    services.Configure<MathOptions>(hostContext.Configuration.GetSection("MathOptions"));
                    services.AddHostedService<Worker>();
                });


        //services.AddScoped<IRestService, RestService>()
        //                                .AddSingleton<IRestPolicy, RestPolicy>());
    }
}
