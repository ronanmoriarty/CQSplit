using Microsoft.Extensions.Configuration;

namespace Cafe.Waiter.Acceptance.Tests
{
    public class CafeWaiterWebsite
    {
        private static readonly IConfigurationRoot ConfigurationRoot;

        static CafeWaiterWebsite()
        {
            ConfigurationRoot = GetConfigurationRoot();
        }

        public static TabBuilder CreateTab { get; } = new TabBuilder(GetWebDriver(), ConfigurationRoot);

        private static IWebDriverFactory GetWebDriver()
        {
            switch (ConfigurationRoot["driver"])
            {
                case "firefox":
                    return new FirefoxDriverFactory();
                default:
                    return new ChromeDriverFactory();
            }
        }

        private static IConfigurationRoot GetConfigurationRoot()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appSettings.json")
                .Build();
        }
    }
}