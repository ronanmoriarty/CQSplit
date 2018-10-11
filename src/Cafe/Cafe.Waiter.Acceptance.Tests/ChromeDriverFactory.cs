using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace Cafe.Waiter.Acceptance.Tests
{
    public class ChromeDriverFactory : IWebDriverFactory
    {
        public RemoteWebDriver CreateWebDriver()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("headless");
            chromeOptions.AddArguments("disable-gpu");
            chromeOptions.AddArguments("no-sandbox");
            chromeOptions.SetLoggingPreference(LogType.Driver, LogLevel.Debug);
            var chromeService = ChromeDriverService.CreateDefaultService("C:\\tools\\selenium\\");
            chromeService.EnableVerboseLogging = true;
            chromeService.LogPath = "C:\\chromedriver-logs\\chromedriver.log";
            return new ChromeDriver(chromeService, chromeOptions, System.TimeSpan.FromMinutes(3));
        }
    }
}