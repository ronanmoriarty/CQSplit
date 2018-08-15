using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cafe.Waiter.Web.Controllers;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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
            _hostingEnvironment = CreateHostingEnvironment();
            _serviceCollection = new ServiceCollection();
            WhenServicesConfigured();
            BuildServiceProvider.Instance = _serviceCollection.BuildServiceProvider();
        }

        private HostingEnvironment CreateHostingEnvironment()
        {
            return new HostingEnvironment
            {
                ContentRootPath = AppDomain.CurrentDomain.BaseDirectory,
                EnvironmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"
            };
        }

        private void WhenServicesConfigured()
        {
            new Startup(_hostingEnvironment).ConfigureServices(_serviceCollection);
            RegisterControllersButNotTheControllersDependencies();
        }

        private void RegisterControllersButNotTheControllersDependencies()
        {
            // services.AddMvc() in Startup.ConfigureServices() doesn't seem to register the controller types in a test context for some reason.
            GetControllerTypes()
                .ForEach(controllerType =>
                {
                    _serviceCollection.Add(new ServiceDescriptor(controllerType, controllerType, ServiceLifetime.Transient));
                }
            );
        }

        private IEnumerable<Type> GetControllerTypes()
        {
            return Assembly
                .GetAssembly(typeof(MenuController))
                .GetTypes()
                .Where(type => typeof(Controller).IsAssignableFrom(type))
                .ToList();
        }
    }
}
