using System;
using System.Collections.Generic;
using System.Linq;
using Cafe.Waiter.Command.Service.Consumers;
using CQRSTutorial.Messaging;
using NUnit.Framework;

namespace Cafe.Waiter.Command.Service.Tests
{
    [TestFixture]
    public class ConsumerProviderTests
    {
        private ConsumerTypeProvider _consumerTypeProvider;

        [SetUp]
        public void SetUp()
        {
            _consumerTypeProvider = new ConsumerTypeProvider();
        }

        [Test]
        public void All_commands_have_consumer_types_registered()
        {
            var typesDerivingFromConsumer = GetTypesDerivingFromConsumer();
            foreach (var consumerType in typesDerivingFromConsumer)
            {
                Console.WriteLine($"Checking consumer type {consumerType.FullName}");
                Assert.That(_consumerTypeProvider.GetConsumerTypes().Any(type => type == consumerType));
            }
        }

        private static IEnumerable<Type> GetTypesDerivingFromConsumer()
        {
            return typeof(CloseTabConsumer).Assembly.GetTypes().Where(type =>
            {
                var baseType = type.BaseType;
                return baseType != null
                       && baseType.IsGenericType
                       && baseType.GetGenericTypeDefinition() == typeof(CommandConsumer<>);
            });
        }
    }
}
