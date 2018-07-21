using Microsoft.Extensions.Configuration;

namespace Cafe.Waiter.Command.Service
{
    public static class ConfigurationRoot
    {
        public static IConfigurationRoot Instance => GetConfigurationRoot();

        private static IConfigurationRoot GetConfigurationRoot()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.override.json", optional: true)
                .Build();
        }
    }
}