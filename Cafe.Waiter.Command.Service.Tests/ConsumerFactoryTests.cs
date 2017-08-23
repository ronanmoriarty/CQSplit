using System;
using System.Collections.Generic;
using System.Linq;
using Cafe.Waiter.Command.Service.Messaging;
using MassTransit;
using NUnit.Framework;

namespace Cafe.Waiter.Command.Service.Tests
{
    [TestFixture]
    public class ConsumerFactoryTests
    {
        [Test]
        public void Can_resolve_all_consumers()
        {
            var consumerFactory = new ConsumerFactory();
            var consumerTypes = GetAllConsumerTypes();
            foreach (var consumerType in consumerTypes)
            {
                Console.WriteLine($"Resolving {consumerType.FullName}...");
                var consumer = consumerFactory.Create(consumerType);
                Assert.That(consumer, Is.Not.Null);
            }
        }

        private IEnumerable<Type> GetAllConsumerTypes()
        {
            return typeof(OpenTabConsumer).Assembly.GetTypes()
                .Where(type => IsConsumerType(type) && !type.IsAbstract)
                .ToList();
        }

        private static bool IsConsumerType(Type type)
        {
            return type.Namespace == typeof(OpenTabConsumer).Namespace
                   && type.GetInterfaces().Any(interfaceType => interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IConsumer<>));
        }
    }
}