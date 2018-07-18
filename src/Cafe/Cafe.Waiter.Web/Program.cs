using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;
using Microsoft.AspNetCore.Hosting;

namespace Cafe.Waiter.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var loggerRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(loggerRepository, new FileInfo("log4net.config"));

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .Build();

            host.Run();
        }
    }
}
