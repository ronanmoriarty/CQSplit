using System.Linq;
using MassTransit;

namespace CQSplit.Messaging.Tests.Common
{
    public static class ConsumerRegistrarFactory
    {
        public static ConsumerRegistrar Create(string queueName, params IConsumer[] consumers)
        {
            return new ConsumerRegistrar(
                new PreviouslyConstructedConsumerFactory(consumers),
                new ConsumerTypeProvider(consumers.Select(consumer => consumer.GetType()).ToArray()),
                new ReceiveEndpointConfiguration(queueName));
        }
    }
}