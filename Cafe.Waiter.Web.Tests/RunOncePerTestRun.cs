﻿using System;
using System.IO;
using System.Reflection;
using Cafe.Waiter.Web.Controllers;
using log4net;
using log4net.Config;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Cafe.Waiter.Web.Tests
{
    [SetUpFixture]
    public class RunOncePerTestRun
    {
        private HostingEnvironment _hostingEnvironment;
        private ServiceCollection _serviceCollection;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var loggerRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(loggerRepository, new FileInfo("log4net.config"));

            _hostingEnvironment = CreateHostingEnvironment();
            _serviceCollection = new ServiceCollection();
            WhenServicesConfigured();
            BuildServiceProvider.Instance = _serviceCollection.BuildServiceProvider();
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

        private void WhenServicesConfigured()
        {
            new Startup(_hostingEnvironment).ConfigureServices(_serviceCollection);
            RegisterControllersButNotTheControllersDependencies();
        }

        private void RegisterControllersButNotTheControllersDependencies()
        {
            // services.AddMvc() in Startup.ConfigureServices() doesn't seem to register the controller types in a test context for some reason.
            _serviceCollection.AddTransient<ValuesController>();
            _serviceCollection.AddTransient<MenuController>();
        }
    }
}
