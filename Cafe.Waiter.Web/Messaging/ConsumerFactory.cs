using System;
using CQRSTutorial.Messaging;

namespace Cafe.Waiter.Web.Messaging
{
    public class ConsumerFactory : IConsumerFactory
    {
        public object Create(Type typeToCreate)
        {
            return null; // TODO: refactor to remove this class

            //EndpointProvider relies on RabbitMqMessageBusFactory to provide it an IBusControl to its constructor, so that we can get an ISendEndpoint.
            //But RabbitMqMessageBusFactory takes an IConsumerFactory constructor dependency to configure ReceiveEndpoints.
            //So return null here as it will never be called in the waiter website - we're only sending commands, not receiving any.
        }
    }
}