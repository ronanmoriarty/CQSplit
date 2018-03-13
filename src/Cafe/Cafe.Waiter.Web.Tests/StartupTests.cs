using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cafe.Waiter.Web.Controllers;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace Cafe.Waiter.Web.Tests
{
    [TestFixture]
    public class StartupTests
    {
        [Test]
        public void Can_resolve_all_controllers()
        {
            ControllerTypes.ForEach(controllerType =>
            {
                Console.WriteLine($"Resolving {controllerType.Name}...");
                var controller = BuildServiceProvider.Instance.GetService(controllerType);
                Assert.That(controller, Is.Not.Null);
            });
        }

        [Test]
        public void BusControl_is_instantiated_as_singleton()
        {
            var busControl = BuildServiceProvider.Instance.GetService(typeof(IBusControl));
            var busControl2 = BuildServiceProvider.Instance.GetService(typeof(IBusControl));
            Assert.That(ReferenceEquals(busControl, busControl2));
        }

        public IEnumerable<Type> ControllerTypes
        {
            get
            {
                return Assembly.GetAssembly(typeof(MenuController)).GetTypes()
                    .Where(type => typeof(Controller).IsAssignableFrom(type)).ToList();
            }
        }
    }
}
