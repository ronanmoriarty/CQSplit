using System.IO;
using Microsoft.AspNetCore.Hosting;
using NLog;

namespace Cafe.Waiter.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            LogFilesInCurrentDirectory();

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .Build();

            host.Run();
        }

        private static void LogFilesInCurrentDirectory()
        {
            var logger = LogManager.GetCurrentClassLogger();
            logger.Debug($"Current Directory: {Directory.GetCurrentDirectory()}");
            foreach (var fileSystemEntry in Directory.GetFileSystemEntries(Directory.GetCurrentDirectory()))
            {
                logger.Debug($"{fileSystemEntry}");
            }
        }
    }
}
