using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;

namespace Cafe.Waiter.Acceptance.Tests
{
    public class FirefoxDriverFactory : IWebDriverFactory
    {
        public RemoteWebDriver CreateWebDriver()
        {
            var firefoxOptions = new FirefoxOptions
            {
                LogLevel = FirefoxDriverLogLevel.Debug
            };
            firefoxOptions.AddArgument("--headless");
            firefoxOptions.SetLoggingPreference(LogType.Driver, LogLevel.Debug);
            firefoxOptions.SetLoggingPreference(LogType.Browser, LogLevel.Debug);

            return new FirefoxDriver(FirefoxDriverService.CreateDefaultService(), firefoxOptions);
        }
    }
}