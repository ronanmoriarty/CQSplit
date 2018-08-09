using System;
using MassTransit;
using NLog;

namespace CQ.Messaging
{
    public class ConsumerRegistrar : IConsumerRegistrar
    {
        private readonly IConsumerFactory _consumerFactory;
        private readonly IConsumerTypeProvider _consumerTypeProvider;
        private readonly IReceiveEndpointConfiguration _receiveEndpointConfiguration;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public ConsumerRegistrar(IConsumerFactory consumerFactory,
            IConsumerTypeProvider consumerTypeProvider,
            IReceiveEndpointConfiguration receiveEndpointConfiguration)
        {
            _consumerFactory = consumerFactory;
            _consumerTypeProvider = consumerTypeProvider;
            _receiveEndpointConfiguration = receiveEndpointConfiguration;
        }

        public void RegisterAllConsumerTypes(Action<ReceiveEndpointArgs> configure)
        {
            var queueName = _receiveEndpointConfiguration.QueueName;
            configure(new ReceiveEndpointArgs(queueName, RegisterAllConsumerTypes));
        }

        private void RegisterAllConsumerTypes(IReceiveEndpointConfigurator receiveEndpointConfigurator)
        {
            var consumerTypes = _consumerTypeProvider.GetConsumerTypes();
            foreach (var consumerType in consumerTypes)
            {
                RegisterConsumerType(receiveEndpointConfigurator, consumerType);
            }
        }

        private void RegisterConsumerType(IReceiveEndpointConfigurator receiveEndpointConfigurator, Type consumerType)
        {
            _logger.Debug($"Register {consumerType.FullName} using {_consumerFactory.GetType().FullName}");
            receiveEndpointConfigurator.Consumer(consumerType, _consumerFactory.Create);
        }
    }
}