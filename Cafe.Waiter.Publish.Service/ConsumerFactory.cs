using System;
using CQRSTutorial.Messaging;

namespace Cafe.Waiter.Publish.Service
{
    public class ConsumerFactory : IConsumerFactory
    {
        public object Create(Type typeToCreate)
        {
            return Container.Instance.Resolve(typeToCreate);
        }
    }
}
