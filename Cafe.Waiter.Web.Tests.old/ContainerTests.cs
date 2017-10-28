using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Cafe.Waiter.Web.DependencyInjection;
using MassTransit;
using NUnit.Framework;

namespace Cafe.Waiter.Web.Tests
{
    [TestFixture]
    public class ContainerTests
    {
        [Test]
        public void Can_instantiate_all_controllers()
        {
            var controllerTypes = GetAllControllerTypes();;
            foreach (var controllerType in controllerTypes)
            {
                Console.WriteLine($"Resolving {controllerType}...");
                Container.Instance.Resolve(controllerType);
            }
        }

        private IEnumerable<Type> GetAllControllerTypes()
        {
            return typeof(Web.Api.TabController).Assembly.GetTypes()
                .Where(type => typeof(Controller).IsAssignableFrom(type));
        }

        [Test]
        public void BusControl_is_instantiated_as_singleton()
        {
            var busControl = Container.Instance.Resolve<IBusControl>();
            var busControl2 = Container.Instance.Resolve<IBusControl>();
            Assert.That(ReferenceEquals(busControl, busControl2));
        }
    }
}
