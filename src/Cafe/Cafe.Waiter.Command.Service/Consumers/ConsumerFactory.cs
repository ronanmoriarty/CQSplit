using System;
using CQ.Messaging;

namespace Cafe.Waiter.Command.Service.Consumers
{
    public class ConsumerFactory : IConsumerFactory
    {
        public object Create(Type typeToCreate)
        {
            return Container.Instance.Resolve(typeToCreate);
        }
    }
}
