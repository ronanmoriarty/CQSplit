using System;
using CQRSTutorial.Infrastructure;

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
