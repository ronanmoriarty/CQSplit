using Microsoft.Extensions.Configuration;

namespace Cafe.Waiter.Acceptance.Tests
{
    public class CafeWaiterWebsite
    {
        private readonly IConfigurationRoot _configurationRoot;

        public CafeWaiterWebsite(IConfigurationRoot configurationRoot)
        {
            _configurationRoot = configurationRoot;
            CreateTab = new TabBuilder(GetWebDriver(), _configurationRoot);
        }

        public TabBuilder CreateTab { get; }

        private IWebDriverFactory GetWebDriver()
        {
            switch (_configurationRoot["driver"])
            {
                case "firefox":
                    return new FirefoxDriverFactory();
                default:
                    return new ChromeDriverFactory();
            }
        }
    }
}