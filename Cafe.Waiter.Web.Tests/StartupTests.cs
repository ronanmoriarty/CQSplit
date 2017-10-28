using System;
using System.IO;
using Cafe.Waiter.Web.Controllers;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Cafe.Waiter.Web.Tests
{
    [TestFixture]
    public class StartupTests
    {
        private HostingEnvironment _hostingEnvironment;
        private ServiceCollection _services;
        private IServiceProvider _buildServiceProvider;

        [SetUp]
        public void SetUp()
        {
            _hostingEnvironment = CreateHostingEnvironment();
            _services = RegisterControllersButNotTheControllersDependencies();
            WhenServicesConfigured();
        }

        [Test]
        public void Can_resolve_ValuesController()
        {
            var valuesController = (ValuesController)_buildServiceProvider.GetService(typeof(ValuesController));

            Assert.That(valuesController, Is.Not.Null);
        }

        [Test]
        public void Can_resolve_MenuController() // this test is currently failing, because the IMenuRepository dependency can't be resolved yet.
        {
            var menuController = (MenuController)_buildServiceProvider.GetService(typeof(MenuController));

            Assert.That(menuController, Is.Not.Null);
        }

        private HostingEnvironment CreateHostingEnvironment()
        {
            return new HostingEnvironment
            {
                ContentRootPath = GetFolderThatContainsAppSettingsJsonFile()
            };
        }

        private string GetFolderThatContainsAppSettingsJsonFile()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\..\\Cafe.Waiter.Web");
        }

        private ServiceCollection RegisterControllersButNotTheControllersDependencies()
        {
            var services = new ServiceCollection();
            services.AddTransient<ValuesController>();
            services.AddTransient<MenuController>();
            return services;
        }

        private void WhenServicesConfigured()
        {
            new Startup(_hostingEnvironment).ConfigureServices(_services);
            _buildServiceProvider = _services.BuildServiceProvider();
        }
    }
}
