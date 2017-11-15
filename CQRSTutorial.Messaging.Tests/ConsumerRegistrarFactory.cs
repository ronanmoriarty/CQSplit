using System;

namespace CQRSTutorial.Messaging.Tests
{
    public static class ConsumerRegistrarFactory
    {
        public static ConsumerRegistrar Create(string queueName, params Type[] consumerTypes)
        {
            return new ConsumerRegistrar(
                new DefaultConstructorConsumerFactory(),
                new ConsumerTypeProvider(consumerTypes),
                new ReceiveEndpointConfiguration(queueName));
        }
    }
}