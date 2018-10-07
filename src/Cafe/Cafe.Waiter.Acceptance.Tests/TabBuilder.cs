using Microsoft.Extensions.Configuration;
using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using LogLevel = OpenQA.Selenium.LogLevel;

namespace Cafe.Waiter.Acceptance.Tests
{
    public class TabBuilder
    {
        private int _tableNumber;
        private string _waiter;
        private readonly ChromeDriver _chromeDriver;
        private readonly IConfigurationRoot _configurationRoot;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public TabBuilder()
        {
            _configurationRoot = new ConfigurationBuilder()
                .AddJsonFile("appSettings.json")
                .Build();

            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("headless");
            // chromeOptions.AddArguments("disable-gpu");
            // chromeOptions.AddArguments("disable-plugins");
            chromeOptions.AddArguments("--enable-logging --v=1");
            // chromeOptions.AddArguments("verbose");
            // chromeOptions.AddArguments("no-sandbox");
            //  chromeOptions.AddArguments("log-level=0");
            //  chromeOptions.AddArguments("log-path=C:\\chromedriver-logs\\chromedriver.log");
            // chromeOptions.AddArguments("disable-dev-shm-usage");
            chromeOptions.SetLoggingPreference(LogType.Driver, LogLevel.Debug);
            var chromeService = ChromeDriverService.CreateDefaultService("C:\\tools\\selenium\\");
            chromeService.EnableVerboseLogging = true;
            chromeService.LogPath = "C:\\chromedriver-logs\\chromedriver.log";
            _chromeDriver = new ChromeDriver(chromeService, chromeOptions, System.TimeSpan.FromMinutes(3));
        }

        public TabBuilder WithTableNumber(int tableNumber)
        {
            _tableNumber = tableNumber;
            return this;
        }

        public TabBuilder WithWaiter(string waiter)
        {
            _waiter = waiter;
            return this;
        }

        public BrowserSession AndSubmit()
        {
            try
            {
                NavigateToOpenTabs();
                SetWaiter();
                SetTableNumber();
                Submit();
                return new BrowserSession(_chromeDriver);
            }
            catch (NoSuchElementException)
            {
                _chromeDriver?.Dispose();
                throw;
            }
        }

        private void NavigateToOpenTabs()
        {
            var root = _configurationRoot["cafe:waiter:web:url"];
            var url = $"{root}/app/index.html#!/tabs";
            _logger.Debug($"Navigating to: {url}");
            _chromeDriver.Url = url;
        }

        private void SetTableNumber()
        {
            SetText("tableNumber", _tableNumber.ToString());
        }

        private void SetWaiter()
        {
            SetText("waiter", _waiter); // TODO: remove setting of waiter by textbox - add login screen.
        }

        private void SetText(string elementId, string text)
        {
            var element = _chromeDriver.FindElementById(elementId);
            element.SendKeys(text);
        }

        private void Submit()
        {
            var createTabButton = _chromeDriver.FindElement(By.XPath("//input[@type=\"submit\"]"));
            createTabButton.Click();
        }
    }
}