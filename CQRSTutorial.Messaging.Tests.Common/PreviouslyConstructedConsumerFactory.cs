using System;
using System.Linq;
using MassTransit;

namespace CQRSTutorial.Messaging.Tests.Common
{
    public class PreviouslyConstructedConsumerFactory : IConsumerFactory
    {
        private readonly IConsumer[] _consumers;

        public PreviouslyConstructedConsumerFactory(IConsumer[] consumers)
        {
            _consumers = consumers;
        }

        public object Create(Type typeToCreate)
        {
            return _consumers.Single(typeToCreate.IsInstanceOfType);
        }
    }
}