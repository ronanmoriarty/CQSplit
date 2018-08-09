using System;
using MassTransit;

namespace CQ.Messaging
{
    public class ReceiveEndpointArgs
    {
        public ReceiveEndpointArgs(string queueName, Action<IReceiveEndpointConfigurator> configure)
        {
            QueueName = queueName;
            Configure = configure;
        }

        public string QueueName { get; }

        public Action<IReceiveEndpointConfigurator> Configure { get; }
    }
}