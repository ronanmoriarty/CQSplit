using Microsoft.Extensions.Configuration;

namespace CQRSTutorial.DAL.Common
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
