using System;
using CQRSTutorial.Messaging;

namespace Cafe.Waiter.EventProjecting.Service.Consumers
{
    public class ConsumerFactory : IConsumerFactory
    {
        public object Create(Type typeToCreate)
        {
            return Container.Instance.Resolve(typeToCreate);
        }
    }
}
