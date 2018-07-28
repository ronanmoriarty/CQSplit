using System;
using System.Threading.Tasks;
using log4net;
using Microsoft.Extensions.Hosting;

namespace Cafe.Waiter.EventProjecting.Service
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Bootstrapper.Start();
            var logger = LogManager.GetLogger(typeof(Program));
            try
            {
                var service = Container.Instance.Resolve<EventProjectingService>();

                AppDomain.CurrentDomain.ProcessExit += (sender, eventArgs) =>
                {
                    service.Stop();
                };

                service.Start();
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                throw;
            }

            var host = new HostBuilder().Build();

            await host.RunAsync();
        }
    }
}
