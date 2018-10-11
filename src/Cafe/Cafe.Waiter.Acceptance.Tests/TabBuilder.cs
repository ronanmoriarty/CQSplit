using Microsoft.Extensions.Configuration;
using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace Cafe.Waiter.Acceptance.Tests
{
    public class TabBuilder
    {
        private int _tableNumber;
        private string _waiter;
        private readonly RemoteWebDriver _webDriver;
        private readonly IConfigurationRoot _configurationRoot;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public TabBuilder(IWebDriverFactory webDriverFactory,
            IConfigurationRoot configurationRoot)
        {
            _configurationRoot = configurationRoot;
            _webDriver = webDriverFactory.CreateWebDriver();
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
                return new BrowserSession(_webDriver);
            }
            catch (NoSuchElementException)
            {
                _webDriver?.Dispose();
                throw;
            }
        }

        private void NavigateToOpenTabs()
        {
            var root = _configurationRoot["cafe:waiter:web:url"];
            var url = $"{root}/app/index.html#!/tabs";
            _logger.Debug($"Navigating to: {url}");
            _webDriver.Url = url;
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
            var element = _webDriver.FindElementById(elementId);
            element.SendKeys(text);
        }

        private void Submit()
        {
            var createTabButton = _webDriver.FindElement(By.XPath("//input[@type=\"submit\"]"));
            createTabButton.Click();
        }
    }
}