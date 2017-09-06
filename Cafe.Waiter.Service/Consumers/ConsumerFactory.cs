using System;
using CQRSTutorial.Infrastructure;

namespace Cafe.Waiter.Service.Consumers
{
    public class ConsumerFactory : IConsumerFactory
    {
        public object Create(Type typeToCreate)
        {
            return Container.Instance.Resolve(typeToCreate);
        }
    }
}
