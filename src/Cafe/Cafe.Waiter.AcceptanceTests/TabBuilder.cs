using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Cafe.Waiter.AcceptanceTests
{
    public class TabBuilder
    {
        private int _tableNumber;
        private string _waiter;
        private readonly ChromeDriver _chromeDriver;
        private IConfigurationRoot _configurationRoot;

        public TabBuilder()
        {
            _configurationRoot = new ConfigurationBuilder()
                .AddJsonFile("appSettings.json")
                .Build();

            _chromeDriver = new ChromeDriver();
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
            _chromeDriver.Url = $"{root}/app/index.html#!/tabs";
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