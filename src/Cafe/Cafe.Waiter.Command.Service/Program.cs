using System.Threading.Tasks;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Cafe.Waiter.Command.Service
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Bootstrapper.Start();

            var host = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<WaiterCommandService>();
                    services.AddSingleton(Container.Instance);
                })
                .UseServiceProviderFactory(new WindsorServiceProviderFactory())
                .UseConsoleLifetime()
                .Build();

            await host.RunAsync();
        }
    }
}
