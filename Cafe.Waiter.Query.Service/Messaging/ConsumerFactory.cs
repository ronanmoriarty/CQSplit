using System;
using CQRSTutorial.Infrastructure;

namespace Cafe.Waiter.Query.Service.Messaging
{
    public class ConsumerFactory : IConsumerFactory
    {
        public object Create(Type typeToCreate)
        {
            return Container.Instance.Resolve(typeToCreate);
        }
    }
}